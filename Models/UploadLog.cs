namespace FileUpload.Models
{
    /// <summary>
    /// 上传日志记录
    /// </summary>
    public class UploadLog
    {
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 文件目录
        /// </summary>
        public string FileDirectory { get; set; } = string.Empty;

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 异常原因
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// HTTP状态码
        /// </summary>
        public int? HttpStatusCode { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 转换为日志字符串
        /// </summary>
        public string ToLogString()
        {
            var status = IsSuccess ? "成功" : "失败";
            var error = string.IsNullOrEmpty(ErrorMessage) ? "" : $" | 错误: {ErrorMessage}";
            var httpCode = HttpStatusCode.HasValue ? $" | HTTP状态码: {HttpStatusCode}" : "";
            var retry = RetryCount > 0 ? $" | 重试次数: {RetryCount}" : "";
            
            return $"[{UploadTime:yyyy-MM-dd HH:mm:ss.fff}] {status} | 设备: {DeviceId} | 文件: {FileName} | " +
                   $"大小: {FormatFileSize(FileSize)} | 耗时: {ElapsedMilliseconds}ms{httpCode}{retry}{error}";
        }

        /// <summary>
        /// 转换为CSV格式
        /// </summary>
        public string ToCsvString()
        {
            return $"{UploadTime:yyyy-MM-dd HH:mm:ss.fff}," +
                   $"{DeviceId}," +
                   $"\"{FileDirectory}\"," +
                   $"\"{FileName}\"," +
                   $"{FileSize}," +
                   $"{(IsSuccess ? "成功" : "失败")}," +
                   $"{ElapsedMilliseconds}," +
                   $"{HttpStatusCode?.ToString() ?? ""}," +
                   $"{RetryCount}," +
                   $"\"{ErrorMessage?.Replace("\"", "\"\"") ?? ""}\"";
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
}

