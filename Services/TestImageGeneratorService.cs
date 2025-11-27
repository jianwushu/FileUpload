using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;

namespace FileUpload.Services
{
    /// <summary>
    /// 测试图片生成服务（支持多线程并行生成）
    /// </summary>
    public static class TestImageGeneratorService
    {
        /// <summary>
        /// 生成测试图片到监控目录（多线程并行版本）
        /// </summary>
        /// <param name="watchFolder">监控目录路径</param>
        /// <param name="count">生成数量</param>
        /// <param name="startNumber">起始序号</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="format">图片格式</param>
        /// <param name="maxDegreeOfParallelism">最大并发数</param>
        /// <param name="progress">进度报告</param>
        /// <param name="cancellationToken">取消令牌</param>
        public static void GenerateTestImages(
            string watchFolder,
            int count,
            int startNumber = 1,
            int width = 800,
            int height = 600,
            ImageFormat? format = null,
            int maxDegreeOfParallelism = -1,
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            if (format == null)
            {
                format = ImageFormat.Jpeg;
            }

            // 确保监控目录存在
            if (!Directory.Exists(watchFolder))
            {
                Directory.CreateDirectory(watchFolder);
            }

            // 使用原子计数器保证序号连续性
            int currentNumber = startNumber - 1;
            object lockObj = new object();

            // 进度计数器
            int completedCount = 0;
            int lastProgressPercentage = -1;

            // 配置并行选项
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism <= 0 ? Environment.ProcessorCount : maxDegreeOfParallelism,
                CancellationToken = cancellationToken
            };

            // 并行生成图片
            Parallel.For(0, count, options, i =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                // 使用原子操作获取下一个序号
                int sequenceNumber = Interlocked.Increment(ref currentNumber);

                // 每个线程获取自己的时间戳，避免重复计算
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string filename = $"TEST_{sequenceNumber:D4}_{timestamp}_用于图片上传验证.jpg";
                string fullPath = Path.Combine(watchFolder, filename);

                // 生成单张图片
                GenerateSingleImageFast(fullPath, sequenceNumber, timestamp, width, height, format);

                // 更新进度（原子操作）
                int currentCompleted = Interlocked.Increment(ref completedCount);

                // 每生成100张图片报告一次进度（减少锁竞争）
                int progressPercentage = (int)((double)currentCompleted / count * 100);
                if (progressPercentage != lastProgressPercentage && progressPercentage % 2 == 0)
                {
                    lock (lockObj)
                    {
                        if (progressPercentage > lastProgressPercentage)
                        {
                            progress?.Report(progressPercentage);
                            lastProgressPercentage = progressPercentage;
                        }
                    }
                }
            });

            // 报告100%完成
            progress?.Report(100);
        }

        /// <summary>
        /// 快速生成单张测试图片（优化版本）
        /// </summary>
        private static void GenerateSingleImageFast(
            string filePath,
            int number,
            string timestamp,
            int width,
            int height,
            ImageFormat format)
        {
            // 使用using确保资源释放
            using (Bitmap bitmap = new Bitmap(width, height))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // 设置高质量渲染但降低计算开销
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // 简化背景：使用纯色而非渐变（更快）
                using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(230, 243, 255)))
                {
                    graphics.FillRectangle(bgBrush, 0, 0, width, height);
                }

                // 绘制边框
                using (Pen borderPen = new Pen(Color.FromArgb(100, 150, 200), 2))
                {
                    graphics.DrawRectangle(borderPen, 5, 5, width - 10, height - 10);
                }

                // 绘制标题
                DrawText(graphics, "测试图片", new Font("Microsoft YaHei", 32, FontStyle.Bold),
                    Color.FromArgb(50, 100, 150), width / 2, 50, true);

                // 绘制序号
                DrawText(graphics, $"序号: {number}", new Font("Microsoft YaHei", 22, FontStyle.Bold),
                    Color.FromArgb(80, 80, 80), width / 2, 130, true);

                // 绘制时间戳
                DrawText(graphics, $"时间戳: {timestamp}", new Font("Consolas", 14),
                    Color.FromArgb(100, 100, 100), width / 2, 175, true);

                // 绘制说明文字
                DrawText(graphics, "用于图片上传验证", new Font("Microsoft YaHei", 18),
                    Color.FromArgb(120, 120, 120), width / 2, 210, true);

                // 绘制底部信息
                string bottomText = $"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                DrawText(graphics, bottomText, new Font("Microsoft YaHei", 10),
                    Color.FromArgb(120, 120, 120), width / 2, height - 30, true);

                // 简化装饰元素：只保留必要的装饰
                DrawSimpleDecorations(graphics, width, height);

                // 保存图片
                bitmap.Save(filePath, format);
            }
        }

        /// <summary>
        /// 绘制文本（优化版本）
        /// </summary>
        private static void DrawText(Graphics graphics, string text, Font font, Color color,
            float centerX, float y, bool centerAlign)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                SizeF textSize = graphics.MeasureString(text, font);
                float x = centerAlign ? (centerX - textSize.Width / 2) : centerX;
                graphics.DrawString(text, font, brush, x, y);
            }
        }

        /// <summary>
        /// 绘制简化装饰元素（优化版本）
        /// </summary>
        private static void DrawSimpleDecorations(Graphics graphics, int width, int height)
        {
            // 只绘制少量装饰元素，提高生成速度
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, 150, 200)))
            {
                // 绘制4个角落的圆形装饰
                int[] cornerX = { 20, width - 40, 20, width - 40 };
                int[] cornerY = { 20, 20, height - 40, height - 40 };

                for (int i = 0; i < 4; i++)
                {
                    graphics.FillEllipse(brush, cornerX[i], cornerY[i], 20, 20);
                }
            }
        }
    }
}
