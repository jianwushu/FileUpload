using FileUpload.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FileUpload.Services
{
    /// <summary>
    /// 文件上传服务
    /// </summary>
    public class FileUploadService
    {
        private readonly AppConfig _config;
        private readonly HttpClient _httpClient;
        private readonly FileNameParser? _fileNameParser;
        private readonly ImageCompressionService? _imageCompressionService;
        private readonly SemaphoreSlim? _semaphore;
        private bool _isRunning;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _uploadTask;

        public FileUploadService(AppConfig config)
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

            LogManager.LogInfo("上传服务已启动");

            // 显示监控文件夹信息
            var watchFolders = _config.GetWatchFolders();
            if (watchFolders.Count == 1)
            {
                LogManager.LogInfo($"监控文件夹: {watchFolders[0]}");
            }
            else if (watchFolders.Count > 1)
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

            // 启动文件扫描任务
            _uploadTask = Task.Run(() => ScanAndUploadLoop(_cancellationTokenSource.Token));
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
        /// 扫描并上传文件循环
        /// </summary>
        private async Task ScanAndUploadLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await ScanAndUploadFiles();
                    await Task.Delay(TimeSpan.FromSeconds(_config.ScanInterval), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    LogManager.LogError("扫描文件时发生错误", ex);
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }

        /// <summary>
        /// 扫描并上传文件
        /// </summary>
        private async Task ScanAndUploadFiles()
        {
            var watchFolders = _config.GetWatchFolders();

            if (watchFolders.Count == 0)
            {
                LogManager.LogWarning("未配置监控文件夹");
                return;
            }

            var allFiles = new List<string>();

            // 扫描所有监控文件夹
            foreach (var watchFolder in watchFolders)
            {
                if (!Directory.Exists(watchFolder))
                {
                    LogManager.LogWarning($"监控文件夹不存在: {watchFolder}");
                    continue;
                }

                var files = ScanFolder(watchFolder);
                allFiles.AddRange(files);
            }

            if (allFiles.Count > 0)
            {
                LogManager.LogInfo($"发现 {allFiles.Count} 个待上传文件");
            }

            // 如果启用了线程池，使用并发处理
            if (_config.EnableThreadPool && _semaphore != null)
            {
                var tasks = allFiles.Select(filePath => ProcessFileWithSemaphore(filePath)).ToList();
                await Task.WhenAll(tasks);
            }
            else
            {
                // 串行处理
                foreach (var filePath in allFiles)
                {
                    await UploadFileWithRetry(filePath);
                }
            }
        }

        /// <summary>
        /// 扫描单个文件夹
        /// </summary>
        /// <param name="folder">文件夹路径</param>
        /// <returns>文件列表</returns>
        private List<string> ScanFolder(string folder)
        {
            var allowedExtensions = _config.AllowedExtensions
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(ext => ext.Trim().ToLower())
                .ToHashSet();

            var searchOption = _config.ScanSubfolders
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            var files = Directory.GetFiles(folder, "*.*", searchOption)
                .Where(f => allowedExtensions.Contains(Path.GetExtension(f).ToLower()))
                .Where(f => !IsInOkNgFolder(f))  // 排除 OK/NG 文件夹
                .ToList();

            return files;
        }

        /// <summary>
        /// 判断文件是否在 OK/NG 文件夹中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否在 OK/NG 文件夹中</returns>
        private bool IsInOkNgFolder(string filePath)
        {
            var normalizedPath = filePath.Replace('/', '\\').ToLower();
            return normalizedPath.Contains("\\ok\\") || normalizedPath.Contains("\\ng\\");
        }

        /// <summary>
        /// 使用信号量控制并发的文件处理
        /// </summary>
        private async Task ProcessFileWithSemaphore(string filePath)
        {
            await _semaphore!.WaitAsync();
            try
            {
                await UploadFileWithRetry(filePath);
            }
            finally
            {
                _semaphore.Release();
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

                    var result = await UploadFile(filePath, retryCount);
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
            MoveFile(filePath, success, errorMessage);
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
                    }else if (res != null && res.RESULT == "OK")
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
                            jsonParams[paramName] = GetLocalIPAddress();
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

            if(!jsonParams.ContainsKey("MACHINENO") || jsonParams["MACHINENO"].Equals(""))
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
        /// 获取本机IP地址
        /// </summary>
        private string GetLocalIPAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch
            {
                // 忽略错误
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 移动文件到OK或NG文件夹
        /// </summary>
        private void MoveFile(string filePath, bool success, string? errorMessage = null)
        {
            try
            {
                // 找到文件所属的监控文件夹
                var watchFolder = FindWatchFolder(filePath);
                if (watchFolder == null)
                {
                    LogManager.LogError($"无法确定文件所属的监控文件夹: {filePath}");
                    return;
                }

                var targetFolder = success ? "OK" : "NG";
                string targetPath;

                if (_config.PreserveDirectoryStructure)
                {
                    // 保留目录结构
                    var relativePath = Path.GetRelativePath(watchFolder, filePath);
                    targetPath = Path.Combine(watchFolder, targetFolder, relativePath);

                    // 确保目标目录存在
                    var targetDir = Path.GetDirectoryName(targetPath);
                    if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }
                }
                else
                {
                    // 不保留目录结构，直接放到 OK/NG 根目录
                    var fileName = Path.GetFileName(filePath);
                    targetPath = Path.Combine(watchFolder, targetFolder, fileName);
                }

                // 处理文件名冲突
                if (File.Exists(targetPath))
                {
                    var dir = Path.GetDirectoryName(targetPath) ?? "";
                    var nameWithoutExt = Path.GetFileNameWithoutExtension(targetPath);
                    var ext = Path.GetExtension(targetPath);
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    targetPath = Path.Combine(dir, $"{nameWithoutExt}_{timestamp}{ext}");
                }

                File.Move(filePath, targetPath);

                // 记录日志
                var reason = errorMessage != null ? $", 原因: {errorMessage}" : "";
                LogManager.LogInfo($"文件已移动到 {targetFolder} 文件夹: {Path.GetFileName(filePath)}{reason}");
            }
            catch (Exception ex)
            {
                LogManager.LogError($"移动文件失败: {filePath}", ex);
            }
        }

        /// <summary>
        /// 查找文件所属的监控文件夹
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>监控文件夹路径，如果未找到则返回 null</returns>
        private string? FindWatchFolder(string filePath)
        {
            var watchFolders = _config.GetWatchFolders();
            var normalizedFilePath = Path.GetFullPath(filePath);

            // 按路径长度降序排序，确保优先匹配最具体的路径
            // 这样可以避免 D:\Upload\Watch 错误匹配 D:\Upload\Watch2 中的文件
            var sortedFolders = watchFolders
                .Select(folder => Path.GetFullPath(folder))
                .OrderByDescending(folder => folder.Length)
                .ToList();

            foreach (var normalizedFolder in sortedFolders)
            {
                // 确保路径分隔符一致，并检查是否是真正的子路径
                var folderWithSeparator = normalizedFolder.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                var filePathWithSeparator = normalizedFilePath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;

                if (filePathWithSeparator.StartsWith(folderWithSeparator, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(normalizedFilePath, normalizedFolder.TrimEnd(Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                {
                    return normalizedFolder.TrimEnd(Path.DirectorySeparatorChar);
                }
            }

            return null;
        }

        /// <summary>
        /// 检查文件是否被占用
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

