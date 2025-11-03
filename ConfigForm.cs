using FileUpload.Models;
using FileUpload.Services;

namespace FileUpload
{
    /// <summary>
    /// 配置页面
    /// </summary>
    public partial class ConfigForm : Form
    {
        private AppConfig _config;

        public ConfigForm()
        {
            InitializeComponent();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            try
            {
                _config = ConfigManager.LoadConfig();
                
                // 基本配置
                txtDeviceId.Text = _config.DeviceId;
                txtUploadUrl.Text = _config.UploadUrl;
                txtCommand.Text = _config.Command;
                txtAllowedExtensions.Text = _config.AllowedExtensions;
                numRequestTimeout.Value = _config.RequestTimeout;
                numScanInterval.Value = _config.ScanInterval;
                numMaxRetryCount.Value = _config.MaxRetryCount;

                // 高级配置 - 监控文件夹
                lstWatchFolders.Items.Clear();
                if (_config.WatchFolders != null && _config.WatchFolders.Count > 0)
                {
                    foreach (var folder in _config.WatchFolders)
                    {
                        lstWatchFolders.Items.Add(folder);
                    }
                }

                chkScanSubfolders.Checked = _config.ScanSubfolders;
                chkPreserveDirectoryStructure.Checked = _config.PreserveDirectoryStructure;

                // 高级配置 - 系统托盘
                chkSystemTrayEnabled.Checked = _config.SystemTray.Enabled;
                chkMinimizeToTray.Checked = _config.SystemTray.MinimizeToTray;

                // 关闭行为映射
                var closeAction = _config.SystemTray.CloseAction?.ToLower() ?? "ask";
                cmbCloseAction.SelectedIndex = closeAction switch
                {
                    "ask" => 0,
                    "minimize" => 1,
                    "exit" => 2,
                    _ => 0
                };

                // 性能优化配置
                chkEnableThreadPool.Checked = _config.EnableThreadPool;
                numThreadPoolSize.Value = _config.ThreadPoolSize;

                // 图片压缩配置
                chkEnableImageCompression.Checked = _config.EnableImageCompression;
                numCompressionThreshold.Value = _config.CompressionThresholdKB;
                numCompressionQuality.Value = _config.CompressionQuality;

                // 语言配置
                LoadLanguageSelection();

                UpdateControlStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLanguageSelection()
        {
            // 根据配置的语言代码选择对应的下拉项
            var language = _config.Language ?? "zh-CN";
            cmbLanguage.SelectedIndex = language switch
            {
                "zh-CN" => 0,      // 中文简体
                "zh-Hant" => 1,    // 繁体中文
                "en" => 2,         // English
                "ja" => 3,         // 日本語
                "vi" => 4,         // Tiếng Việt
                _ => 0             // 默认中文简体
            };
        }

        private string GetSelectedLanguageCode()
        {
            // 根据下拉框选择的索引返回语言代码
            return cmbLanguage.SelectedIndex switch
            {
                0 => "zh-CN",      // 中文简体
                1 => "zh-Hant",    // 繁体中文
                2 => "en",         // English
                3 => "ja",         // 日本語
                4 => "vi",         // Tiếng Việt
                _ => "zh-CN"       // 默认中文简体
            };
        }

        private void UpdateControlStates()
        {
            numThreadPoolSize.Enabled = chkEnableThreadPool.Checked;
            numCompressionThreshold.Enabled = chkEnableImageCompression.Checked;
            numCompressionQuality.Enabled = chkEnableImageCompression.Checked;

            chkMinimizeToTray.Enabled = chkSystemTrayEnabled.Checked;
            lblCloseAction.Enabled = chkSystemTrayEnabled.Checked;
            cmbCloseAction.Enabled = chkSystemTrayEnabled.Checked;
        }

        private void chkEnableThreadPool_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        private void chkEnableImageCompression_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        private void chkSystemTrayEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "选择要添加的监控文件夹";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var folder = dialog.SelectedPath;

                // 检查是否已存在
                if (lstWatchFolders.Items.Contains(folder))
                {
                    MessageBox.Show("该文件夹已在监控列表中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                lstWatchFolders.Items.Add(folder);
            }
        }

        private void btnRemoveFolder_Click(object sender, EventArgs e)
        {
            if (lstWatchFolders.SelectedIndex >= 0)
            {
                lstWatchFolders.Items.RemoveAt(lstWatchFolders.SelectedIndex);
            }
            else
            {
                MessageBox.Show("请先选择要移除的文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(txtDeviceId.Text))
                {
                    MessageBox.Show("设备编号不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUploadUrl.Text))
                {
                    MessageBox.Show("上传接口URL不能为空", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 验证监控文件夹（至少要有一个）
                if (lstWatchFolders.Items.Count == 0)
                {
                    MessageBox.Show("至少需要配置一个监控文件夹", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 更新基本配置
                _config.DeviceId = txtDeviceId.Text.Trim();
                _config.UploadUrl = txtUploadUrl.Text.Trim();
                _config.Command = txtCommand.Text.Trim();
                _config.AllowedExtensions = txtAllowedExtensions.Text.Trim();
                _config.RequestTimeout = (int)numRequestTimeout.Value;
                _config.ScanInterval = (int)numScanInterval.Value;
                _config.MaxRetryCount = (int)numMaxRetryCount.Value;

                // 更新高级配置 - 监控文件夹
                _config.WatchFolders = new List<string>();
                foreach (var item in lstWatchFolders.Items)
                {
                    _config.WatchFolders.Add(item.ToString()!);
                }
                _config.WatchFolder = null; // 清空旧配置

                _config.ScanSubfolders = chkScanSubfolders.Checked;
                _config.PreserveDirectoryStructure = chkPreserveDirectoryStructure.Checked;

                // 更新高级配置 - 系统托盘
                _config.SystemTray.Enabled = chkSystemTrayEnabled.Checked;
                _config.SystemTray.MinimizeToTray = chkMinimizeToTray.Checked;
                _config.SystemTray.CloseAction = cmbCloseAction.SelectedIndex switch
                {
                    0 => "ask",
                    1 => "minimize",
                    2 => "exit",
                    _ => "ask"
                };

                // 更新性能配置
                _config.EnableThreadPool = chkEnableThreadPool.Checked;
                _config.ThreadPoolSize = (int)numThreadPoolSize.Value;

                _config.EnableImageCompression = chkEnableImageCompression.Checked;
                _config.CompressionThresholdKB = (int)numCompressionThreshold.Value;
                _config.CompressionQuality = (int)numCompressionQuality.Value;

                // 更新语言配置
                _config.Language = GetSelectedLanguageCode();

                // 保存配置
                ConfigManager.SaveConfig(_config);
                
                MessageBox.Show("配置保存成功！\n\n请重启程序以使配置生效。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

