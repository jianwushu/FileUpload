using System.Text;



namespace FileUpload.Services
{
    public class LogMonitor
    {
        private static long _lastPosition = 0;
        private static DateTime _currentDate = DateTime.MinValue;
        private static (int total, int success, int failed) _todayStats = (0, 0, 0);
        private static readonly object _lock = new object();

        /// <summary>
        /// 更新并获取今日的最新统计信息（增量读取）
        /// </summary>
        /// <returns>今日累计的统计数据</returns>
        public static (int total, int success, int failed) UpdateAndGetTodayStatistics()
        {
            lock (_lock)
            {
                var now = DateTime.Now;
                // 如果是新的一天，重置所有状态
                if (_currentDate != now.Date)
                {
                    _currentDate = now.Date;
                    _todayStats = (0, 0, 0);
                    _lastPosition = 0; // 文件也可能被归档，从头开始
                                       // 注意：如果日志文件按天轮转，这里需要更新为当天的文件名
                }

                try
                {
                    var csvPath = LogManager.GetCsvLogPath(); // 假设路径固定
                    if (!File.Exists(csvPath)) return _todayStats;

                    using (var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(fs, Encoding.UTF8))
                    {
                        // 如果文件被截断或替换（大小变小），从头开始读
                        if (fs.Length < _lastPosition)
                        {
                            _lastPosition = 0;
                            _todayStats = (0, 0, 0); // 数据已重置，重新统计
                        }

                        // 定位到上次读取结束的位置
                        fs.Seek(_lastPosition, SeekOrigin.Begin);

                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            var parts = line.Split(',');
                            if (parts.Length < 6) continue;

                            // 由于我们是增量读取，可以假设新写入的都是今天的
                            _todayStats.total++;
                            if ("成功".Equals(parts[5].Trim(), StringComparison.OrdinalIgnoreCase))
                                _todayStats.success++;
                            else
                                _todayStats.failed++;
                        }

                        // 更新下次开始读取的位置
                        _lastPosition = fs.Position;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"增量读取日志时出错: {ex.Message}");
                }

                return _todayStats;
            }
        }
    }
}
