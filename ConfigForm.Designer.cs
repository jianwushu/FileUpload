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
            panelTop = new Panel();
            lblTitle = new Label();
            tabControl = new TabControl();
            tabBasic = new TabPage();
            lblDeviceId = new Label();
            txtDeviceId = new TextBox();
            lblUploadUrl = new Label();
            txtUploadUrl = new TextBox();
            lblCommand = new Label();
            txtCommand = new TextBox();
            lblAllowedExtensions = new Label();
            txtAllowedExtensions = new TextBox();
            lblRequestTimeout = new Label();
            numRequestTimeout = new NumericUpDown();
            lblScanInterval = new Label();
            numScanInterval = new NumericUpDown();
            lblMaxRetryCount = new Label();
            numMaxRetryCount = new NumericUpDown();
            lblLanguage = new Label();
            cmbLanguage = new ComboBox();
            tabAdvanced = new TabPage();
            lblWatchFolders = new Label();
            lstWatchFolders = new ListBox();
            btnAddFolder = new Button();
            btnRemoveFolder = new Button();
            chkScanSubfolders = new CheckBox();
            chkPreserveDirectoryStructure = new CheckBox();
            grpSystemTray = new GroupBox();
            chkSystemTrayEnabled = new CheckBox();
            chkMinimizeToTray = new CheckBox();
            lblCloseAction = new Label();
            cmbCloseAction = new ComboBox();
            tabService = new TabPage();
            lblServiceCenterApi = new Label();
            txtServiceCenterApi = new TextBox();
            lblServiceName = new Label();
            lblServiceNameValue = new Label();
            chkAutoRegister = new CheckBox();
            chkEnableHeartbeat = new CheckBox();
            lblHeartbeatInterval = new Label();
            numHeartbeatInterval = new NumericUpDown();
            lblLastHeartbeat = new Label();
            lblLastHeartbeatValue = new Label();
            lblRegistrationStatus = new Label();
            lblStatusValue = new Label();
            btnServiceRegister = new Button();
            btnUploadConfig = new Button();
            btnDownloadConfig = new Button();
            lblOperationStatus = new Label();
            progressService = new ProgressBar();
            tabPerformance = new TabPage();
            grpThreadPool = new GroupBox();
            chkEnableThreadPool = new CheckBox();
            lblThreadPoolSize = new Label();
            numThreadPoolSize = new NumericUpDown();
            grpImageCompression = new GroupBox();
            chkEnableImageCompression = new CheckBox();
            lblCompressionThreshold = new Label();
            numCompressionThreshold = new NumericUpDown();
            lblCompressionQuality = new Label();
            numCompressionQuality = new NumericUpDown();
            panelButtons = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            panelTop.SuspendLayout();
            tabControl.SuspendLayout();
            tabBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRequestTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numScanInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxRetryCount).BeginInit();
            tabAdvanced.SuspendLayout();
            grpSystemTray.SuspendLayout();
            tabService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numHeartbeatInterval).BeginInit();
            tabPerformance.SuspendLayout();
            grpThreadPool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numThreadPoolSize).BeginInit();
            grpImageCompression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCompressionThreshold).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCompressionQuality).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(41, 128, 185);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(700, 60);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(110, 31);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "系统配置";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabBasic);
            tabControl.Controls.Add(tabAdvanced);
            tabControl.Controls.Add(tabService);
            tabControl.Controls.Add(tabPerformance);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Microsoft YaHei UI", 9F);
            tabControl.Location = new Point(0, 60);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(700, 490);
            tabControl.TabIndex = 1;
            // 
            // tabBasic
            // 
            tabBasic.BackColor = Color.White;
            tabBasic.Controls.Add(lblDeviceId);
            tabBasic.Controls.Add(txtDeviceId);
            tabBasic.Controls.Add(lblUploadUrl);
            tabBasic.Controls.Add(txtUploadUrl);
            tabBasic.Controls.Add(lblCommand);
            tabBasic.Controls.Add(txtCommand);
            tabBasic.Controls.Add(lblAllowedExtensions);
            tabBasic.Controls.Add(txtAllowedExtensions);
            tabBasic.Controls.Add(lblRequestTimeout);
            tabBasic.Controls.Add(numRequestTimeout);
            tabBasic.Controls.Add(lblScanInterval);
            tabBasic.Controls.Add(numScanInterval);
            tabBasic.Controls.Add(lblMaxRetryCount);
            tabBasic.Controls.Add(numMaxRetryCount);
            tabBasic.Controls.Add(lblLanguage);
            tabBasic.Controls.Add(cmbLanguage);
            tabBasic.Location = new Point(4, 26);
            tabBasic.Name = "tabBasic";
            tabBasic.Padding = new Padding(20);
            tabBasic.Size = new Size(692, 460);
            tabBasic.TabIndex = 0;
            tabBasic.Text = Resources.Strings.Tab_Basic;
            // 
            // lblDeviceId
            // 
            lblDeviceId.AutoSize = true;
            lblDeviceId.Location = new Point(20, 20);
            lblDeviceId.Name = "lblDeviceId";
            lblDeviceId.Size = new Size(59, 17);
            lblDeviceId.TabIndex = 0;
            lblDeviceId.Text = "设备编号:";
            // 
            // txtDeviceId
            // 
            txtDeviceId.Location = new Point(150, 17);
            txtDeviceId.Name = "txtDeviceId";
            txtDeviceId.Size = new Size(500, 23);
            txtDeviceId.TabIndex = 1;
            // 
            // lblUploadUrl
            // 
            lblUploadUrl.AutoSize = true;
            lblUploadUrl.Location = new Point(20, 60);
            lblUploadUrl.Name = "lblUploadUrl";
            lblUploadUrl.Size = new Size(82, 17);
            lblUploadUrl.TabIndex = 2;
            lblUploadUrl.Text = "上传接口URL:";
            // 
            // txtUploadUrl
            // 
            txtUploadUrl.Location = new Point(150, 57);
            txtUploadUrl.Name = "txtUploadUrl";
            txtUploadUrl.Size = new Size(500, 23);
            txtUploadUrl.TabIndex = 3;
            // 
            // lblCommand
            // 
            lblCommand.AutoSize = true;
            lblCommand.Location = new Point(20, 100);
            lblCommand.Name = "lblCommand";
            lblCommand.Size = new Size(59, 17);
            lblCommand.TabIndex = 4;
            lblCommand.Text = "上传命令:";
            // 
            // txtCommand
            // 
            txtCommand.Location = new Point(150, 97);
            txtCommand.Name = "txtCommand";
            txtCommand.Size = new Size(500, 23);
            txtCommand.TabIndex = 5;
            // 
            // lblAllowedExtensions
            // 
            lblAllowedExtensions.AutoSize = true;
            lblAllowedExtensions.Location = new Point(20, 140);
            lblAllowedExtensions.Name = "lblAllowedExtensions";
            lblAllowedExtensions.Size = new Size(95, 17);
            lblAllowedExtensions.TabIndex = 6;
            lblAllowedExtensions.Text = "允许的文件后缀:";
            // 
            // txtAllowedExtensions
            // 
            txtAllowedExtensions.Location = new Point(150, 137);
            txtAllowedExtensions.Name = "txtAllowedExtensions";
            txtAllowedExtensions.Size = new Size(500, 23);
            txtAllowedExtensions.TabIndex = 7;
            // 
            // lblRequestTimeout
            // 
            lblRequestTimeout.AutoSize = true;
            lblRequestTimeout.Location = new Point(20, 180);
            lblRequestTimeout.Name = "lblRequestTimeout";
            lblRequestTimeout.Size = new Size(103, 17);
            lblRequestTimeout.TabIndex = 8;
            lblRequestTimeout.Text = "请求超时时间(秒):";
            // 
            // numRequestTimeout
            // 
            numRequestTimeout.Location = new Point(150, 177);
            numRequestTimeout.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            numRequestTimeout.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numRequestTimeout.Name = "numRequestTimeout";
            numRequestTimeout.Size = new Size(100, 23);
            numRequestTimeout.TabIndex = 9;
            numRequestTimeout.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // lblScanInterval
            // 
            lblScanInterval.AutoSize = true;
            lblScanInterval.Location = new Point(20, 220);
            lblScanInterval.Name = "lblScanInterval";
            lblScanInterval.Size = new Size(103, 17);
            lblScanInterval.TabIndex = 10;
            lblScanInterval.Text = "文件扫描间隔(秒):";
            // 
            // numScanInterval
            // 
            numScanInterval.Location = new Point(150, 217);
            numScanInterval.Maximum = new decimal(new int[] { 43200, 0, 0, 0 });
            numScanInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numScanInterval.Name = "numScanInterval";
            numScanInterval.Size = new Size(100, 23);
            numScanInterval.TabIndex = 11;
            numScanInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // lblMaxRetryCount
            // 
            lblMaxRetryCount.AutoSize = true;
            lblMaxRetryCount.Location = new Point(20, 260);
            lblMaxRetryCount.Name = "lblMaxRetryCount";
            lblMaxRetryCount.Size = new Size(83, 17);
            lblMaxRetryCount.TabIndex = 12;
            lblMaxRetryCount.Text = "最大重试次数:";
            // 
            // numMaxRetryCount
            // 
            numMaxRetryCount.Location = new Point(150, 257);
            numMaxRetryCount.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numMaxRetryCount.Name = "numMaxRetryCount";
            numMaxRetryCount.Size = new Size(100, 23);
            numMaxRetryCount.TabIndex = 13;
            numMaxRetryCount.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // lblLanguage
            // 
            lblLanguage.AutoSize = true;
            lblLanguage.Location = new Point(20, 300);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new Size(59, 17);
            lblLanguage.TabIndex = 14;
            lblLanguage.Text = "界面语言:";
            // 
            // cmbLanguage
            // 
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.Items.AddRange(new object[] { Resources.Strings.Language_ChineseSimplified, Resources.Strings.Language_ChineseTraditional, Resources.Strings.Language_English, Resources.Strings.Language_Japanese, Resources.Strings.Language_Vietnamese });
            cmbLanguage.Location = new Point(150, 297);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(200, 25);
            cmbLanguage.TabIndex = 15;
            // 
            // tabAdvanced
            // 
            tabAdvanced.BackColor = Color.White;
            tabAdvanced.Controls.Add(lblWatchFolders);
            tabAdvanced.Controls.Add(lstWatchFolders);
            tabAdvanced.Controls.Add(btnAddFolder);
            tabAdvanced.Controls.Add(btnRemoveFolder);
            tabAdvanced.Controls.Add(chkScanSubfolders);
            tabAdvanced.Controls.Add(chkPreserveDirectoryStructure);
            tabAdvanced.Controls.Add(grpSystemTray);
            tabAdvanced.Location = new Point(4, 26);
            tabAdvanced.Name = "tabAdvanced";
            tabAdvanced.Padding = new Padding(20);
            tabAdvanced.Size = new Size(692, 460);
            tabAdvanced.TabIndex = 2;
            tabAdvanced.Text = Resources.Strings.Tab_Advanced;
            // 
            // lblWatchFolders
            // 
            lblWatchFolders.AutoSize = true;
            lblWatchFolders.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            lblWatchFolders.Location = new Point(20, 20);
            lblWatchFolders.Name = "lblWatchFolders";
            lblWatchFolders.Size = new Size(95, 17);
            lblWatchFolders.TabIndex = 0;
            lblWatchFolders.Text = "监控文件夹列表:";
            // 
            // lstWatchFolders
            // 
            lstWatchFolders.ItemHeight = 17;
            lstWatchFolders.Location = new Point(20, 45);
            lstWatchFolders.Name = "lstWatchFolders";
            lstWatchFolders.Size = new Size(550, 106);
            lstWatchFolders.TabIndex = 1;
            // 
            // btnAddFolder
            // 
            btnAddFolder.Location = new Point(580, 45);
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Size = new Size(90, 30);
            btnAddFolder.TabIndex = 2;
            btnAddFolder.Text = Resources.Strings.Button_AddFolder;
            btnAddFolder.Click += btnAddFolder_Click;
            // 
            // btnRemoveFolder
            // 
            btnRemoveFolder.Location = new Point(580, 85);
            btnRemoveFolder.Name = "btnRemoveFolder";
            btnRemoveFolder.Size = new Size(90, 30);
            btnRemoveFolder.TabIndex = 3;
            btnRemoveFolder.Text = Resources.Strings.Button_RemoveFolder;
            btnRemoveFolder.Click += btnRemoveFolder_Click;
            // 
            // chkScanSubfolders
            // 
            chkScanSubfolders.AutoSize = true;
            chkScanSubfolders.Location = new Point(20, 180);
            chkScanSubfolders.Name = "chkScanSubfolders";
            chkScanSubfolders.Size = new Size(243, 21);
            chkScanSubfolders.TabIndex = 4;
            chkScanSubfolders.Text = Resources.Strings.Label_ScanSubfolders;
            // 
            // chkPreserveDirectoryStructure
            // 
            chkPreserveDirectoryStructure.AutoSize = true;
            chkPreserveDirectoryStructure.Location = new Point(20, 210);
            chkPreserveDirectoryStructure.Name = "chkPreserveDirectoryStructure";
            chkPreserveDirectoryStructure.Size = new Size(189, 21);
            chkPreserveDirectoryStructure.TabIndex = 5;
            chkPreserveDirectoryStructure.Text = Resources.Strings.Label_PreserveDirectoryStructure;
            // 
            // grpSystemTray
            // 
            grpSystemTray.Controls.Add(chkSystemTrayEnabled);
            grpSystemTray.Controls.Add(chkMinimizeToTray);
            grpSystemTray.Controls.Add(lblCloseAction);
            grpSystemTray.Controls.Add(cmbCloseAction);
            grpSystemTray.Location = new Point(20, 250);
            grpSystemTray.Name = "grpSystemTray";
            grpSystemTray.Size = new Size(650, 150);
            grpSystemTray.TabIndex = 6;
            grpSystemTray.TabStop = false;
            grpSystemTray.Text = "系统托盘配置";
            // 
            // chkSystemTrayEnabled
            // 
            chkSystemTrayEnabled.AutoSize = true;
            chkSystemTrayEnabled.Location = new Point(20, 30);
            chkSystemTrayEnabled.Name = "chkSystemTrayEnabled";
            chkSystemTrayEnabled.Size = new Size(123, 21);
            chkSystemTrayEnabled.TabIndex = 0;
            chkSystemTrayEnabled.Text = Resources.Strings.Label_EnableSystemTray;
            chkSystemTrayEnabled.CheckedChanged += chkSystemTrayEnabled_CheckedChanged;
            // 
            // chkMinimizeToTray
            // 
            chkMinimizeToTray.AutoSize = true;
            chkMinimizeToTray.Location = new Point(20, 60);
            chkMinimizeToTray.Name = "chkMinimizeToTray";
            chkMinimizeToTray.Size = new Size(159, 21);
            chkMinimizeToTray.TabIndex = 1;
            chkMinimizeToTray.Text = Resources.Strings.Label_MinimizeToTray;
            // 
            // lblCloseAction
            // 
            lblCloseAction.AutoSize = true;
            lblCloseAction.Location = new Point(20, 95);
            lblCloseAction.Name = "lblCloseAction";
            lblCloseAction.Size = new Size(107, 17);
            lblCloseAction.TabIndex = 2;
            lblCloseAction.Text = "关闭窗口时的行为:";
            // 
            // cmbCloseAction
            // 
            cmbCloseAction.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCloseAction.Items.AddRange(new object[] { Resources.Strings.CloseAction_Ask, Resources.Strings.CloseAction_Minimize, Resources.Strings.CloseAction_Exit });
            cmbCloseAction.Location = new Point(150, 92);
            cmbCloseAction.Name = "cmbCloseAction";
            cmbCloseAction.Size = new Size(200, 25);
            cmbCloseAction.TabIndex = 3;
            // 
            // tabService
            // 
            tabService.BackColor = Color.White;
            tabService.Controls.Add(lblServiceCenterApi);
            tabService.Controls.Add(txtServiceCenterApi);
            tabService.Controls.Add(lblServiceName);
            tabService.Controls.Add(lblServiceNameValue);
            tabService.Controls.Add(chkAutoRegister);
            tabService.Controls.Add(chkEnableHeartbeat);
            tabService.Controls.Add(lblHeartbeatInterval);
            tabService.Controls.Add(numHeartbeatInterval);
            tabService.Controls.Add(lblLastHeartbeat);
            tabService.Controls.Add(lblLastHeartbeatValue);
            tabService.Controls.Add(lblRegistrationStatus);
            tabService.Controls.Add(lblStatusValue);
            tabService.Controls.Add(btnServiceRegister);
            tabService.Controls.Add(btnUploadConfig);
            tabService.Controls.Add(btnDownloadConfig);
            tabService.Controls.Add(lblOperationStatus);
            tabService.Controls.Add(progressService);
            tabService.Location = new Point(4, 26);
            tabService.Name = "tabService";
            tabService.Padding = new Padding(20);
            tabService.Size = new Size(692, 460);
            tabService.TabIndex = 1;
            tabService.Text = "服务注册";
            // 
            // lblServiceCenterApi
            // 
            lblServiceCenterApi.AutoSize = true;
            lblServiceCenterApi.Location = new Point(20, 20);
            lblServiceCenterApi.Name = "lblServiceCenterApi";
            lblServiceCenterApi.Size = new Size(78, 17);
            lblServiceCenterApi.TabIndex = 0;
            lblServiceCenterApi.Text = "服务中心API:";
            // 
            // txtServiceCenterApi
            // 
            txtServiceCenterApi.Location = new Point(150, 17);
            txtServiceCenterApi.Name = "txtServiceCenterApi";
            txtServiceCenterApi.Size = new Size(500, 23);
            txtServiceCenterApi.TabIndex = 1;
            // 
            // lblServiceName
            // 
            lblServiceName.AutoSize = true;
            lblServiceName.Location = new Point(20, 60);
            lblServiceName.Name = "lblServiceName";
            lblServiceName.Size = new Size(59, 17);
            lblServiceName.TabIndex = 2;
            lblServiceName.Text = "服务名称:";
            // 
            // lblServiceNameValue
            // 
            lblServiceNameValue.AutoSize = true;
            lblServiceNameValue.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            lblServiceNameValue.ForeColor = Color.FromArgb(41, 128, 185);
            lblServiceNameValue.Location = new Point(150, 57);
            lblServiceNameValue.Name = "lblServiceNameValue";
            lblServiceNameValue.Size = new Size(13, 17);
            lblServiceNameValue.TabIndex = 3;
            lblServiceNameValue.Text = "-";
            // 
            // chkAutoRegister
            // 
            chkAutoRegister.AutoSize = true;
            chkAutoRegister.Location = new Point(20, 100);
            chkAutoRegister.Name = "chkAutoRegister";
            chkAutoRegister.Size = new Size(99, 21);
            chkAutoRegister.TabIndex = 4;
            chkAutoRegister.Text = "自动注册服务";
            // 
            // chkEnableHeartbeat
            // 
            chkEnableHeartbeat.AutoSize = true;
            chkEnableHeartbeat.Location = new Point(20, 130);
            chkEnableHeartbeat.Name = "chkEnableHeartbeat";
            chkEnableHeartbeat.Size = new Size(75, 21);
            chkEnableHeartbeat.TabIndex = 5;
            chkEnableHeartbeat.Text = "启用心跳";
            chkEnableHeartbeat.CheckedChanged += chkEnableHeartbeat_CheckedChanged;
            // 
            // lblHeartbeatInterval
            // 
            lblHeartbeatInterval.AutoSize = true;
            lblHeartbeatInterval.Location = new Point(40, 160);
            lblHeartbeatInterval.Name = "lblHeartbeatInterval";
            lblHeartbeatInterval.Size = new Size(79, 17);
            lblHeartbeatInterval.TabIndex = 6;
            lblHeartbeatInterval.Text = "心跳间隔(秒):";
            // 
            // numHeartbeatInterval
            // 
            numHeartbeatInterval.Location = new Point(150, 157);
            numHeartbeatInterval.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            numHeartbeatInterval.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            numHeartbeatInterval.Name = "numHeartbeatInterval";
            numHeartbeatInterval.Size = new Size(100, 23);
            numHeartbeatInterval.TabIndex = 7;
            numHeartbeatInterval.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // lblLastHeartbeat
            // 
            lblLastHeartbeat.AutoSize = true;
            lblLastHeartbeat.Location = new Point(40, 190);
            lblLastHeartbeat.Name = "lblLastHeartbeat";
            lblLastHeartbeat.Size = new Size(59, 17);
            lblLastHeartbeat.TabIndex = 8;
            lblLastHeartbeat.Text = "最后心跳:";
            // 
            // lblLastHeartbeatValue
            // 
            lblLastHeartbeatValue.AutoSize = true;
            lblLastHeartbeatValue.Font = new Font("Microsoft YaHei UI", 9F);
            lblLastHeartbeatValue.ForeColor = Color.FromArgb(127, 140, 141);
            lblLastHeartbeatValue.Location = new Point(150, 187);
            lblLastHeartbeatValue.Name = "lblLastHeartbeatValue";
            lblLastHeartbeatValue.Size = new Size(13, 17);
            lblLastHeartbeatValue.TabIndex = 9;
            lblLastHeartbeatValue.Text = "-";
            // 
            // lblRegistrationStatus
            // 
            lblRegistrationStatus.AutoSize = true;
            lblRegistrationStatus.Location = new Point(20, 230);
            lblRegistrationStatus.Name = "lblRegistrationStatus";
            lblRegistrationStatus.Size = new Size(59, 17);
            lblRegistrationStatus.TabIndex = 10;
            lblRegistrationStatus.Text = "注册状态:";
            // 
            // lblStatusValue
            // 
            lblStatusValue.AutoSize = true;
            lblStatusValue.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            lblStatusValue.ForeColor = Color.FromArgb(192, 57, 43);
            lblStatusValue.Location = new Point(150, 227);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new Size(44, 17);
            lblStatusValue.TabIndex = 11;
            lblStatusValue.Text = "未注册";
            // 
            // btnServiceRegister
            // 
            btnServiceRegister.BackColor = Color.FromArgb(39, 174, 96);
            btnServiceRegister.FlatStyle = FlatStyle.Flat;
            btnServiceRegister.ForeColor = Color.White;
            btnServiceRegister.Location = new Point(20, 270);
            btnServiceRegister.Name = "btnServiceRegister";
            btnServiceRegister.Size = new Size(120, 35);
            btnServiceRegister.TabIndex = 12;
            btnServiceRegister.Text = "服务注册";
            btnServiceRegister.UseVisualStyleBackColor = false;
            btnServiceRegister.Click += BtnServiceRegister_Click;
            // 
            // btnUploadConfig
            // 
            btnUploadConfig.BackColor = Color.FromArgb(52, 152, 219);
            btnUploadConfig.FlatStyle = FlatStyle.Flat;
            btnUploadConfig.ForeColor = Color.White;
            btnUploadConfig.Location = new Point(150, 270);
            btnUploadConfig.Name = "btnUploadConfig";
            btnUploadConfig.Size = new Size(120, 35);
            btnUploadConfig.TabIndex = 13;
            btnUploadConfig.Text = "配置上传";
            btnUploadConfig.UseVisualStyleBackColor = false;
            btnUploadConfig.Click += BtnUploadConfig_Click;
            // 
            // btnDownloadConfig
            // 
            btnDownloadConfig.BackColor = Color.FromArgb(155, 89, 182);
            btnDownloadConfig.FlatStyle = FlatStyle.Flat;
            btnDownloadConfig.ForeColor = Color.White;
            btnDownloadConfig.Location = new Point(280, 270);
            btnDownloadConfig.Name = "btnDownloadConfig";
            btnDownloadConfig.Size = new Size(120, 35);
            btnDownloadConfig.TabIndex = 14;
            btnDownloadConfig.Text = "配置下载";
            btnDownloadConfig.UseVisualStyleBackColor = false;
            btnDownloadConfig.Click += BtnDownloadConfig_Click;
            // 
            // lblOperationStatus
            // 
            lblOperationStatus.AutoSize = true;
            lblOperationStatus.Location = new Point(20, 320);
            lblOperationStatus.Name = "lblOperationStatus";
            lblOperationStatus.Size = new Size(59, 17);
            lblOperationStatus.TabIndex = 15;
            lblOperationStatus.Text = "操作状态:";
            // 
            // progressService
            // 
            progressService.Location = new Point(20, 350);
            progressService.Name = "progressService";
            progressService.Size = new Size(650, 23);
            progressService.Style = ProgressBarStyle.Continuous;
            progressService.TabIndex = 16;
            // 
            // tabPerformance
            // 
            tabPerformance.BackColor = Color.White;
            tabPerformance.Controls.Add(grpThreadPool);
            tabPerformance.Controls.Add(grpImageCompression);
            tabPerformance.Location = new Point(4, 26);
            tabPerformance.Name = "tabPerformance";
            tabPerformance.Padding = new Padding(20);
            tabPerformance.Size = new Size(692, 460);
            tabPerformance.TabIndex = 2;
            tabPerformance.Text = Resources.Strings.Tab_Performance;
            // 
            // grpThreadPool
            // 
            grpThreadPool.Controls.Add(chkEnableThreadPool);
            grpThreadPool.Controls.Add(lblThreadPoolSize);
            grpThreadPool.Controls.Add(numThreadPoolSize);
            grpThreadPool.Location = new Point(20, 20);
            grpThreadPool.Name = "grpThreadPool";
            grpThreadPool.Size = new Size(650, 120);
            grpThreadPool.TabIndex = 0;
            grpThreadPool.TabStop = false;
            grpThreadPool.Text = "线程池配置";
            // 
            // chkEnableThreadPool
            // 
            chkEnableThreadPool.AutoSize = true;
            chkEnableThreadPool.Location = new Point(20, 30);
            chkEnableThreadPool.Name = "chkEnableThreadPool";
            chkEnableThreadPool.Size = new Size(231, 21);
            chkEnableThreadPool.TabIndex = 0;
            chkEnableThreadPool.Text = Resources.Strings.Label_EnableThreadPool;
            chkEnableThreadPool.CheckedChanged += chkEnableThreadPool_CheckedChanged;
            // 
            // lblThreadPoolSize
            // 
            lblThreadPoolSize.AutoSize = true;
            lblThreadPoolSize.Location = new Point(20, 70);
            lblThreadPoolSize.Name = "lblThreadPoolSize";
            lblThreadPoolSize.Size = new Size(143, 17);
            lblThreadPoolSize.TabIndex = 1;
            lblThreadPoolSize.Text = "线程池大小（并发数量）:";
            // 
            // numThreadPoolSize
            // 
            numThreadPoolSize.Location = new Point(200, 67);
            numThreadPoolSize.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numThreadPoolSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numThreadPoolSize.Name = "numThreadPoolSize";
            numThreadPoolSize.Size = new Size(100, 23);
            numThreadPoolSize.TabIndex = 2;
            numThreadPoolSize.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // grpImageCompression
            // 
            grpImageCompression.Controls.Add(chkEnableImageCompression);
            grpImageCompression.Controls.Add(lblCompressionThreshold);
            grpImageCompression.Controls.Add(numCompressionThreshold);
            grpImageCompression.Controls.Add(lblCompressionQuality);
            grpImageCompression.Controls.Add(numCompressionQuality);
            grpImageCompression.Location = new Point(20, 160);
            grpImageCompression.Name = "grpImageCompression";
            grpImageCompression.Size = new Size(650, 180);
            grpImageCompression.TabIndex = 1;
            grpImageCompression.TabStop = false;
            grpImageCompression.Text = "图片压缩配置";
            // 
            // chkEnableImageCompression
            // 
            chkEnableImageCompression.AutoSize = true;
            chkEnableImageCompression.Location = new Point(20, 30);
            chkEnableImageCompression.Name = "chkEnableImageCompression";
            chkEnableImageCompression.Size = new Size(207, 21);
            chkEnableImageCompression.TabIndex = 0;
            chkEnableImageCompression.Text = Resources.Strings.Label_EnableImageCompression;
            chkEnableImageCompression.CheckedChanged += chkEnableImageCompression_CheckedChanged;
            // 
            // lblCompressionThreshold
            // 
            lblCompressionThreshold.AutoSize = true;
            lblCompressionThreshold.Location = new Point(20, 70);
            lblCompressionThreshold.Name = "lblCompressionThreshold";
            lblCompressionThreshold.Size = new Size(195, 17);
            lblCompressionThreshold.TabIndex = 1;
            lblCompressionThreshold.Text = "压缩阈值（KB，大于此值将压缩）:";
            // 
            // numCompressionThreshold
            // 
            numCompressionThreshold.Location = new Point(250, 67);
            numCompressionThreshold.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numCompressionThreshold.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numCompressionThreshold.Name = "numCompressionThreshold";
            numCompressionThreshold.Size = new Size(100, 23);
            numCompressionThreshold.TabIndex = 2;
            numCompressionThreshold.Value = new decimal(new int[] { 500, 0, 0, 0 });
            // 
            // lblCompressionQuality
            // 
            lblCompressionQuality.AutoSize = true;
            lblCompressionQuality.Location = new Point(20, 110);
            lblCompressionQuality.Name = "lblCompressionQuality";
            lblCompressionQuality.Size = new Size(224, 17);
            lblCompressionQuality.TabIndex = 3;
            lblCompressionQuality.Text = "压缩质量（1-100，值越小压缩率越高）:";
            // 
            // numCompressionQuality
            // 
            numCompressionQuality.Location = new Point(250, 107);
            numCompressionQuality.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numCompressionQuality.Name = "numCompressionQuality";
            numCompressionQuality.Size = new Size(100, 23);
            numCompressionQuality.TabIndex = 4;
            numCompressionQuality.Value = new decimal(new int[] { 75, 0, 0, 0 });
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.FromArgb(236, 240, 241);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 550);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(700, 60);
            panelButtons.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(39, 174, 96);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(470, 10);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 40);
            btnSave.TabIndex = 0;
            btnSave.Text = Resources.Strings.Button_Save;
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(189, 195, 199);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(580, 10);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 40);
            btnCancel.TabIndex = 1;
            btnCancel.Text = Resources.Strings.Button_Cancel;
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 610);
            Controls.Add(tabControl);
            Controls.Add(panelTop);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConfigForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "系统配置";
            Load += ConfigForm_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            tabControl.ResumeLayout(false);
            tabBasic.ResumeLayout(false);
            tabBasic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numRequestTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)numScanInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxRetryCount).EndInit();
            tabAdvanced.ResumeLayout(false);
            tabAdvanced.PerformLayout();
            grpSystemTray.ResumeLayout(false);
            grpSystemTray.PerformLayout();
            tabService.ResumeLayout(false);
            tabService.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numHeartbeatInterval).EndInit();
            tabPerformance.ResumeLayout(false);
            grpThreadPool.ResumeLayout(false);
            grpThreadPool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numThreadPoolSize).EndInit();
            grpImageCompression.ResumeLayout(false);
            grpImageCompression.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCompressionThreshold).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCompressionQuality).EndInit();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel panelTop;
        private Label lblTitle;
        private TabControl tabControl;
        private TabPage tabBasic;
        private TabPage tabAdvanced;
        private TabPage tabPerformance;
        private TabPage tabService;
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

        private Label lblServiceCenterApi;
        private TextBox txtServiceCenterApi;
        private Label lblServiceName;
        private Label lblServiceNameValue;
        private CheckBox chkAutoRegister;
        private CheckBox chkEnableHeartbeat;
        private Label lblHeartbeatInterval;
        private NumericUpDown numHeartbeatInterval;
        private Label lblLastHeartbeat;
        private Label lblLastHeartbeatValue;
        private Label lblRegistrationStatus;
        private Label lblStatusValue;
        private Button btnServiceRegister;
        private Button btnUploadConfig;
        private Button btnDownloadConfig;
        private ProgressBar progressService;
        private Label lblOperationStatus;
        private Label lblWatchFolders;
    }
}

