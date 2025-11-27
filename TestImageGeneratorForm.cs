using FileUpload.Models;
using FileUpload.Services;
using System.Threading;

namespace FileUpload
{
    public partial class TestImageGeneratorForm : Form
    {
        private readonly AppConfig _config;
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isGenerating = false;

        public TestImageGeneratorForm(AppConfig config)
        {
            _config = config;
            InitializeComponent();
            LoadWatchFolderInfo();
        }

        /// <summary>
        /// 加载监控目录信息
        /// </summary>
        private void LoadWatchFolderInfo()
        {
            var watchFolders = _config.GetWatchFolders();
            if (watchFolders.Count > 0)
            {
                if (watchFolders.Count == 1)
                {
                    lblWatchFolder.Text = $"监控目录: {watchFolders[0]}";
                }
                else
                {
                    lblWatchFolder.Text = $"监控目录: {watchFolders.Count} 个文件夹";
                    // 创建ToolTip来显示多行目录信息
                    var toolTip = new ToolTip();
                    toolTip.SetToolTip(lblWatchFolder, string.Join(Environment.NewLine, watchFolders));
                }
            }
            else
            {
                lblWatchFolder.Text = "监控目录: 未配置";
                lblWatchFolder.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// 生成测试图片
        /// </summary>
        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (!ValidateInput(out int count, out int startNumber, out int width, out int height, out int maxConcurrency))
                {
                    return;
                }

                // 获取监控目录
                var watchFolders = _config.GetWatchFolders();
                if (watchFolders.Count == 0)
                {
                    MessageBox.Show(
                        "请先配置监控目录！",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // 使用第一个监控目录作为输出目录
                string outputFolder = watchFolders[0];

                // 设置UI状态
                SetGeneratingUI(true);

                // 创建取消令牌
                _cancellationTokenSource = new CancellationTokenSource();

                // 创建进度报告器
                var progress = new Progress<int>(percent =>
                {
                    progressBar1.Value = percent;
                    lblProgress.Text = $"生成进度: {percent}%";
                });

                // 在后台线程生成图片（支持取消和进度）
                await Task.Run(() =>
                {
                    try
                    {
                        TestImageGeneratorService.GenerateTestImages(
                            outputFolder,
                            count,
                            startNumber,
                            width,
                            height,
                            format: null,
                            maxDegreeOfParallelism: maxConcurrency,
                            progress: progress,
                            cancellationToken: _cancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // 生成被取消，不显示错误
                    }
                }, _cancellationTokenSource.Token);

                // 如果没有被取消，显示成功消息
                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    MessageBox.Show(
                        $"成功生成 {count} 张测试图片到:\r\n{outputFolder}\r\n\r\n" +
                        $"并发数: {maxConcurrency}",
                        "生成完成",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // 关闭窗体
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    // 被取消，显示取消消息
                    MessageBox.Show(
                        "生成已取消",
                        "提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                // 生成被取消，不显示错误
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"生成测试图片时发生错误:\r\n{ex.Message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                // 恢复UI状态
                SetGeneratingUI(false);
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// 验证用户输入
        /// </summary>
        private bool ValidateInput(out int count, out int startNumber, out int width, out int height, out int maxConcurrency)
        {
            count = 0;
            startNumber = 0;
            width = 0;
            height = 0;
            maxConcurrency = 0;

            // 验证生成数量
            if (!int.TryParse(txtCount.Text, out count) || count <= 0 || count > 1000000)
            {
                MessageBox.Show(
                    "请输入有效的生成数量（1-1,000,000）！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtCount.Focus();
                return false;
            }

            // 验证起始序号
            if (!int.TryParse(txtStartNumber.Text, out startNumber) || startNumber < 1)
            {
                MessageBox.Show(
                    "请输入有效的起始序号（>=1）！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtStartNumber.Focus();
                return false;
            }

            // 验证图片宽度
            if (!int.TryParse(txtWidth.Text, out width) || width < 100 || width > 4000)
            {
                MessageBox.Show(
                    "请输入有效的图片宽度（100-4000）！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtWidth.Focus();
                return false;
            }

            // 验证图片高度
            if (!int.TryParse(txtHeight.Text, out height) || height < 100 || height > 4000)
            {
                MessageBox.Show(
                    "请输入有效的图片高度（100-4000）！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtHeight.Focus();
                return false;
            }

            // 验证并发数
            if (!int.TryParse(txtMaxConcurrency.Text, out maxConcurrency) || maxConcurrency < 1 || maxConcurrency > 100)
            {
                MessageBox.Show(
                    "请输入有效的并发数（1-100）！",
                    "输入错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtMaxConcurrency.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 设置生成中的UI状态
        /// </summary>
        private void SetGeneratingUI(bool isGenerating)
        {
            _isGenerating = isGenerating;

            btnGenerate.Enabled = !isGenerating;
            btnCancelGeneration.Enabled = isGenerating;
            btnCancel.Enabled = !isGenerating;

            // 显示/隐藏进度条
            progressBar1.Visible = isGenerating;
            lblProgress.Visible = isGenerating;

            // 重置进度
            if (isGenerating)
            {
                progressBar1.Value = 0;
                lblProgress.Text = "生成进度: 0%";
            }

            // 禁用输入框
            txtCount.ReadOnly = isGenerating;
            txtStartNumber.ReadOnly = isGenerating;
            txtWidth.ReadOnly = isGenerating;
            txtHeight.ReadOnly = isGenerating;
            txtMaxConcurrency.ReadOnly = isGenerating;

            // 更新按钮文字
            btnGenerate.Text = isGenerating ? "生成中..." : "生成";
        }

        /// <summary>
        /// 取消按钮点击事件（取消窗体）
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_isGenerating)
            {
                MessageBox.Show(
                    "正在生成中，请点击\"停止\"按钮取消生成！",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// 取消生成按钮点击事件（取消生成过程）
        /// </summary>
        private void btnCancelGeneration_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            btnCancelGeneration.Enabled = false;
            btnCancelGeneration.Text = "停止中...";
        }
    }
}
