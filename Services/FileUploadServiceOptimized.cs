using FileUpload.Models;
using FileUpload.utils;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace FileUpload.Services
{
    /// <summary>
    /// 优化版文件上传服务（支持大批量文件处理）
    /// 优化点：
    /// 1. 使用生产者-消费者模式，避免一次性加载所有文件
    /// 2. 使用Channel实现异步队列，内存使用恒定
    /// 3. 分批扫描和流式处理，避免创建大量Task
    /// </summary>
    public class FileUploadServiceOptimized
    {
        private readonly AppConfig _config;
        private readonly HttpClient _httpClient;
        private readonly FileNameParser? _fileNameParser;
        private readonly ImageCompressionService? _imageCompressionService;
        private readonly SemaphoreSlim? _semaphore;
        private bool _isRunning;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _uploadTask;
        private Channel<string>? _fileChannel;

        private readonly int _channelCapacity = 10000;

        public FileUploadServiceOptimized(AppConfig config)
        {
            _config = config;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(config.RequestTimeout)
            };

            // 初始化文件名解析器
            if (_config.FileNameParseRules?.Enabled == true)
            {
                _fileNameParser = new FileNameParser(_config.FileNameParseRules);
            }

            // 初始化图片压缩服务
            if (_config.EnableImageCompression)
            {
                _imageCompressionService = new ImageCompressionService(
                    _config.CompressionThresholdKB,
                    _config.CompressionQuality);
                LogManager.LogInfo($"图片压缩已启用: 阈值={_config.CompressionThresholdKB}KB, 质量={_config.CompressionQuality}");
            }

            // 初始化线程池信号量
            if (_config.EnableThreadPool && _config.ThreadPoolSize > 0)
            {
                _semaphore = new SemaphoreSlim(_config.ThreadPoolSize, _config.ThreadPoolSize);
                LogManager.LogInfo($"线程池已启用: 并发数={_config.ThreadPoolSize}");
            }
        }

        /// <summary>
        /// 启动上传服务
        /// </summary>
        public void Start()
        {
            if (_isRunning)
            {
                LogManager.LogWarning("上传服务已在运行中");
                return;
            }

            _isRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();

            // 确保必要的文件夹存在
            EnsureFoldersExist();

            LogManager.LogInfo("上传服务已启动（优化版本）");

            // 显示监控文件夹信息
            var watchFolders = _config.GetWatchFolders();
            if (watchFolders.Count > 0)
            {
                LogManager.LogInfo($"监控文件夹 ({watchFolders.Count} 个):");
                for (int i = 0; i < watchFolders.Count; i++)
                {
                    LogManager.LogInfo($"  [{i + 1}] {watchFolders[i]}");
                }
            }
            else
            {
                LogManager.LogWarning("未配置监控文件夹");
            }

            LogManager.LogInfo($"允许的文件类型: {_config.AllowedExtensions}");

            // 启动文件扫描和上传任务
            _uploadTask = Task.Run(() => ScanAndUploadLoopAsync(_cancellationTokenSource.Token));
        }

        /// <summary>
        /// 停止上传服务
        /// </summary>
        public void Stop()
        {
            if (!_isRunning)
            {
                LogManager.LogWarning("上传服务未运行");
                return;
            }

            _isRunning = false;
            _cancellationTokenSource?.Cancel();

            try
            {
                _uploadTask?.Wait(TimeSpan.FromSeconds(5));
            }
            catch (AggregateException)
            {
                // 忽略取消异常
            }

            LogManager.LogInfo("上传服务已停止");
        }

        /// <summary>
        /// 扫描并上传文件循环（优化版）
        /// </summary>
        private async Task ScanAndUploadLoopAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // 创建Channel（使用BoundedChannel以限制内存使用）
                    var channelOptions = new BoundedChannelOptions(_channelCapacity)
                    {
                        FullMode = BoundedChannelFullMode.Wait,
                        SingleReader = false,
                        SingleWriter = false
                    };
                    _fileChannel = Channel.CreateBounded<string>(channelOptions);

                    // 启动生产者和消费者
                    var watchFolders = _config.GetWatchFolders();

                    var producerTask = Task.Run(() => ProduceFilesAsync(watchFolders, cancellationToken), cancellationToken);

                    // 启动多个消费者（根据并发数）
                    var consumerCount = _config.EnableThreadPool && _config.ThreadPoolSize > 0
                        ? _config.ThreadPoolSize
                        : 1;

                    var consumers = new List<Task>();
                    for (int i = 0; i < consumerCount; i++)
                    {
                        consumers.Add(Task.Run(() => ConsumeFilesAsync(cancellationToken), cancellationToken));
                    }

                    // 等待生产者完成
                    await producerTask;

                    // 等待所有消费者完成
                    _fileChannel?.Writer.Complete();
                    await Task.WhenAll(consumers);

                    LogManager.LogInfo("本轮扫描完成");

                    // ✅ 等待扫描间隔（使用配置的ScanInterval）
                    await Task.Delay(TimeSpan.FromSeconds(_config.ScanInterval), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    LogManager.LogError("扫描文件时发生错误", ex);
                    // 异常情况下也等待扫描间隔，避免频繁错误
                    await Task.Delay(TimeSpan.FromSeconds(_config.ScanInterval), cancellationToken);
                }
            }
        }

        /// <summary>
        /// 生产者：流式扫描文件并分批放入队列（优化大对象问题）
        /// </summary>
        private async Task ProduceFilesAsync(List<string> watchFolders, CancellationToken cancellationToken)
        {
            var allowedExtensions = _config.AllowedExtensions
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(ext => ext.Trim().ToLower())
                .ToHashSet();

            var searchOption = _config.ScanSubfolders
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            const int batchSize = 1000; // 每批1000个文件，避免大对象堆分配
            var batch = new List<string>(batchSize);
            int totalFileCount = 0;

            foreach (var watchFolder in watchFolders)
            {
                if (!Directory.Exists(watchFolder))
                {
                    LogManager.LogWarning($"监控文件夹不存在: {watchFolder}");
                    continue;
                }

                LogManager.LogInfo($"开始扫描: {watchFolder}");

                try
                {
                    // ✅ 流式枚举，不一次性加载所有文件（避免100万/1000万文件时的内存问题）
                    foreach (var file in Directory.EnumerateFiles(watchFolder, "*.*", searchOption))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // 过滤文件
                        var extension = Path.GetExtension(file).ToLower();
                        if (!allowedExtensions.Contains(extension) || IsInOkNgFolder(file))
                            continue;

                        // 放入当前批次
                        batch.Add(file);

                        // 当批次满时，批量写入Channel
                        if (batch.Count >= batchSize)
                        {
                            await WriteBatchToChannel(batch, cancellationToken);
                            totalFileCount += batch.Count;
                            LogManager.LogInfo($"已扫描 {totalFileCount} 个文件，已放入队列...");
                            batch.Clear();
                        }

                        // 每1000个文件报告一次进度（使用总计数）
                        if (totalFileCount % 1000 == 0 && batch.Count == 0)
                        {
                            LogManager.LogInfo($"已扫描 {totalFileCount} 个文件...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError($"扫描文件夹失败: {watchFolder}", ex);
                    continue;
                }
            }

            // 处理剩余文件
            if (batch.Count > 0)
            {
                await WriteBatchToChannel(batch, cancellationToken);
                totalFileCount += batch.Count;
                batch.Clear();
            }

            LogManager.LogInfo($"扫描完成，总共发现 {totalFileCount} 个文件");
        }

        /// <summary>
        /// 批量写入文件路径到Channel
        /// </summary>
        private async Task WriteBatchToChannel(List<string> batch, CancellationToken cancellationToken)
        {
            if (batch.Count == 0)
                return;

            try
            {
                // 逐个写入，确保Channel的流量控制生效
                foreach (var file in batch)
                {
                    await _fileChannel!.Writer.WriteAsync(file, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError("批量写入文件路径到队列时发生错误", ex);
                throw;
            }
        }

        /// <summary>
        /// 消费者：从队列取文件并处理
        /// </summary>
        private async Task ConsumeFilesAsync(CancellationToken cancellationToken)
        {
            try
            {
                await foreach (var filePath in _fileChannel!.Reader.ReadAllAsync(cancellationToken))
                {
                    if (_config.EnableThreadPool && _semaphore != null)
                    {
                        await _semaphore.WaitAsync(cancellationToken);
                        try
                        {
                            await UploadFileWithRetry(filePath);
                        }
                        finally
                        {
                            _semaphore.Release();
                        }
                    }
                    else
                    {
                        await UploadFileWithRetry(filePath);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 消费者被取消，优雅退出
            }
        }

        /// <summary>
        /// 上传文件（带重试）
        /// </summary>
        private async Task UploadFileWithRetry(string filePath)
        {
            int retryCount = 0;
            bool success = false;
            string? errorMessage = null;

            while (retryCount <= _config.MaxRetryCount && !success)
            {
                try
                {
                    // 检查文件是否被占用
                    if (IsFileLocked(filePath))
                    {
                        LogManager.LogWarning($"文件被占用，跳过: {Path.GetFileName(filePath)}");
                        await Task.Delay(1000);
                        continue;
                    }

                    var result = await UploadFileTest(filePath, retryCount);
                    success = result.isSuccess;
                    errorMessage = result.errorMessage;

                    if (!success && retryCount < _config.MaxRetryCount)
                    {
                        retryCount++;
                        LogManager.LogWarning($"上传失败，准备第 {retryCount} 次重试: {Path.GetFileName(filePath)}");
                        await Task.Delay(2000 * retryCount); // 递增延迟
                    }
                    else
                    {
                        break;
                    }
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("图片名称不合法"))
                {
                    // 文件名匹配值为空的异常，直接移到NG文件夹，不重试
                    errorMessage = ex.Message;
                    LogManager.LogError($"文件名解析失败: {Path.GetFileName(filePath)}, 原因: {errorMessage}");

                    // 记录上传日志
                    var fileInfo = new FileInfo(filePath);
                    var log = new UploadLog
                    {
                        UploadTime = DateTime.Now,
                        FileDirectory = Path.GetDirectoryName(filePath) ?? "",
                        FileName = Path.GetFileName(filePath),
                        FileSize = fileInfo.Length,
                        IsSuccess = false,
                        ElapsedMilliseconds = 0,
                        ErrorMessage = errorMessage,
                        DeviceId = _config.DeviceId,
                        RetryCount = 0
                    };
                    LogManager.LogUpload(log);

                    break; // 直接跳出循环，不重试
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    if (retryCount < _config.MaxRetryCount)
                    {
                        retryCount++;
                        await Task.Delay(2000 * retryCount);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // 移动文件到OK或NG文件夹
            MoveFileToDestination(filePath, success, errorMessage);
        }

        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        private bool IsFileLocked(string filePath)
        {
            try
            {
                using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        /// <summary>
        /// 上传单个文件
        /// </summary>
        private async Task<(bool isSuccess, string? errorMessage, int? statusCode)> UploadFile(string filePath, int retryCount)
        {
            var stopwatch = Stopwatch.StartNew();
            var fileName = Path.GetFileName(filePath);
            var fileInfo = new FileInfo(filePath);

            try
            {
                using var form = new MultipartFormDataContent();

                // 构建JSON请求体
                var jsonBody = BuildJsonRequestBody(fileName, filePath);
                form.Add(new StringContent(jsonBody, Encoding.UTF8, "application/json"), _config.JsonRequestBody.FieldName);

                // 添加文件（支持图片压缩）
                byte[] fileData;
                long originalFileSize = fileInfo.Length;
                long uploadFileSize = originalFileSize;

                // 判断是否需要压缩
                if (_config.EnableImageCompression &&
                    _imageCompressionService != null &&
                    ImageCompressionService.IsImageFile(filePath) &&
                    _imageCompressionService.ShouldCompress(filePath))
                {
                    var (compressedData, origSize, compSize) = await _imageCompressionService.CompressImageAsync(filePath);
                    fileData = compressedData;
                    uploadFileSize = compSize;
                }
                else
                {
                    fileData = await File.ReadAllBytesAsync(filePath);
                }

                var fileContent = new ByteArrayContent(fileData);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                form.Add(fileContent, "File", fileName);

                // 发送请求
                var response = await _httpClient.PostAsync(_config.UploadUrl, form);
                stopwatch.Stop();

                var isSuccess = response.IsSuccessStatusCode;
                var statusCode = (int)response.StatusCode;
                string? errorMessage = null;

                var responseContent = await response.Content.ReadAsStringAsync();

                // {"COMMAND":"SENDDATA","LINE":null,"STATION_NAME":null,"BARCODE":"11S55HFF096X6","TOOLINGNO":"R05A_CR0395","CAVITYNO":"A1","MACHINENO":"IMG-TEST-01","LOTNO":"","RESULT":"NG","RESULT_INFO":"设备编号[SENDDATA]未检索到站点信息,请检查!"}
                LogManager.LogInfo($"上传响应: {responseContent}");

                if (!isSuccess)
                {
                    errorMessage = $"HTTP {statusCode}: {responseContent}";
                }
                else
                {
                    var res = JsonSerializer.Deserialize<ApiResponse>(responseContent);
                    if (res != null && res.RESULT == "NG")
                    {
                        isSuccess = false;
                        errorMessage = res.RESULT_INFO;
                    }
                    else if (res != null && res.RESULT == "OK")
                    {
                        isSuccess = true;
                    }
                }

                // 记录日志
                var log = new UploadLog
                {
                    UploadTime = DateTime.Now,
                    FileDirectory = Path.GetDirectoryName(filePath) ?? "",
                    FileName = fileName,
                    FileSize = fileInfo.Length,
                    IsSuccess = isSuccess,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    ErrorMessage = errorMessage,
                    HttpStatusCode = statusCode,
                    DeviceId = _config.DeviceId,
                    RetryCount = retryCount
                };

                LogManager.LogUpload(log);

                return (isSuccess, errorMessage, statusCode);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("图片名称不合法"))
            {
                // 文件名解析失败，直接抛出异常，让上层处理（不重试）
                throw;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                var log = new UploadLog
                {
                    UploadTime = DateTime.Now,
                    FileDirectory = Path.GetDirectoryName(filePath) ?? "",
                    FileName = fileName,
                    FileSize = fileInfo.Length,
                    IsSuccess = false,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    ErrorMessage = ex.Message,
                    DeviceId = _config.DeviceId,
                    RetryCount = retryCount
                };

                LogManager.LogUpload(log);

                return (false, ex.Message, null);
            }
        }

        /// <summary>
        /// 测试上传文件（简化版，实际应调用真实的上传API）
        /// </summary>
        private async Task<(bool isSuccess, string? errorMessage)> UploadFileTest(string filePath, int retryCount)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 这里是实际的上传逻辑
                // 为了测试，我们假设上传总是成功
                await Task.Delay(100); // 模拟网络延迟

                // 记录上传日志
                var fileInfo = new FileInfo(filePath);
                var log = new UploadLog
                {
                    UploadTime = DateTime.Now,
                    FileDirectory = Path.GetDirectoryName(filePath) ?? "",
                    FileName = Path.GetFileName(filePath),
                    FileSize = fileInfo.Length,
                    IsSuccess = true,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    ErrorMessage = "",
                    DeviceId = _config.DeviceId,
                    RetryCount = retryCount
                };
                LogManager.LogUpload(log);

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// 移动文件到OK或NG文件夹
        /// </summary>
        private void MoveFileToDestination(string filePath, bool success, string? errorMessage)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);
                var sourceDir = Path.GetDirectoryName(filePath) ?? "";
                var watchFolder = FindWatchFolder(sourceDir);
                var destinationFolder = Path.Combine(watchFolder, success ? "OK" : "NG");

                // 如果启用目录结构保留
                if (_config.PreserveDirectoryStructure && !sourceDir.Equals(watchFolder, StringComparison.OrdinalIgnoreCase))
                {
                    var relativePath = Path.GetRelativePath(watchFolder, sourceDir);
                    destinationFolder = Path.Combine(destinationFolder, relativePath);
                }

                // 确保目标文件夹存在
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                var destinationPath = Path.Combine(destinationFolder, fileName);

                // 处理文件名冲突
                destinationPath = GetUniqueFilePath(destinationPath);

                // 移动文件
                File.Move(filePath, destinationPath);

                var reason = errorMessage != null ? $", 原因: {errorMessage}" : "";
                LogManager.LogInfo($"文件已移动到 {destinationFolder} 文件夹: {Path.GetFileName(filePath)}{reason}");
            }
            catch (Exception ex)
            {
                LogManager.LogError($"移动文件失败: {filePath}", ex);
            }
        }

        /// <summary>
        /// 查找文件所属的监控文件夹
        /// </summary>
        private string FindWatchFolder(string fileDir)
        {
            var watchFolders = _config.GetWatchFolders();
            foreach (var watchFolder in watchFolders)
            {
                if (fileDir.StartsWith(watchFolder, StringComparison.OrdinalIgnoreCase))
                {
                    return watchFolder;
                }
            }
            // 如果没找到，返回第一个监控文件夹
            return watchFolders.FirstOrDefault() ?? fileDir;
        }

        /// <summary>
        /// 获取唯一的文件路径（如果文件已存在，添加时间戳）
        /// </summary>
        private string GetUniqueFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return filePath;
            }

            var directory = Path.GetDirectoryName(filePath) ?? "";
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");

            return Path.Combine(directory, $"{fileName}_{timestamp}{extension}");
        }

        /// <summary>
        /// 判断文件是否在 OK/NG 文件夹中
        /// </summary>
        private bool IsInOkNgFolder(string filePath)
        {
            var normalizedPath = filePath.Replace('/', '\\').ToLower();
            return normalizedPath.Contains("\\ok\\") || normalizedPath.Contains("\\ng\\");
        }

        /// <summary>
        /// 确保必要的文件夹存在
        /// </summary>
        private void EnsureFoldersExist()
        {
            if (_config.AutoCreateFolders)
            {
                var watchFolders = _config.GetWatchFolders();

                foreach (var watchFolder in watchFolders)
                {
                    var folders = new[]
                    {
                        watchFolder,
                        Path.Combine(watchFolder, "OK"),
                        Path.Combine(watchFolder, "NG")
                    };

                    foreach (var folder in folders)
                    {
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                            LogManager.LogInfo($"已创建文件夹: {folder}");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 构建JSON格式请求体
        /// </summary>
        private string BuildJsonRequestBody(string fileName, string filePath)
        {
            var jsonParams = new Dictionary<string, string>();

            // 1. 添加固定参数
            if (_config.JsonRequestBody.FixedParams != null)
            {
                foreach (var kvp in _config.JsonRequestBody.FixedParams)
                {
                    jsonParams[kvp.Key] = kvp.Value;
                }
            }

            // 2. 从文件名提取参数
            if (_fileNameParser != null)
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                var extractedParams = _fileNameParser.ParseFileName(fileNameWithoutExt);

                // 只添加配置中指定需要提取的参数
                if (_config.JsonRequestBody.ExtractParams != null)
                {
                    foreach (var paramName in _config.JsonRequestBody.ExtractParams)
                    {
                        if (extractedParams.ContainsKey(paramName))
                        {
                            var paramValue = extractedParams[paramName];

                            // 检查是否启用空值拒绝功能
                            if (_config.FileNameParseRules?.RejectEmptyValues == true && string.IsNullOrWhiteSpace(paramValue))
                            {
                                throw new InvalidOperationException($"图片名称不合法: 参数 [{paramName}] 的值为空");
                            }

                            jsonParams[paramName] = paramValue;
                        }
                        else
                        {
                            // 如果启用空值拒绝功能，且参数未提取到，也认为是异常
                            if (_config.FileNameParseRules?.RejectEmptyValues == true)
                            {
                                throw new InvalidOperationException($"图片名称不合法: 参数 [{paramName}] 未能从文件名中提取");
                            }
                        }
                    }
                }
            }

            // 3. 添加自动生成的参数
            if (_config.JsonRequestBody.AutoGenerateParams != null)
            {
                // 获取文件信息（用于多个参数）
                var fileInfo = new FileInfo(filePath);

                foreach (var kvp in _config.JsonRequestBody.AutoGenerateParams)
                {
                    var paramName = kvp.Key;
                    var paramType = kvp.Value.ToUpper();

                    switch (paramType)
                    {
                        case "FILENAME":
                            jsonParams[paramName] = fileName;
                            break;
                        case "CREATETIME":
                            // 从文件创建时间获取
                            jsonParams[paramName] = fileInfo.CreationTime.ToString("yyyyMMddHHmmss");
                            break;
                        case "MODIFYTIME":
                            // 从文件修改时间获取
                            jsonParams[paramName] = fileInfo.LastWriteTime.ToString("yyyyMMddHHmmss");
                            break;
                        case "DATETIME":
                            // DATETIME 使用当前时间（保持原有行为）
                            jsonParams[paramName] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            break;
                        case "COMPUTERNO":
                            jsonParams[paramName] = Environment.MachineName;
                            break;
                        case "COMPUTERIP":
                            jsonParams[paramName] = ToolHelper.GetLocalIPAddress();
                            break;
                        case "LOCAL_PATH":
                            jsonParams[paramName] = Path.GetDirectoryName(filePath) ?? "";
                            break;
                        case "FILESIZE":
                        case "ORI_FILESIZE_KB":
                            var sizeKB = (int)Math.Ceiling(fileInfo.Length / 1024.0);
                            jsonParams[paramName] = sizeKB.ToString();
                            break;
                        default:
                            // 如果是固定值，直接使用
                            jsonParams[paramName] = kvp.Value;
                            break;
                    }
                }
            }

            if (!jsonParams.ContainsKey("MACHINENO") || jsonParams["MACHINENO"].Equals(""))
            {
                jsonParams["MACHINENO"] = _config.DeviceId;
            }

            if (!jsonParams.ContainsKey("COMMAND") || jsonParams["COMMAND"].Equals(""))
            {
                jsonParams["COMMAND"] = _config.Command;
            }

            // 序列化为JSON字符串
            var jsonString = JsonSerializer.Serialize(jsonParams, new JsonSerializerOptions
            {
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            LogManager.LogInfo($"JSON请求体: {jsonString}");
            return jsonString;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Stop();
            _httpClient?.Dispose();
            _cancellationTokenSource?.Dispose();
            _semaphore?.Dispose();
        }
    }
}
