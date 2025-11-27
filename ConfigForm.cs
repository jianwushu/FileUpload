using FileUpload.Models;
using FileUpload.Services;
using FileUpload.utils;
using Newtonsoft.Json;

namespace FileUpload
{
    /// <summary>
    /// 配置页面
    /// </summary>
    public partial class ConfigForm : Form
    {
        private AppConfig _config = null!;

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

                // 服务注册配置
                txtServiceCenterApi.Text = _config.ServiceRegistration.ServiceCenterApi;
                chkAutoRegister.Checked = _config.ServiceRegistration.AutoRegister;
                chkEnableHeartbeat.Checked = _config.ServiceRegistration.EnableHeartbeat;
                numHeartbeatInterval.Value = _config.ServiceRegistration.HeartbeatInterval;
                lblServiceNameValue.Text = _config.DeviceId;
                lblStatusValue.Text = _config.ServiceRegistration.RegistrationStatus ?? "未注册";

                // 显示最后心跳时间
                if (_config.ServiceRegistration.LastHeartbeatTime.HasValue)
                {
                    lblLastHeartbeatValue.Text = _config.ServiceRegistration.LastHeartbeatTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    lblLastHeartbeatValue.Text = "-";
                }

                UpdateRegistrationStatusColor();

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

            // 心跳配置控件状态
            lblHeartbeatInterval.Enabled = chkEnableHeartbeat.Checked;
            numHeartbeatInterval.Enabled = chkEnableHeartbeat.Checked;
            lblLastHeartbeat.Enabled = chkEnableHeartbeat.Checked;
            lblLastHeartbeatValue.Enabled = chkEnableHeartbeat.Checked;
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

        private void chkEnableHeartbeat_CheckedChanged(object sender, EventArgs e)
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

                // 更新服务注册配置
                _config.ServiceRegistration.ServiceCenterApi = txtServiceCenterApi.Text.Trim();
                _config.ServiceRegistration.AutoRegister = chkAutoRegister.Checked;
                _config.ServiceRegistration.EnableHeartbeat = chkEnableHeartbeat.Checked;
                _config.ServiceRegistration.HeartbeatInterval = (int)numHeartbeatInterval.Value;

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

        #region 服务注册相关方法

        /// <summary>
        /// 更新注册状态颜色
        /// </summary>
        private void UpdateRegistrationStatusColor()
        {
            if (lblStatusValue.Text.Contains("成功") || lblStatusValue.Text.Contains("已注册"))
            {
                lblStatusValue.ForeColor = Color.FromArgb(39, 174, 96); // 绿色
            }
            else if (lblStatusValue.Text.Contains("失败") || lblStatusValue.Text.Contains("错误"))
            {
                lblStatusValue.ForeColor = Color.FromArgb(192, 57, 43); // 红色
            }
            else
            {
                lblStatusValue.ForeColor = Color.FromArgb(243, 156, 18); // 橙色
            }
        }

        /// <summary>
        /// 服务注册按钮点击事件
        /// </summary>
        private async void BtnServiceRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证服务中心API地址
                if (string.IsNullOrWhiteSpace(txtServiceCenterApi.Text))
                {
                    MessageBox.Show("请先配置服务中心API地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 禁用按钮，显示进度
                btnServiceRegister.Enabled = false;
                progressService.Style = ProgressBarStyle.Marquee;
                lblOperationStatus.Text = "操作状态: 正在注册服务...";

                // 发送注册请求
                var result = await ApiHelper.RegisterServiceDiscoveryAsync(
                    txtServiceCenterApi.Text,
                    txtDeviceId.Text.Trim(),
                    "V1.0.0",
                    (_config.ServiceRegistration.HeartbeatInterval*2).ToString());

                if (result.Success)
                {
                    lblStatusValue.Text = "注册成功";
                    _config.ServiceRegistration.RegistrationStatus = "注册成功";
                    _config.ServiceRegistration.LastRegistrationTime = DateTime.Now;
                    UpdateRegistrationStatusColor();

                    lblOperationStatus.Text = "操作状态: 服务注册成功";
                    MessageBox.Show("服务注册成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatusValue.Text = "注册失败";
                    UpdateRegistrationStatusColor();
                    lblOperationStatus.Text = $"操作状态: 注册失败 - {result.StatusCode}";
                    MessageBox.Show($"服务注册失败: {result.StatusCode}\n{result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatusValue.Text = "注册失败";
                UpdateRegistrationStatusColor();
                lblOperationStatus.Text = $"操作状态: 注册异常 - {ex.Message}";
                MessageBox.Show($"服务注册异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnServiceRegister.Enabled = true;
                progressService.Style = ProgressBarStyle.Continuous;
                progressService.Value = 0;
            }
        }

        /// <summary>
        /// 配置上传按钮点击事件
        /// </summary>
        private async void BtnUploadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证服务中心API地址
                if (string.IsNullOrWhiteSpace(txtServiceCenterApi.Text))
                {
                    MessageBox.Show("请先配置服务中心API地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 禁用按钮，显示进度
                btnUploadConfig.Enabled = false;
                progressService.Style = ProgressBarStyle.Marquee;
                lblOperationStatus.Text = "操作状态: 正在上传配置...";

                var config = ConfigManager.LoadConfig();

                // 发送上传请求
                var result = await ApiHelper.UploadConfigAsync(
                    txtServiceCenterApi.Text,
                    txtDeviceId.Text.Trim(),
                    config,
                    "prod");

                if (result.Success)
                {
                    lblOperationStatus.Text = "操作状态: 配置上传成功";
                    MessageBox.Show("配置上传成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblOperationStatus.Text = $"操作状态: 上传失败 - {result.StatusCode}";
                    MessageBox.Show($"配置上传失败: {result.StatusCode}\n{result.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblOperationStatus.Text = $"操作状态: 上传异常 - {ex.Message}";
                MessageBox.Show($"配置上传异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUploadConfig.Enabled = true;
                progressService.Style = ProgressBarStyle.Continuous;
                progressService.Value = 0;
            }
        }

        /// <summary>
        /// 配置下载按钮点击事件
        /// </summary>
        private async void BtnDownloadConfig_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证服务中心API地址
                if (string.IsNullOrWhiteSpace(txtServiceCenterApi.Text))
                {
                    MessageBox.Show("请先配置服务中心API地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 确认是否下载配置（会覆盖当前配置）
                var result = MessageBox.Show(
                    "下载配置将覆盖当前的配置文件，是否继续？\n\n建议先备份当前配置。",
                    "确认下载",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                // 禁用按钮，显示进度
                btnDownloadConfig.Enabled = false;
                progressService.Style = ProgressBarStyle.Marquee;
                lblOperationStatus.Text = "操作状态: 正在下载配置...";

                // 发送下载请求
                var deviceId = txtDeviceId.Text.Trim();
                var apiResult = await ApiHelper.DownloadConfigAsync(
                    txtServiceCenterApi.Text,
                    deviceId);

                if (apiResult.Success)
                {

                    if (apiResult.Data != null)
                    {

                        if(apiResult.Data.Code == 200 && apiResult.Data.Data != null)
                        {
                            // 备份当前配置
                            var configPath = ConfigManager.ConfigFilePath;
                            var backupPath = $"{configPath}.backup.{DateTime.Now:yyyyMMddHHmmss}";

                            if (File.Exists(configPath))
                            {
                                File.Copy(configPath, backupPath, true);
                            }

                            var config = JsonConvert.DeserializeObject<AppConfig>(apiResult.Data.Data.ToString());
                            ConfigManager.SaveConfig(config);

                            lblOperationStatus.Text = "操作状态: 配置下载成功";
                            MessageBox.Show(
                                $"配置下载成功！\n\n原配置已备份至:\n{backupPath}\n\n请重启程序以使新配置生效。",
                                "成功",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            // 重新加载配置
                            LoadConfiguration();
                        }
                        else
                        {
                            lblOperationStatus.Text = "操作状态: 未找到配置";
                            MessageBox.Show(apiResult.Data.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        lblOperationStatus.Text = "操作状态: 响应格式错误";
                        MessageBox.Show("服务器响应格式错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (apiResult.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    lblOperationStatus.Text = "操作状态: 未找到配置";
                    MessageBox.Show("请求异常", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblOperationStatus.Text = $"操作状态: 下载失败 - {apiResult.StatusCode}";
                    MessageBox.Show($"配置下载失败: {apiResult.StatusCode}\n{apiResult.ErrorMessage}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblOperationStatus.Text = $"操作状态: 下载异常 - {ex.Message}";
                MessageBox.Show($"配置下载异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDownloadConfig.Enabled = true;
                progressService.Style = ProgressBarStyle.Continuous;
                progressService.Value = 0;
            }
        }

        #endregion
    }
}

