using YamlDotNet.Serialization;

namespace FileUpload.Models
{
    /// <summary>
    /// 应用程序配置类
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [YamlMember(Alias = "deviceId")]
        public string DeviceId { get; set; } = "DEVICE_001";

        /// <summary>
        /// 上传接口URL
        /// </summary>
        [YamlMember(Alias = "uploadUrl")]
        public string UploadUrl { get; set; } = "http://localhost:5000/api/upload";

        /// <summary>
        /// 监控的文件夹路径（单个文件夹，向后兼容）
        /// </summary>
        [YamlMember(Alias = "watchFolder")]
        public string? WatchFolder { get; set; } = "D:\\Upload\\Watch";

        /// <summary>
        /// 监控的文件夹列表（支持多个文件夹）
        /// </summary>
        [YamlMember(Alias = "watchFolders")]
        public List<string>? WatchFolders { get; set; }

        /// <summary>
        /// 是否扫描子文件夹（递归扫描）
        /// </summary>
        [YamlMember(Alias = "scanSubfolders")]
        public bool ScanSubfolders { get; set; } = false;

        /// <summary>
        /// 移动到OK/NG文件夹时是否保留目录结构
        /// </summary>
        [YamlMember(Alias = "preserveDirectoryStructure")]
        public bool PreserveDirectoryStructure { get; set; } = false;

        /// <summary>
        /// 允许的文件后缀（逗号分隔）
        /// </summary>
        [YamlMember(Alias = "allowedExtensions")]
        public string AllowedExtensions { get; set; } = ".jpg,.jpeg,.png,.gif,.bmp,.pdf,.doc,.docx,.xls,.xlsx";

        /// <summary>
        /// HTTP请求超时时间（秒）
        /// </summary>
        [YamlMember(Alias = "requestTimeout")]
        public int RequestTimeout { get; set; } = 30;

        /// <summary>
        /// 文件扫描间隔（秒）
        /// </summary>
        [YamlMember(Alias = "scanInterval")]
        public int ScanInterval { get; set; } = 5;

        /// <summary>
        /// 最大重试次数
        /// </summary>
        [YamlMember(Alias = "maxRetryCount")]
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// 是否自动创建OK/NG文件夹
        /// </summary>
        [YamlMember(Alias = "autoCreateFolders")]
        public bool AutoCreateFolders { get; set; } = true;

        /// <summary>
        /// 上传命令
        /// </summary>
        [YamlMember(Alias = "command")]
        public string Command { get; set; } = "";

        /// <summary>
        /// JSON请求体参数配置
        /// </summary>
        [YamlMember(Alias = "jsonRequestBody")]
        public JsonRequestBodyConfig JsonRequestBody { get; set; } = new JsonRequestBodyConfig();

        /// <summary>
        /// 文件名解析规则
        /// </summary>
        [YamlMember(Alias = "fileNameParseRules")]
        public FileNameParseRules FileNameParseRules { get; set; } = new FileNameParseRules();

        /// <summary>
        /// 是否启用线程池
        /// </summary>
        [YamlMember(Alias = "enableThreadPool")]
        public bool EnableThreadPool { get; set; } = false;

        /// <summary>
        /// 线程池大小（并发处理的文件数量）
        /// </summary>
        [YamlMember(Alias = "threadPoolSize")]
        public int ThreadPoolSize { get; set; } = 3;

        /// <summary>
        /// 是否启用图片压缩
        /// </summary>
        [YamlMember(Alias = "enableImageCompression")]
        public bool EnableImageCompression { get; set; } = false;

        /// <summary>
        /// 图片压缩阈值（KB），大于此值的图片将被压缩
        /// </summary>
        [YamlMember(Alias = "compressionThresholdKB")]
        public int CompressionThresholdKB { get; set; } = 500;

        /// <summary>
        /// 图片压缩质量（1-100），值越小压缩率越高
        /// </summary>
        [YamlMember(Alias = "compressionQuality")]
        public int CompressionQuality { get; set; } = 75;

        /// <summary>
        /// 系统托盘配置
        /// </summary>
        [YamlMember(Alias = "systemTray")]
        public SystemTrayConfig SystemTray { get; set; } = new SystemTrayConfig();

        /// <summary>
        /// 界面语言设置（zh-CN, zh-Hant, ja, vi, en）
        /// </summary>
        [YamlMember(Alias = "language")]
        public string Language { get; set; } = "zh-CN";

        /// <summary>
        /// 服务注册配置
        /// </summary>
        [YamlMember(Alias = "serviceRegistration")]
        public ServiceRegistrationConfig ServiceRegistration { get; set; } = new ServiceRegistrationConfig();

        /// <summary>
        /// 获取所有监控文件夹（兼容旧配置）
        /// </summary>
        /// <returns>监控文件夹列表</returns>
        public List<string> GetWatchFolders()
        {
            // 优先使用 watchFolders 配置
            if (WatchFolders != null && WatchFolders.Count > 0)
            {
                return WatchFolders;
            }

            // 兼容旧的 watchFolder 配置
            if (!string.IsNullOrEmpty(WatchFolder))
            {
                return new List<string> { WatchFolder };
            }

            // 默认返回空列表
            return new List<string>();
        }
    }

    /// <summary>
    /// JSON请求体配置
    /// </summary>
    public class JsonRequestBodyConfig
    {
        /// <summary>
        /// JSON字段名称（默认为strjson）
        /// </summary>
        [YamlMember(Alias = "fieldName")]
        public string FieldName { get; set; } = "strjson";

        /// <summary>
        /// 固定参数（不从文件名提取的参数）
        /// </summary>
        [YamlMember(Alias = "fixedParams")]
        public Dictionary<string, string> FixedParams { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 需要从文件名提取的参数列表
        /// </summary>
        [YamlMember(Alias = "extractParams")]
        public List<string> ExtractParams { get; set; } = new List<string>();

        /// <summary>
        /// 自动生成的参数配置
        /// </summary>
        [YamlMember(Alias = "autoGenerateParams")]
        public Dictionary<string, string> AutoGenerateParams { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// 文件名解析规则
    /// </summary>
    public class FileNameParseRules
    {
        /// <summary>
        /// 是否启用文件名解析
        /// </summary>
        [YamlMember(Alias = "enabled")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// 文件名分隔符
        /// </summary>
        [YamlMember(Alias = "separator")]
        public string Separator { get; set; } = "_";

        /// <summary>
        /// 位置映射规则（参数名 -> 位置或范围）
        /// 支持两种格式:
        /// 1. 单个位置: "0" 表示第0个分段
        /// 2. 范围: "2-3" 表示从第2段到第3段合并
        /// 例如:
        ///   MACHINENO: "0"
        ///   BARCODE: "1"
        ///   TOOLINGNO: "2-3"
        ///   CAVITYNO: "4"
        /// </summary>
        [YamlMember(Alias = "positionMapping")]
        public Dictionary<string, string> PositionMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 正则表达式解析规则（参数名 -> 正则表达式）
        /// 例如: BARCODE: "BARCODE-(.+?)" 表示从文件名中匹配BARCODE-后面的内容
        /// </summary>
        [YamlMember(Alias = "regexMapping")]
        public Dictionary<string, string> RegexMapping { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 解析模板（使用占位符的方式定义解析规则）
        /// 例如: "{MACHINENO}_{BARCODE}_{TOOLINGNO}_{CAVITYNO}_{CREATETIME}_{}_{}_{FILETYPE}_{FOLDERTYPE}"
        /// </summary>
        [YamlMember(Alias = "template")]
        public string Template { get; set; } = "";

        /// <summary>
        /// 是否拒绝空值（当文件名匹配值为空时，认为图片生成异常，移到NG文件夹）
        /// </summary>
        [YamlMember(Alias = "rejectEmptyValues")]
        public bool RejectEmptyValues { get; set; } = false;
    }

    /// <summary>
    /// 系统托盘配置
    /// </summary>
    public class SystemTrayConfig
    {
        /// <summary>
        /// 是否启用系统托盘功能
        /// </summary>
        [YamlMember(Alias = "enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 最小化时是否自动隐藏到托盘
        /// </summary>
        [YamlMember(Alias = "minimizeToTray")]
        public bool MinimizeToTray { get; set; } = true;

        /// <summary>
        /// 关闭窗口时的默认行为
        /// minimize: 最小化到托盘
        /// exit: 直接退出
        /// ask: 询问用户
        /// </summary>
        [YamlMember(Alias = "closeAction")]
        public string CloseAction { get; set; } = "ask";
    }

    /// <summary>
    /// 服务注册配置
    /// </summary>
    public class ServiceRegistrationConfig
    {
        /// <summary>
        /// 服务中心API地址
        /// </summary>
        [YamlMember(Alias = "serviceCenterApi")]
        public string ServiceCenterApi { get; set; } = "http://localhost:8080/api/service-center";

        /// <summary>
        /// 是否自动注册服务
        /// </summary>
        [YamlMember(Alias = "autoRegister")]
        public bool AutoRegister { get; set; } = false;

        /// <summary>
        /// 是否启用心跳
        /// </summary>
        [YamlMember(Alias = "enableHeartbeat")]
        public bool EnableHeartbeat { get; set; } = true;

        /// <summary>
        /// 心跳间隔（秒），即TTL时间
        /// </summary>
        [YamlMember(Alias = "heartbeatInterval")]
        public int HeartbeatInterval { get; set; } = 30;

        /// <summary>
        /// 服务注册状态
        /// </summary>
        [YamlMember(Alias = "registrationStatus")]
        public string RegistrationStatus { get; set; } = "未注册";

        /// <summary>
        /// 最后注册时间
        /// </summary>
        [YamlMember(Alias = "lastRegistrationTime")]
        public DateTime? LastRegistrationTime { get; set; }

        /// <summary>
        /// 最后心跳时间
        /// </summary>
        [YamlMember(Alias = "lastHeartbeatTime")]
        public DateTime? LastHeartbeatTime { get; set; }
    }
}

