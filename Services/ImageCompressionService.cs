using System.Drawing;
using System.Drawing.Imaging;

namespace FileUpload.Services
{
    /// <summary>
    /// 图片压缩服务
    /// </summary>
    public class ImageCompressionService
    {
        private readonly int _thresholdKB;
        private readonly int _quality;

        public ImageCompressionService(int thresholdKB, int quality)
        {
            _thresholdKB = thresholdKB;
            _quality = Math.Clamp(quality, 1, 100);
        }

        /// <summary>
        /// 判断文件是否需要压缩
        /// </summary>
        public bool ShouldCompress(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                var fileSizeKB = fileInfo.Length / 1024;
                return fileSizeKB > _thresholdKB;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 压缩图片并返回压缩后的字节数组
        /// </summary>
        public async Task<(byte[] compressedData, long originalSize, long compressedSize)> CompressImageAsync(string filePath)
        {
            try
            {
                var originalSize = new FileInfo(filePath).Length;
                
                // 读取原始图片
                using var originalImage = Image.FromFile(filePath);
                
                // 创建内存流保存压缩后的图片
                using var memoryStream = new MemoryStream();
                
                // 获取JPEG编码器
                var jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                
                // 设置压缩质量
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)_quality);
                
                // 保存压缩后的图片到内存流
                originalImage.Save(memoryStream, jpegEncoder, encoderParameters);
                
                var compressedData = memoryStream.ToArray();
                var compressedSize = compressedData.Length;
                
                LogManager.LogInfo($"图片压缩完成: {Path.GetFileName(filePath)}, " +
                    $"原始大小: {originalSize / 1024}KB, " +
                    $"压缩后: {compressedSize / 1024}KB, " +
                    $"压缩率: {(1 - (double)compressedSize / originalSize) * 100:F1}%");
                
                return (compressedData, originalSize, compressedSize);
            }
            catch (Exception ex)
            {
                LogManager.LogError($"图片压缩失败: {Path.GetFileName(filePath)}", ex);
                // 如果压缩失败，返回原始文件数据
                var originalData = await File.ReadAllBytesAsync(filePath);
                return (originalData, originalData.Length, originalData.Length);
            }
        }

        /// <summary>
        /// 获取图片编码器
        /// </summary>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return codecs[0];
        }

        /// <summary>
        /// 判断文件是否为图片
        /// </summary>
        public static bool IsImageFile(string filePath)
        {
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var extension = Path.GetExtension(filePath).ToLower();
            return imageExtensions.Contains(extension);
        }
    }
}

