using FileUpload.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FileUpload.Services
{
    /// <summary>
    /// 配置文件管理器
    /// </summary>
    public class ConfigManager
    {
        private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.yml");
        private static AppConfig? _cachedConfig;

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public static AppConfig LoadConfig()
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    // 如果配置文件不存在，创建默认配置
                    var defaultConfig = new AppConfig();
                    SaveConfig(defaultConfig);
                    _cachedConfig = defaultConfig;
                    return defaultConfig;
                }

                var yaml = File.ReadAllText(ConfigFilePath);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                _cachedConfig = deserializer.Deserialize<AppConfig>(yaml);
                return _cachedConfig ?? new AppConfig();
            }
            catch (Exception ex)
            {
                throw new Exception($"加载配置文件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public static void SaveConfig(AppConfig config)
        {
            try
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var yaml = serializer.Serialize(config);
                File.WriteAllText(ConfigFilePath, yaml);
                _cachedConfig = config;
            }
            catch (Exception ex)
            {
                throw new Exception($"保存配置文件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取缓存的配置（如果没有则加载）
        /// </summary>
        public static AppConfig GetConfig()
        {
            return _cachedConfig ?? LoadConfig();
        }

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public static AppConfig ReloadConfig()
        {
            _cachedConfig = null;
            return LoadConfig();
        }

        /// <summary>
        /// 验证配置是否有效
        /// </summary>
        public static (bool isValid, string errorMessage) ValidateConfig(AppConfig config)
        {
            if (string.IsNullOrWhiteSpace(config.DeviceId))
                return (false, "设备编号不能为空");

            if (string.IsNullOrWhiteSpace(config.UploadUrl))
                return (false, "上传接口URL不能为空");

            if (!Uri.TryCreate(config.UploadUrl, UriKind.Absolute, out _))
                return (false, "上传接口URL格式不正确");

            if (string.IsNullOrWhiteSpace(config.AllowedExtensions))
                return (false, "允许的文件后缀不能为空");

            if (config.RequestTimeout <= 0)
                return (false, "请求超时时间必须大于0");

            if (config.ScanInterval <= 0)
                return (false, "文件扫描间隔必须大于0");

            // 验证文件名解析规则
            if (config.FileNameParseRules != null && config.FileNameParseRules.Enabled)
            {
                var (isValid, errorMessage) = FileNameParser.ValidateRules(config.FileNameParseRules);
                if (!isValid)
                    return (false, $"文件名解析规则配置错误: {errorMessage}");
            }

            // 验证JSON请求体配置
            if (config.JsonRequestBody == null)
                return (false, "必须配置jsonRequestBody");

            if (string.IsNullOrWhiteSpace(config.JsonRequestBody.FieldName))
                return (false, "JSON字段名不能为空");

            return (true, string.Empty);
        }
    }
}

