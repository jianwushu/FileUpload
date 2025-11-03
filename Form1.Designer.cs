namespace FileUpload
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuConfig = new ToolStripMenuItem();
            menuExit = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            panelTop = new Panel();
            lblTitle = new Label();
            panelConfig = new Panel();
            lblParseRuleValue = new Label();
            lblParseRule = new Label();
            lblUploadUrlValue = new Label();
            lblUploadUrl = new Label();
            lblDeviceIdValue = new Label();
            lblDeviceId = new Label();
            lblWatchFolderValue = new Label();
            lblWatchFolder = new Label();
            lblConfigTitle = new Label();
            panelControl = new Panel();
            lblStatusValue = new Label();
            lblStatus = new Label();
            btnStop = new Button();
            btnStart = new Button();
            lblFailedValue = new Label();
            lblSuccessValue = new Label();
            lblTotalValue = new Label();
            lblFailed = new Label();
            lblSuccess = new Label();
            lblTotal = new Label();
            panelLog = new Panel();
            txtLog = new TextBox();
            panelLogHeader = new Panel();
            btnClearLog = new Button();
            lblLogTitle = new Label();
            menuStrip.SuspendLayout();
            panelTop.SuspendLayout();
            panelConfig.SuspendLayout();
            panelControl.SuspendLayout();
            panelLog.SuspendLayout();
            panelLogHeader.SuspendLayout();
            SuspendLayout();
            //
            // menuStrip
            //
            menuStrip.BackColor = Color.White;
            menuStrip.Items.AddRange(new ToolStripItem[] { menuFile, menuHelp });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1000, 25);
            menuStrip.TabIndex = 5;
            menuStrip.Text = "menuStrip";
            //
            // menuFile
            //
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuConfig, menuExit });
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(58, 21);
            menuFile.Text = Resources.Strings.Menu_File;
            //
            // menuConfig
            //
            menuConfig.Name = "menuConfig";
            menuConfig.Size = new Size(180, 22);
            menuConfig.Text = Resources.Strings.Menu_Config;
            menuConfig.Click += menuConfig_Click;
            //
            // menuExit
            //
            menuExit.Name = "menuExit";
            menuExit.Size = new Size(180, 22);
            menuExit.Text = Resources.Strings.Menu_Exit;
            menuExit.Click += menuExit_Click;
            //
            // menuHelp
            //
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout });
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(61, 21);
            menuHelp.Text = Resources.Strings.Menu_Help;
            //
            // menuAbout
            //
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(180, 22);
            menuAbout.Text = Resources.Strings.Menu_About;
            menuAbout.Click += menuAbout_Click;
            //
            // panelTop
            //
            panelTop.BackColor = Color.FromArgb(41, 128, 185);
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 25);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1000, 60);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(158, 31);
            lblTitle.TabIndex = 0;
            lblTitle.Text = Resources.Strings.Form_Title;
            // 
            // panelConfig
            //
            panelConfig.BackColor = Color.White;
            panelConfig.BorderStyle = BorderStyle.FixedSingle;
            panelConfig.Controls.Add(lblParseRuleValue);
            panelConfig.Controls.Add(lblParseRule);
            panelConfig.Controls.Add(lblUploadUrlValue);
            panelConfig.Controls.Add(lblUploadUrl);
            panelConfig.Controls.Add(lblDeviceIdValue);
            panelConfig.Controls.Add(lblDeviceId);
            panelConfig.Controls.Add(lblWatchFolderValue);
            panelConfig.Controls.Add(lblWatchFolder);
            panelConfig.Controls.Add(lblConfigTitle);
            panelConfig.Location = new Point(20, 105);
            panelConfig.Name = "panelConfig";
            panelConfig.Size = new Size(960, 140);
            panelConfig.TabIndex = 1;
            // 
            // lblParseRuleValue
            //
            lblParseRuleValue.AutoSize = true;
            lblParseRuleValue.Font = new Font("Microsoft YaHei UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            lblParseRuleValue.ForeColor = Color.FromArgb(52, 73, 94);
            lblParseRuleValue.Location = new Point(520, 45);
            lblParseRuleValue.MaximumSize = new Size(420, 0);
            lblParseRuleValue.Name = "lblParseRuleValue";
            lblParseRuleValue.Size = new Size(11, 16);
            lblParseRuleValue.TabIndex = 8;
            lblParseRuleValue.Text = "-";
            //
            // lblParseRule
            //
            lblParseRule.AutoSize = true;
            lblParseRule.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblParseRule.ForeColor = Color.FromArgb(52, 73, 94);
            lblParseRule.Location = new Point(520, 45);
            lblParseRule.Name = "lblParseRule";
            lblParseRule.Size = new Size(80, 17);
            lblParseRule.TabIndex = 7;
            lblParseRule.Text = Resources.Strings.Label_ParseRule;
            lblParseRule.Visible = false;
            //
            // lblUploadUrlValue
            //
            lblUploadUrlValue.AutoSize = true;
            lblUploadUrlValue.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblUploadUrlValue.ForeColor = Color.FromArgb(52, 73, 94);
            lblUploadUrlValue.Location = new Point(120, 105);
            lblUploadUrlValue.Name = "lblUploadUrlValue";
            lblUploadUrlValue.Size = new Size(13, 17);
            lblUploadUrlValue.TabIndex = 6;
            lblUploadUrlValue.Text = "-";
            //
            // lblUploadUrl
            //
            lblUploadUrl.AutoSize = true;
            lblUploadUrl.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblUploadUrl.ForeColor = Color.FromArgb(52, 73, 94);
            lblUploadUrl.Location = new Point(15, 105);
            lblUploadUrl.Name = "lblUploadUrl";
            lblUploadUrl.Size = new Size(68, 17);
            lblUploadUrl.TabIndex = 5;
            lblUploadUrl.Text = Resources.Strings.Label_UploadUrl;
            // 
            // lblDeviceIdValue
            // 
            lblDeviceIdValue.AutoSize = true;
            lblDeviceIdValue.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblDeviceIdValue.ForeColor = Color.FromArgb(52, 73, 94);
            lblDeviceIdValue.Location = new Point(120, 45);
            lblDeviceIdValue.Name = "lblDeviceIdValue";
            lblDeviceIdValue.Size = new Size(13, 17);
            lblDeviceIdValue.TabIndex = 4;
            lblDeviceIdValue.Text = "-";
            // 
            // lblDeviceId
            //
            lblDeviceId.AutoSize = true;
            lblDeviceId.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblDeviceId.ForeColor = Color.FromArgb(52, 73, 94);
            lblDeviceId.Location = new Point(15, 45);
            lblDeviceId.Name = "lblDeviceId";
            lblDeviceId.Size = new Size(68, 17);
            lblDeviceId.TabIndex = 3;
            lblDeviceId.Text = Resources.Strings.Label_DeviceId;
            //
            // lblWatchFolderValue
            //
            lblWatchFolderValue.AutoSize = true;
            lblWatchFolderValue.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblWatchFolderValue.ForeColor = Color.FromArgb(52, 73, 94);
            lblWatchFolderValue.Location = new Point(120, 75);
            lblWatchFolderValue.Name = "lblWatchFolderValue";
            lblWatchFolderValue.Size = new Size(13, 17);
            lblWatchFolderValue.TabIndex = 2;
            lblWatchFolderValue.Text = "-";
            //
            // lblWatchFolder
            //
            lblWatchFolder.AutoSize = true;
            lblWatchFolder.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblWatchFolder.ForeColor = Color.FromArgb(52, 73, 94);
            lblWatchFolder.Location = new Point(15, 75);
            lblWatchFolder.Name = "lblWatchFolder";
            lblWatchFolder.Size = new Size(80, 17);
            lblWatchFolder.TabIndex = 1;
            lblWatchFolder.Text = Resources.Strings.Label_WatchFolder;
            //
            // lblConfigTitle
            //
            lblConfigTitle.BackColor = Color.FromArgb(236, 240, 241);
            lblConfigTitle.Dock = DockStyle.Top;
            lblConfigTitle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblConfigTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblConfigTitle.Location = new Point(0, 0);
            lblConfigTitle.Name = "lblConfigTitle";
            lblConfigTitle.Padding = new Padding(10, 0, 0, 0);
            lblConfigTitle.Size = new Size(958, 30);
            lblConfigTitle.TabIndex = 0;
            lblConfigTitle.Text = Resources.Strings.Config_Title;
            lblConfigTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelControl (合并统计和控制)
            //
            panelControl.BackColor = Color.White;
            panelControl.BorderStyle = BorderStyle.FixedSingle;
            panelControl.Controls.Add(lblFailedValue);
            panelControl.Controls.Add(lblSuccessValue);
            panelControl.Controls.Add(lblTotalValue);
            panelControl.Controls.Add(lblFailed);
            panelControl.Controls.Add(lblSuccess);
            panelControl.Controls.Add(lblTotal);
            panelControl.Controls.Add(lblStatusValue);
            panelControl.Controls.Add(lblStatus);
            panelControl.Controls.Add(btnStop);
            panelControl.Controls.Add(btnStart);
            panelControl.Location = new Point(20, 265);
            panelControl.Name = "panelControl";
            panelControl.Size = new Size(960, 70);
            panelControl.TabIndex = 2;
            // 
            // lblStatusValue
            //
            lblStatusValue.AutoSize = true;
            lblStatusValue.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatusValue.ForeColor = Color.FromArgb(192, 57, 43);
            lblStatusValue.Location = new Point(80, 25);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new Size(51, 19);
            lblStatusValue.TabIndex = 3;
            lblStatusValue.Text = Resources.Strings.Status_NotRunning;
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
            lblStatus.Location = new Point(15, 25);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(51, 19);
            lblStatus.TabIndex = 2;
            lblStatus.Text = Resources.Strings.Label_Status;
            //
            // lblTotalValue
            //
            lblTotalValue.AutoSize = true;
            lblTotalValue.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblTotalValue.ForeColor = Color.FromArgb(41, 128, 185);
            lblTotalValue.Location = new Point(250, 25);
            lblTotalValue.Name = "lblTotalValue";
            lblTotalValue.Size = new Size(18, 19);
            lblTotalValue.TabIndex = 4;
            lblTotalValue.Text = "0";
            //
            // lblTotal
            //
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblTotal.ForeColor = Color.FromArgb(127, 140, 141);
            lblTotal.Location = new Point(180, 27);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(56, 17);
            lblTotal.TabIndex = 1;
            lblTotal.Text = Resources.Strings.Label_Total;
            //
            // lblSuccessValue
            //
            lblSuccessValue.AutoSize = true;
            lblSuccessValue.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblSuccessValue.ForeColor = Color.FromArgb(39, 174, 96);
            lblSuccessValue.Location = new Point(370, 25);
            lblSuccessValue.Name = "lblSuccessValue";
            lblSuccessValue.Size = new Size(18, 19);
            lblSuccessValue.TabIndex = 5;
            lblSuccessValue.Text = "0";
            //
            // lblSuccess
            //
            lblSuccess.AutoSize = true;
            lblSuccess.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSuccess.ForeColor = Color.FromArgb(127, 140, 141);
            lblSuccess.Location = new Point(300, 27);
            lblSuccess.Name = "lblSuccess";
            lblSuccess.Size = new Size(56, 17);
            lblSuccess.TabIndex = 2;
            lblSuccess.Text = Resources.Strings.Label_Success;
            //
            // lblFailedValue
            //
            lblFailedValue.AutoSize = true;
            lblFailedValue.Font = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lblFailedValue.ForeColor = Color.FromArgb(231, 76, 60);
            lblFailedValue.Location = new Point(490, 25);
            lblFailedValue.Name = "lblFailedValue";
            lblFailedValue.Size = new Size(18, 19);
            lblFailedValue.TabIndex = 6;
            lblFailedValue.Text = "0";
            //
            // lblFailed
            //
            lblFailed.AutoSize = true;
            lblFailed.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblFailed.ForeColor = Color.FromArgb(127, 140, 141);
            lblFailed.Location = new Point(420, 27);
            lblFailed.Name = "lblFailed";
            lblFailed.Size = new Size(56, 17);
            lblFailed.TabIndex = 3;
            lblFailed.Text = Resources.Strings.Label_Failed;
            //
            // btnStop
            //
            btnStop.BackColor = Color.FromArgb(231, 76, 60);
            btnStop.Enabled = false;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnStop.ForeColor = Color.White;
            btnStop.Location = new Point(830, 15);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(110, 40);
            btnStop.TabIndex = 1;
            btnStop.Text = Resources.Strings.Button_Stop;
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            //
            // btnStart
            //
            btnStart.BackColor = Color.FromArgb(39, 174, 96);
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(700, 15);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(110, 40);
            btnStart.TabIndex = 0;
            btnStart.Text = Resources.Strings.Button_Start;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            //
            // panelLog
            //
            panelLog.BackColor = Color.White;
            panelLog.BorderStyle = BorderStyle.FixedSingle;
            panelLog.Controls.Add(txtLog);
            panelLog.Controls.Add(panelLogHeader);
            panelLog.Location = new Point(20, 355);
            panelLog.Name = "panelLog";
            panelLog.Size = new Size(960, 345);
            panelLog.TabIndex = 4;
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(44, 62, 80);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtLog.ForeColor = Color.FromArgb(236, 240, 241);
            txtLog.Location = new Point(0, 40);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(958, 328);
            txtLog.TabIndex = 1;
            // 
            // panelLogHeader
            // 
            panelLogHeader.BackColor = Color.FromArgb(236, 240, 241);
            panelLogHeader.Controls.Add(btnClearLog);
            panelLogHeader.Controls.Add(lblLogTitle);
            panelLogHeader.Dock = DockStyle.Top;
            panelLogHeader.Location = new Point(0, 0);
            panelLogHeader.Name = "panelLogHeader";
            panelLogHeader.Size = new Size(958, 40);
            panelLogHeader.TabIndex = 0;
            // 
            // btnClearLog
            //
            btnClearLog.BackColor = Color.FromArgb(189, 195, 199);
            btnClearLog.FlatAppearance.BorderSize = 0;
            btnClearLog.FlatStyle = FlatStyle.Flat;
            btnClearLog.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnClearLog.ForeColor = Color.White;
            btnClearLog.Location = new Point(860, 8);
            btnClearLog.Name = "btnClearLog";
            btnClearLog.Size = new Size(80, 25);
            btnClearLog.TabIndex = 1;
            btnClearLog.Text = Resources.Strings.Button_ClearLog;
            btnClearLog.UseVisualStyleBackColor = false;
            btnClearLog.Click += btnClearLog_Click;
            //
            // lblLogTitle
            //
            lblLogTitle.AutoSize = true;
            lblLogTitle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblLogTitle.ForeColor = Color.FromArgb(52, 73, 94);
            lblLogTitle.Location = new Point(10, 11);
            lblLogTitle.Name = "lblLogTitle";
            lblLogTitle.Size = new Size(65, 19);
            lblLogTitle.TabIndex = 0;
            lblLogTitle.Text = Resources.Strings.Log_Title;
            // 
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(1000, 720);
            Controls.Add(panelLog);
            Controls.Add(panelControl);
            Controls.Add(panelConfig);
            Controls.Add(panelTop);
            Controls.Add(menuStrip);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip;
            MaximizeBox = false;
            Name = "Form1";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = Resources.Strings.Form_WindowTitle;
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelConfig.ResumeLayout(false);
            panelConfig.PerformLayout();
            panelControl.ResumeLayout(false);
            panelControl.PerformLayout();
            panelLog.ResumeLayout(false);
            panelLog.PerformLayout();
            panelLogHeader.ResumeLayout(false);
            panelLogHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuConfig;
        private ToolStripMenuItem menuExit;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuAbout;
        private Panel panelTop;
        private Label lblTitle;
        private Panel panelConfig;
        private Label lblConfigTitle;
        private Label lblDeviceId;
        private Label lblWatchFolderValue;
        private Label lblWatchFolder;
        private Label lblParseRuleValue;
        private Label lblParseRule;
        private Label lblUploadUrlValue;
        private Label lblUploadUrl;
        private Label lblDeviceIdValue;
        private Panel panelControl;
        private Button btnStart;
        private Button btnStop;
        private Label lblStatus;
        private Label lblStatusValue;
        private Label lblTotal;
        private Label lblSuccess;
        private Label lblFailed;
        private Label lblTotalValue;
        private Label lblSuccessValue;
        private Label lblFailedValue;
        private Panel panelLog;
        private Panel panelLogHeader;
        private Label lblLogTitle;
        private TextBox txtLog;
        private Button btnClearLog;
    }
}
