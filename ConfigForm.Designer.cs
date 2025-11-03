namespace FileUpload
{
    partial class ConfigForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new Panel();
            this.lblTitle = new Label();
            this.tabControl = new TabControl();
            this.tabBasic = new TabPage();
            this.tabAdvanced = new TabPage();
            this.tabPerformance = new TabPage();
            this.panelButtons = new Panel();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            
            // 基本配置控件
            this.lblDeviceId = new Label();
            this.txtDeviceId = new TextBox();
            this.lblUploadUrl = new Label();
            this.txtUploadUrl = new TextBox();
            this.lblCommand = new Label();
            this.txtCommand = new TextBox();
            this.lblAllowedExtensions = new Label();
            this.txtAllowedExtensions = new TextBox();
            this.lblRequestTimeout = new Label();
            this.numRequestTimeout = new NumericUpDown();
            this.lblScanInterval = new Label();
            this.numScanInterval = new NumericUpDown();
            this.lblMaxRetryCount = new Label();
            this.numMaxRetryCount = new NumericUpDown();
            this.lblLanguage = new Label();
            this.cmbLanguage = new ComboBox();

            // 高级配置控件
            this.lstWatchFolders = new ListBox();
            this.btnAddFolder = new Button();
            this.btnRemoveFolder = new Button();
            this.chkScanSubfolders = new CheckBox();
            this.chkPreserveDirectoryStructure = new CheckBox();
            this.grpSystemTray = new GroupBox();
            this.chkSystemTrayEnabled = new CheckBox();
            this.chkMinimizeToTray = new CheckBox();
            this.lblCloseAction = new Label();
            this.cmbCloseAction = new ComboBox();

            // 性能优化控件
            this.grpThreadPool = new GroupBox();
            this.chkEnableThreadPool = new CheckBox();
            this.lblThreadPoolSize = new Label();
            this.numThreadPoolSize = new NumericUpDown();
            this.grpImageCompression = new GroupBox();
            this.chkEnableImageCompression = new CheckBox();
            this.lblCompressionThreshold = new Label();
            this.numCompressionThreshold = new NumericUpDown();
            this.lblCompressionQuality = new Label();
            this.numCompressionQuality = new NumericUpDown();
            
            this.panelTop.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabBasic.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.tabPerformance.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.grpSystemTray.SuspendLayout();
            this.grpThreadPool.SuspendLayout();
            this.grpImageCompression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRequestTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScanInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRetryCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreadPoolSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCompressionThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCompressionQuality)).BeginInit();
            this.SuspendLayout();
            
            // panelTop
            this.panelTop.BackColor = Color.FromArgb(41, 128, 185);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Location = new Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new Size(700, 60);
            this.panelTop.TabIndex = 0;
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(110, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = Resources.Strings.ConfigForm_Title;
            
            // tabControl
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Font = new Font("Microsoft YaHei UI", 9F);
            this.tabControl.Location = new Point(0, 60);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(700, 490);
            this.tabControl.TabIndex = 1;
            this.tabControl.Controls.Add(this.tabBasic);
            this.tabControl.Controls.Add(this.tabAdvanced);
            this.tabControl.Controls.Add(this.tabPerformance);
            
            // tabBasic
            this.tabBasic.BackColor = Color.White;
            this.tabBasic.Location = new Point(4, 26);
            this.tabBasic.Name = "tabBasic";
            this.tabBasic.Padding = new Padding(20);
            this.tabBasic.Size = new Size(692, 460);
            this.tabBasic.TabIndex = 0;
            this.tabBasic.Text = Resources.Strings.Tab_Basic;
            
            // 设备编号
            this.lblDeviceId.AutoSize = true;
            this.lblDeviceId.Location = new Point(20, 20);
            this.lblDeviceId.Text = Resources.Strings.Label_DeviceIdConfig;
            this.tabBasic.Controls.Add(this.lblDeviceId);

            this.txtDeviceId.Location = new Point(150, 17);
            this.txtDeviceId.Size = new Size(500, 23);
            this.tabBasic.Controls.Add(this.txtDeviceId);

            // 上传接口
            this.lblUploadUrl.AutoSize = true;
            this.lblUploadUrl.Location = new Point(20, 60);
            this.lblUploadUrl.Text = Resources.Strings.Label_UploadUrlConfig;
            this.tabBasic.Controls.Add(this.lblUploadUrl);

            this.txtUploadUrl.Location = new Point(150, 57);
            this.txtUploadUrl.Size = new Size(500, 23);
            this.tabBasic.Controls.Add(this.txtUploadUrl);

            // 上传命令
            this.lblCommand.AutoSize = true;
            this.lblCommand.Location = new Point(20, 100);
            this.lblCommand.Text = "上传命令:";
            this.tabBasic.Controls.Add(this.lblCommand);

            this.txtCommand.Location = new Point(150, 97);
            this.txtCommand.Size = new Size(500, 23);
            this.tabBasic.Controls.Add(this.txtCommand);

            // 允许的文件后缀
            this.lblAllowedExtensions.AutoSize = true;
            this.lblAllowedExtensions.Location = new Point(20, 140);
            this.lblAllowedExtensions.Text = Resources.Strings.Label_AllowedExtensions;
            this.tabBasic.Controls.Add(this.lblAllowedExtensions);

            this.txtAllowedExtensions.Location = new Point(150, 137);
            this.txtAllowedExtensions.Size = new Size(500, 23);
            this.tabBasic.Controls.Add(this.txtAllowedExtensions);

            // 请求超时时间
            this.lblRequestTimeout.AutoSize = true;
            this.lblRequestTimeout.Location = new Point(20, 180);
            this.lblRequestTimeout.Text = Resources.Strings.Label_RequestTimeout;
            this.tabBasic.Controls.Add(this.lblRequestTimeout);

            this.numRequestTimeout.Location = new Point(150, 177);
            this.numRequestTimeout.Size = new Size(100, 23);
            this.numRequestTimeout.Minimum = 10;
            this.numRequestTimeout.Maximum = 300;
            this.numRequestTimeout.Value = 30;
            this.tabBasic.Controls.Add(this.numRequestTimeout);

            // 扫描间隔
            this.lblScanInterval.AutoSize = true;
            this.lblScanInterval.Location = new Point(20, 220);
            this.lblScanInterval.Text = Resources.Strings.Label_ScanInterval;
            this.tabBasic.Controls.Add(this.lblScanInterval);

            this.numScanInterval.Location = new Point(150, 217);
            this.numScanInterval.Size = new Size(100, 23);
            this.numScanInterval.Minimum = 1;
            this.numScanInterval.Maximum = 60;
            this.numScanInterval.Value = 5;
            this.tabBasic.Controls.Add(this.numScanInterval);

            // 最大重试次数
            this.lblMaxRetryCount.AutoSize = true;
            this.lblMaxRetryCount.Location = new Point(20, 260);
            this.lblMaxRetryCount.Text = Resources.Strings.Label_MaxRetryCount;
            this.tabBasic.Controls.Add(this.lblMaxRetryCount);

            this.numMaxRetryCount.Location = new Point(150, 257);
            this.numMaxRetryCount.Size = new Size(100, 23);
            this.numMaxRetryCount.Minimum = 0;
            this.numMaxRetryCount.Maximum = 10;
            this.numMaxRetryCount.Value = 3;
            this.tabBasic.Controls.Add(this.numMaxRetryCount);

            // 界面语言
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new Point(20, 300);
            this.lblLanguage.Text = Resources.Strings.Label_Language;
            this.tabBasic.Controls.Add(this.lblLanguage);

            this.cmbLanguage.Location = new Point(150, 297);
            this.cmbLanguage.Size = new Size(200, 23);
            this.cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbLanguage.Items.AddRange(new object[] {
                Resources.Strings.Language_ChineseSimplified,
                Resources.Strings.Language_ChineseTraditional,
                Resources.Strings.Language_English,
                Resources.Strings.Language_Japanese,
                Resources.Strings.Language_Vietnamese
            });
            this.tabBasic.Controls.Add(this.cmbLanguage);

            // tabAdvanced
            this.tabAdvanced.BackColor = Color.White;
            this.tabAdvanced.Location = new Point(4, 26);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Padding = new Padding(20);
            this.tabAdvanced.Size = new Size(692, 460);
            this.tabAdvanced.TabIndex = 2;
            this.tabAdvanced.Text = Resources.Strings.Tab_Advanced;

            // 监控文件夹列表
            var lblWatchFolders = new Label();
            lblWatchFolders.AutoSize = true;
            lblWatchFolders.Location = new Point(20, 20);
            lblWatchFolders.Text = Resources.Strings.Label_WatchFolders;
            lblWatchFolders.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            this.tabAdvanced.Controls.Add(lblWatchFolders);

            this.lstWatchFolders.Location = new Point(20, 45);
            this.lstWatchFolders.Size = new Size(550, 120);
            this.lstWatchFolders.SelectionMode = SelectionMode.One;
            this.tabAdvanced.Controls.Add(this.lstWatchFolders);

            this.btnAddFolder.Location = new Point(580, 45);
            this.btnAddFolder.Size = new Size(90, 30);
            this.btnAddFolder.Text = Resources.Strings.Button_AddFolder;
            this.btnAddFolder.Click += btnAddFolder_Click;
            this.tabAdvanced.Controls.Add(this.btnAddFolder);

            this.btnRemoveFolder.Location = new Point(580, 85);
            this.btnRemoveFolder.Size = new Size(90, 30);
            this.btnRemoveFolder.Text = Resources.Strings.Button_RemoveFolder;
            this.btnRemoveFolder.Click += btnRemoveFolder_Click;
            this.tabAdvanced.Controls.Add(this.btnRemoveFolder);

            // 文件夹扫描选项
            this.chkScanSubfolders.AutoSize = true;
            this.chkScanSubfolders.Location = new Point(20, 180);
            this.chkScanSubfolders.Text = Resources.Strings.Label_ScanSubfolders;
            this.tabAdvanced.Controls.Add(this.chkScanSubfolders);

            this.chkPreserveDirectoryStructure.AutoSize = true;
            this.chkPreserveDirectoryStructure.Location = new Point(20, 210);
            this.chkPreserveDirectoryStructure.Text = Resources.Strings.Label_PreserveDirectoryStructure;
            this.tabAdvanced.Controls.Add(this.chkPreserveDirectoryStructure);

            // 系统托盘配置组
            this.grpSystemTray.Location = new Point(20, 250);
            this.grpSystemTray.Size = new Size(650, 150);
            this.grpSystemTray.Text = Resources.Strings.Group_SystemTray;
            this.tabAdvanced.Controls.Add(this.grpSystemTray);

            this.chkSystemTrayEnabled.AutoSize = true;
            this.chkSystemTrayEnabled.Location = new Point(20, 30);
            this.chkSystemTrayEnabled.Text = Resources.Strings.Label_EnableSystemTray;
            this.chkSystemTrayEnabled.CheckedChanged += chkSystemTrayEnabled_CheckedChanged;
            this.grpSystemTray.Controls.Add(this.chkSystemTrayEnabled);

            this.chkMinimizeToTray.AutoSize = true;
            this.chkMinimizeToTray.Location = new Point(20, 60);
            this.chkMinimizeToTray.Text = Resources.Strings.Label_MinimizeToTray;
            this.grpSystemTray.Controls.Add(this.chkMinimizeToTray);

            this.lblCloseAction.AutoSize = true;
            this.lblCloseAction.Location = new Point(20, 95);
            this.lblCloseAction.Text = Resources.Strings.Label_CloseAction;
            this.grpSystemTray.Controls.Add(this.lblCloseAction);

            this.cmbCloseAction.Location = new Point(150, 92);
            this.cmbCloseAction.Size = new Size(200, 23);
            this.cmbCloseAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCloseAction.Items.AddRange(new object[] { Resources.Strings.CloseAction_Ask, Resources.Strings.CloseAction_Minimize, Resources.Strings.CloseAction_Exit });
            this.cmbCloseAction.SelectedIndex = 0;
            this.grpSystemTray.Controls.Add(this.cmbCloseAction);

            // tabPerformance
            this.tabPerformance.BackColor = Color.White;
            this.tabPerformance.Location = new Point(4, 26);
            this.tabPerformance.Name = "tabPerformance";
            this.tabPerformance.Padding = new Padding(20);
            this.tabPerformance.Size = new Size(692, 460);
            this.tabPerformance.TabIndex = 1;
            this.tabPerformance.Text = Resources.Strings.Tab_Performance;

            // 线程池配置组
            this.grpThreadPool.Location = new Point(20, 20);
            this.grpThreadPool.Size = new Size(650, 120);
            this.grpThreadPool.Text = Resources.Strings.Group_ThreadPool;
            this.tabPerformance.Controls.Add(this.grpThreadPool);

            this.chkEnableThreadPool.AutoSize = true;
            this.chkEnableThreadPool.Location = new Point(20, 30);
            this.chkEnableThreadPool.Text = Resources.Strings.Label_EnableThreadPool;
            this.chkEnableThreadPool.CheckedChanged += chkEnableThreadPool_CheckedChanged;
            this.grpThreadPool.Controls.Add(this.chkEnableThreadPool);

            this.lblThreadPoolSize.AutoSize = true;
            this.lblThreadPoolSize.Location = new Point(20, 70);
            this.lblThreadPoolSize.Text = Resources.Strings.Label_ThreadPoolSize;
            this.grpThreadPool.Controls.Add(this.lblThreadPoolSize);

            this.numThreadPoolSize.Location = new Point(200, 67);
            this.numThreadPoolSize.Size = new Size(100, 23);
            this.numThreadPoolSize.Minimum = 1;
            this.numThreadPoolSize.Maximum = 10;
            this.numThreadPoolSize.Value = 3;
            this.grpThreadPool.Controls.Add(this.numThreadPoolSize);

            // 图片压缩配置组
            this.grpImageCompression.Location = new Point(20, 160);
            this.grpImageCompression.Size = new Size(650, 180);
            this.grpImageCompression.Text = Resources.Strings.Group_ImageCompression;
            this.tabPerformance.Controls.Add(this.grpImageCompression);

            this.chkEnableImageCompression.AutoSize = true;
            this.chkEnableImageCompression.Location = new Point(20, 30);
            this.chkEnableImageCompression.Text = Resources.Strings.Label_EnableImageCompression;
            this.chkEnableImageCompression.CheckedChanged += chkEnableImageCompression_CheckedChanged;
            this.grpImageCompression.Controls.Add(this.chkEnableImageCompression);

            this.lblCompressionThreshold.AutoSize = true;
            this.lblCompressionThreshold.Location = new Point(20, 70);
            this.lblCompressionThreshold.Text = Resources.Strings.Label_CompressionThreshold;
            this.grpImageCompression.Controls.Add(this.lblCompressionThreshold);

            this.numCompressionThreshold.Location = new Point(250, 67);
            this.numCompressionThreshold.Size = new Size(100, 23);
            this.numCompressionThreshold.Minimum = 100;
            this.numCompressionThreshold.Maximum = 10000;
            this.numCompressionThreshold.Value = 500;
            this.grpImageCompression.Controls.Add(this.numCompressionThreshold);

            this.lblCompressionQuality.AutoSize = true;
            this.lblCompressionQuality.Location = new Point(20, 110);
            this.lblCompressionQuality.Text = Resources.Strings.Label_CompressionQuality;
            this.grpImageCompression.Controls.Add(this.lblCompressionQuality);

            this.numCompressionQuality.Location = new Point(250, 107);
            this.numCompressionQuality.Size = new Size(100, 23);
            this.numCompressionQuality.Minimum = 1;
            this.numCompressionQuality.Maximum = 100;
            this.numCompressionQuality.Value = 75;
            this.grpImageCompression.Controls.Add(this.numCompressionQuality);
            
            // panelButtons
            this.panelButtons.BackColor = Color.FromArgb(236, 240, 241);
            this.panelButtons.Dock = DockStyle.Bottom;
            this.panelButtons.Location = new Point(0, 550);
            this.panelButtons.Size = new Size(700, 60);
            this.panelButtons.Controls.Add(this.btnSave);
            this.panelButtons.Controls.Add(this.btnCancel);
            
            // btnSave
            this.btnSave.BackColor = Color.FromArgb(39, 174, 96);
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(470, 10);
            this.btnSave.Size = new Size(100, 40);
            this.btnSave.Text = Resources.Strings.Button_Save;
            this.btnSave.Click += btnSave_Click;

            // btnCancel
            this.btnCancel.BackColor = Color.FromArgb(189, 195, 199);
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(580, 10);
            this.btnCancel.Size = new Size(100, 40);
            this.btnCancel.Text = Resources.Strings.Button_Cancel;
            this.btnCancel.Click += btnCancel_Click;
            
            // ConfigForm
            this.AutoScaleDimensions = new SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(700, 610);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelButtons);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = Resources.Strings.ConfigForm_Title;
            this.Load += ConfigForm_Load;
            
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabBasic.ResumeLayout(false);
            this.tabBasic.PerformLayout();
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            this.grpSystemTray.ResumeLayout(false);
            this.grpSystemTray.PerformLayout();
            this.tabPerformance.ResumeLayout(false);
            this.grpThreadPool.ResumeLayout(false);
            this.grpThreadPool.PerformLayout();
            this.grpImageCompression.ResumeLayout(false);
            this.grpImageCompression.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRequestTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScanInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxRetryCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreadPoolSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCompressionThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCompressionQuality)).EndInit();
            this.ResumeLayout(false);
        }

        private Panel panelTop;
        private Label lblTitle;
        private TabControl tabControl;
        private TabPage tabBasic;
        private TabPage tabAdvanced;
        private TabPage tabPerformance;
        private Panel panelButtons;
        private Button btnSave;
        private Button btnCancel;

        private Label lblDeviceId;
        private TextBox txtDeviceId;
        private Label lblUploadUrl;
        private TextBox txtUploadUrl;
        private Label lblCommand;
        private TextBox txtCommand;
        private Label lblAllowedExtensions;
        private TextBox txtAllowedExtensions;
        private Label lblRequestTimeout;
        private NumericUpDown numRequestTimeout;
        private Label lblScanInterval;
        private NumericUpDown numScanInterval;
        private Label lblMaxRetryCount;
        private NumericUpDown numMaxRetryCount;
        private Label lblLanguage;
        private ComboBox cmbLanguage;

        private ListBox lstWatchFolders;
        private Button btnAddFolder;
        private Button btnRemoveFolder;
        private CheckBox chkScanSubfolders;
        private CheckBox chkPreserveDirectoryStructure;
        private GroupBox grpSystemTray;
        private CheckBox chkSystemTrayEnabled;
        private CheckBox chkMinimizeToTray;
        private Label lblCloseAction;
        private ComboBox cmbCloseAction;

        private GroupBox grpThreadPool;
        private CheckBox chkEnableThreadPool;
        private Label lblThreadPoolSize;
        private NumericUpDown numThreadPoolSize;
        private GroupBox grpImageCompression;
        private CheckBox chkEnableImageCompression;
        private Label lblCompressionThreshold;
        private NumericUpDown numCompressionThreshold;
        private Label lblCompressionQuality;
        private NumericUpDown numCompressionQuality;
    }
}

