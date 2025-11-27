using FileUpload.Models;
using FileUpload.Services;
using FileUpload.utils;

namespace FileUpload
{
    public partial class Form1 : Form
    {
        private AppConfig? _config;
        private FileUploadService? _uploadService;
        private ServiceHeartbeatManager? _heartbeatManager;
        private System.Windows.Forms.Timer? _statsTimer;
        private NotifyIcon? _notifyIcon;
        private const int MaxLogLines = 1000;

        public Form1()
        {
            InitializeComponent();
            InitializeNotifyIcon();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // 加载配置
                LoadConfiguration();

                // 订阅日志事件
                LogManager.LogReceived += OnLogReceived;

                // 初始化统计定时器
                _statsTimer = new System.Windows.Forms.Timer();
                _statsTimer.Interval = 2000; // 每2秒更新一次统计
                _statsTimer.Tick += UpdateStatistics;
                _statsTimer.Start();

                // 初始化心跳管理器
                InitializeHeartbeatManager();

                LogManager.LogInfo("程序启动成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"程序初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化系统托盘图标
        /// </summary>
        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = this.Icon ?? SystemIcons.Application;
            _notifyIcon.Text = "文件上传系统";
            _notifyIcon.Visible = false;

            // 双击托盘图标恢复窗口
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            // 创建托盘菜单
            var contextMenu = new ContextMenuStrip();

            contextMenu.Items.Add("显示主窗口", null, (s, e) => ShowMainWindow());
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("开始任务", null, (s, e) => btnStart_Click(s!, e));
            contextMenu.Items.Add("停止任务", null, (s, e) => btnStop_Click(s!, e));
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("打开配置", null, (s, e) => menuConfig_Click(s!, e));
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("退出程序", null, (s, e) => ExitApplication());

            _notifyIcon.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// 托盘图标双击事件
        /// </summary>
        private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            ShowMainWindow();
        }

        /// <summary>
        /// 显示主窗口
        /// </summary>
        private void ShowMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
            }
        }

        /// <summary>
        /// 退出应用程序
        /// </summary>
        private void ExitApplication()
        {
            // 停止服务
            if (_uploadService != null)
            {
                _uploadService.Stop();
                _uploadService = null;
            }

            // 停止心跳
            if (_heartbeatManager != null)
            {
                _heartbeatManager.StopHeartbeat();
                _heartbeatManager.Dispose();
                _heartbeatManager = null;
            }

            // 隐藏托盘图标
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
            }

            Application.Exit();
        }

        private void LoadConfiguration()
        {
            try
            {
                _config = ConfigManager.LoadConfig();

                // 验证配置
                var (isValid, errorMessage) = ConfigManager.ValidateConfig(_config);
                if (!isValid)
                {
                    MessageBox.Show($"配置文件验证失败: {errorMessage}\n\n请检查 config.yml 文件",
                        "配置错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 显示配置信息
                lblDeviceIdValue.Text = _config.DeviceId;

                // 显示监控文件夹（支持多个）
                var watchFolders = _config.GetWatchFolders();
                if (watchFolders.Count > 0)
                {
                    lblWatchFolderValue.Text = watchFolders.Count == 1
                        ? watchFolders[0]
                        : $"{watchFolders[0]} 等 {watchFolders.Count} 个文件夹";
                }
                else
                {
                    lblWatchFolderValue.Text = "未配置";
                }

                lblUploadUrlValue.Text = _config.UploadUrl;

                // 显示解析规则
                DisplayParseRule();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _config = new AppConfig(); // 使用默认配置
            }
        }

        /// <summary>
        /// 初始化心跳管理器
        /// </summary>
        private void InitializeHeartbeatManager()
        {
            try
            {
                if (_config == null)
                {
                    return;
                }

                // 创建心跳管理器
                _heartbeatManager = new ServiceHeartbeatManager();

                // 订阅心跳状态变化事件
                _heartbeatManager.HeartbeatStatusChanged += OnHeartbeatStatusChanged;

                // 如果配置了自动注册，则自动注册服务
                if (_config.ServiceRegistration.AutoRegister)
                {
                    Task.Run(async () => await AutoRegisterService());
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError($"初始化心跳管理器失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 自动注册服务
        /// </summary>
        private async Task AutoRegisterService()
        {
            try
            {
                if (_config == null || string.IsNullOrWhiteSpace(_config.ServiceRegistration.ServiceCenterApi))
                {
                    LogManager.LogWarning("服务中心API未配置，跳过自动注册");
                    return;
                }

                LogManager.LogInfo("正在自动注册服务...");

                var result = await ApiHelper.RegisterServiceDiscoveryAsync(
                    _config.ServiceRegistration.ServiceCenterApi,
                    _config.DeviceId);

                if (result.Success)
                {
                    _config.ServiceRegistration.RegistrationStatus = "注册成功";
                    _config.ServiceRegistration.LastRegistrationTime = DateTime.Now;
                    ConfigManager.SaveConfig(_config);

                    LogManager.LogInfo("服务自动注册成功");

                    // 启动心跳
                    _heartbeatManager?.SetRegistrationStatus(true);
                }
                else
                {
                    LogManager.LogError($"服务自动注册失败: {result.StatusCode} - {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                LogManager.LogError($"服务自动注册异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 心跳状态变化事件处理
        /// </summary>
        private void OnHeartbeatStatusChanged(object? sender, HeartbeatStatusEventArgs e)
        {
            // 在UI线程上更新
            if (InvokeRequired)
            {
                Invoke(new Action<object?, HeartbeatStatusEventArgs>(OnHeartbeatStatusChanged), sender, e);
                return;
            }

            // 可以在这里更新UI显示心跳状态
            // 例如在状态栏显示最后心跳时间等
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (_config == null)
                {
                    MessageBox.Show("配置未加载，无法启动服务", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 再次验证配置
                var (isValid, errorMessage) = ConfigManager.ValidateConfig(_config);
                if (!isValid)
                {
                    MessageBox.Show($"配置验证失败: {errorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 创建并启动上传服务
                _uploadService = new FileUploadService(_config);
                _uploadService.Start();

                // 更新UI状态
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                lblStatusValue.Text = Resources.Strings.Status_Running;
                lblStatusValue.ForeColor = Color.FromArgb(39, 174, 96);

                LogManager.LogInfo("上传任务已启动");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动服务失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogManager.LogError("启动服务失败", ex);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (_uploadService != null)
                {
                    _uploadService.Stop();
                    _uploadService.Dispose();
                    _uploadService = null;
                }

                // 更新UI状态
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                lblStatusValue.Text = Resources.Strings.Status_NotRunning;
                lblStatusValue.ForeColor = Color.FromArgb(192, 57, 43);

                LogManager.LogInfo("上传任务已停止");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止服务失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogManager.LogError("停止服务失败", ex);
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void OnLogReceived(string logMessage)
        {
            // 使用Invoke确保在UI线程上更新
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnLogReceived), logMessage);
                return;
            }

            // 添加日志到文本框
            txtLog.AppendText(logMessage + Environment.NewLine);

            // 限制日志行数
            var lines = txtLog.Lines;
            if (lines.Length > MaxLogLines)
            {
                txtLog.Lines = lines.Skip(lines.Length - MaxLogLines).ToArray();
            }

            // 自动滚动到底部
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }

        private void UpdateStatistics(object? sender, EventArgs e)
        {
            try
            {
                var (total, success, failed) = LogMonitor.UpdateAndGetTodayStatistics();

                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        lblTotalValue.Text = total.ToString();
                        lblSuccessValue.Text = success.ToString();
                        lblFailedValue.Text = failed.ToString();
                    }));
                }
                else
                {
                    lblTotalValue.Text = total.ToString();
                    lblSuccessValue.Text = success.ToString();
                    lblFailedValue.Text = failed.ToString();
                }
            }
            catch
            {
                // 忽略统计更新错误
            }
        }

        /// <summary>
        /// 显示解析规则
        /// </summary>
        private void DisplayParseRule()
        {
            if (_config == null)
                return;

            try
            {
                if (_config.FileNameParseRules?.Enabled == true)
                {
                    lblParseRule.Visible = true;
                    lblParseRuleValue.Visible = true;

                    string ruleText = "";

                    // 优先显示模板
                    if (!string.IsNullOrWhiteSpace(_config.FileNameParseRules.Template))
                    {
                        ruleText = $"模板: {_config.FileNameParseRules.Template}";
                    }
                    else if (_config.FileNameParseRules.PositionMapping?.Count > 0)
                    {
                        var mappings = _config.FileNameParseRules.PositionMapping
                            .Select(x => $"{x.Key}({x.Value})");
                        ruleText = $"位置映射: {string.Join(", ", mappings)}";
                    }
                    else if (_config.FileNameParseRules.RegexMapping?.Count > 0)
                    {
                        ruleText = $"正则表达式: {_config.FileNameParseRules.RegexMapping.Count}个规则";
                    }

                    lblParseRuleValue.Text = ruleText;
                }
                else
                {
                    lblParseRule.Visible = false;
                    lblParseRuleValue.Visible = false;
                }
            }
            catch
            {
                lblParseRule.Visible = false;
                lblParseRuleValue.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 停止定时器
                _statsTimer?.Stop();
                _statsTimer?.Dispose();

                // 停止上传服务
                if (_uploadService != null)
                {
                    _uploadService.Stop();
                    _uploadService.Dispose();
                }

                // 取消订阅日志事件
                LogManager.LogReceived -= OnLogReceived;

                LogManager.LogInfo("程序正常退出");
            }
            catch
            {
                // 忽略关闭时的错误
            }
        }

        private void menuConfig_Click(object sender, EventArgs e)
        {
            try
            {
                using var configForm = new ConfigForm();
                if (configForm.ShowDialog() == DialogResult.OK)
                {
                    // 配置已保存，提示用户重启
                    var result = MessageBox.Show(
                        "配置已保存成功！\n\n是否立即重启程序以使配置生效？",
                        "配置已保存",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        Application.Restart();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开配置页面失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            try
            {
                using var aboutForm = new AboutForm();
                aboutForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开关于页面失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuGenerateTestImages_Click(object sender, EventArgs e)
        {
            try
            {
                if (_config == null)
                {
                    MessageBox.Show(
                        "配置未加载，请先启动程序！",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using var testForm = new TestImageGeneratorForm(_config);
                testForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开测试图片生成器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体大小改变事件
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // 检查是否启用系统托盘功能
            if (_config?.SystemTray?.Enabled == true &&
                _config.SystemTray.MinimizeToTray &&
                this.WindowState == FormWindowState.Minimized)
            {
                // 最小化时隐藏窗口，显示托盘图标
                this.Hide();
                if (_notifyIcon != null)
                {
                    _notifyIcon.Visible = true;
                    _notifyIcon.ShowBalloonTip(2000, "文件上传系统", "程序已最小化到系统托盘", ToolTipIcon.Info);
                }
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // 检查是否启用系统托盘功能
                if (_config?.SystemTray?.Enabled == true)
                {
                    var closeAction = _config.SystemTray.CloseAction?.ToLower() ?? "ask";

                    if (closeAction == "minimize")
                    {
                        // 最小化到托盘
                        e.Cancel = true;
                        this.WindowState = FormWindowState.Minimized;
                    }
                    else if (closeAction == "ask")
                    {
                        // 询问用户
                        var result = MessageBox.Show(
                            "是否最小化到系统托盘?\n\n点击\"是\"最小化到托盘\n点击\"否\"退出程序",
                            "确认",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            // 最小化到托盘
                            e.Cancel = true;
                            this.WindowState = FormWindowState.Minimized;
                        }
                        else if (result == DialogResult.No)
                        {
                            // 退出程序
                            ExitApplication();
                        }
                        else
                        {
                            // 取消关闭
                            e.Cancel = true;
                        }
                        return;
                    }
                    // closeAction == "exit" 时直接退出
                }
            }

            base.OnFormClosing(e);
        }
    }
}
