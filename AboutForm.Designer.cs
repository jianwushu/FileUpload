namespace FileUpload
{
    partial class AboutForm
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
            this.panelContent = new Panel();
            this.lblVersion = new Label();
            this.lblDescription = new Label();
            this.lblFeatures = new Label();
            this.lblCopyright = new Label();
            this.btnClose = new Button();
            
            this.panelTop.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            
            // panelTop
            this.panelTop.BackColor = Color.FromArgb(41, 128, 185);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Location = new Point(0, 0);
            this.panelTop.Size = new Size(500, 80);
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Microsoft YaHei UI", 20F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(20, 20);
            this.lblTitle.Text = Resources.Strings.About_Title;

            // panelContent
            this.panelContent.BackColor = Color.White;
            this.panelContent.Dock = DockStyle.Fill;
            this.panelContent.Location = new Point(0, 80);
            this.panelContent.Padding = new Padding(30);
            this.panelContent.Size = new Size(500, 370);
            this.panelContent.Controls.Add(this.lblVersion);
            this.panelContent.Controls.Add(this.lblDescription);
            this.panelContent.Controls.Add(this.lblFeatures);
            this.panelContent.Controls.Add(this.lblCopyright);
            this.panelContent.Controls.Add(this.btnClose);

            // lblVersion
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            this.lblVersion.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblVersion.Location = new Point(30, 30);
            this.lblVersion.Text = Resources.Strings.About_Version;

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new Font("Microsoft YaHei UI", 9F);
            this.lblDescription.ForeColor = Color.FromArgb(127, 140, 141);
            this.lblDescription.Location = new Point(30, 70);
            this.lblDescription.Size = new Size(440, 60);
            this.lblDescription.Text = Resources.Strings.About_Description;

            // lblFeatures
            this.lblFeatures.AutoSize = true;
            this.lblFeatures.Font = new Font("Microsoft YaHei UI", 9F);
            this.lblFeatures.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblFeatures.Location = new Point(30, 150);
            this.lblFeatures.Size = new Size(440, 200);
            this.lblFeatures.Text = Resources.Strings.About_Features;

            // lblCopyright
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Font = new Font("Microsoft YaHei UI", 8F);
            this.lblCopyright.ForeColor = Color.FromArgb(149, 165, 166);
            this.lblCopyright.Location = new Point(30, 280);
            this.lblCopyright.Text = Resources.Strings.About_Copyright;

            // btnClose
            this.btnClose.BackColor = Color.FromArgb(41, 128, 185);
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(370, 310);
            this.btnClose.Size = new Size(100, 35);
            this.btnClose.Text = Resources.Strings.Button_Close;
            this.btnClose.Click += btnClose_Click;

            // AboutForm
            this.AutoScaleDimensions = new SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(500, 450);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = Resources.Strings.About_WindowTitle;
            
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);
        }

        private Panel panelTop;
        private Label lblTitle;
        private Panel panelContent;
        private Label lblVersion;
        private Label lblDescription;
        private Label lblFeatures;
        private Label lblCopyright;
        private Button btnClose;
    }
}

