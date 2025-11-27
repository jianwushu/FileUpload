using FileUpload.Models;
using System.Collections.Concurrent;
using System.Text;

namespace FileUpload.Services
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    public class LogManager
    {
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        private static readonly object _fileLock = new object();

        /// <summary>
        /// 日志事件（用于UI显示）
        /// </summary>
        public static event Action<string>? LogReceived;

        static LogManager()
        {
            // 确保日志目录存在
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            // 初始化CSV日志文件（如果不存在则创建表头）
            var csvLogPath = GetCsvLogPath();
            if (!File.Exists(csvLogPath))
            {
                var header = "上传时间,设备编号,文件目录,文件名称,文件大小(字节),成功标识,耗时(ms),HTTP状态码,重试次数,异常原因";
                File.WriteAllText(csvLogPath, header + Environment.NewLine, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 记录上传日志
        /// </summary>
        public static void LogUpload(UploadLog log)
        {
            try
            {
                // 写入文本日志
                var logMessage = log.ToLogString();
                WriteToTextLog(logMessage);

                // 写入CSV日志
                WriteToCsvLog(log.ToCsvString());

                // 异步触发UI更新事件，避免阻塞上传线程
                Task.Run(() => LogReceived?.Invoke(logMessage));
            }
            catch (Exception ex)
            {
                // 日志记录失败时，至少尝试写入错误信息
                WriteToTextLog($"[ERROR] 日志记录失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 记录普通信息日志
        /// </summary>
        public static void LogInfo(string message)
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [INFO] {message}";
            WriteToTextLog(logMessage);
            // 异步触发UI更新事件
            Task.Run(() => LogReceived?.Invoke(logMessage));
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        public static void LogWarning(string message)
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [WARN] {message}";
            WriteToTextLog(logMessage);
            // 异步触发UI更新事件
            Task.Run(() => LogReceived?.Invoke(logMessage));
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        public static void LogError(string message, Exception? ex = null)
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [ERROR] {message}";
            if (ex != null)
            {
                logMessage += $" | 异常: {ex.Message}";
            }
            WriteToTextLog(logMessage);
            // 异步触发UI更新事件
            Task.Run(() => LogReceived?.Invoke(logMessage));
        }

        /// <summary>
        /// 写入文本日志文件
        /// </summary>
        private static void WriteToTextLog(string message)
        {
            try
            {
                var logPath = GetTextLogPath();
                lock (_fileLock)
                {
                    File.AppendAllText(logPath, message + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // 忽略日志写入失败
            }
        }

        /// <summary>
        /// 写入CSV日志文件
        /// </summary>
        private static void WriteToCsvLog(string csvLine)
        {
            try
            {
                var logPath = GetCsvLogPath();
                lock (_fileLock)
                {
                    File.AppendAllText(logPath, csvLine + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // 忽略日志写入失败
            }
        }

        /// <summary>
        /// 获取文本日志文件路径（按日期）
        /// </summary>
        private static string GetTextLogPath()
        {
            var fileName = $"upload_{DateTime.Now:yyyyMMdd}.log";
            return Path.Combine(LogDirectory, fileName);
        }

        /// <summary>
        /// 获取CSV日志文件路径（按月份）
        /// </summary>
        public static string GetCsvLogPath()
        {
            var fileName = $"upload_{DateTime.Now:yyyyMMdd}.csv";
            return Path.Combine(LogDirectory, fileName);
        }

        /// <summary>
        /// 清理旧日志文件（保留最近N天）
        /// </summary>
        public static void CleanOldLogs(int keepDays = 30)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-keepDays);
                var files = Directory.GetFiles(LogDirectory, "*.log")
                    .Concat(Directory.GetFiles(LogDirectory, "*.csv"));

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.LastWriteTime < cutoffDate)
                    {
                        File.Delete(file);
                        LogInfo($"已删除旧日志文件: {fileInfo.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("清理旧日志文件失败", ex);
            }
        }

        /// <summary>
        /// 获取今日统计信息
        /// </summary>
        public static (int total, int success, int failed) GetTodayStatistics()
        {
            try
            {
                var csvPath = GetCsvLogPath();
                if (!File.Exists(csvPath))
                    return (0, 0, 0);

                //var today = DateTime.Now.Date;
                var lines = File.ReadAllLines(csvPath, Encoding.UTF8);
                
                int total = 0, success = 0, failed = 0;

                foreach (var line in lines.Skip(1)) // 跳过表头
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parts = line.Split(',');
                    if (parts.Length < 6)
                        continue;

                    total++;
                    if (parts[5].Contains("成功"))
                        success++;
                    else
                        failed++;
                }

                return (total, success, failed);
            }
            catch
            {
                return (0, 0, 0);
            }
        }
    }
}

