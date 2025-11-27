namespace FileUpload
{
    partial class TestImageGeneratorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtCount = new TextBox();
            txtStartNumber = new TextBox();
            txtWidth = new TextBox();
            txtHeight = new TextBox();
            btnGenerate = new Button();
            btnCancel = new Button();
            btnCancelGeneration = new Button();
            panel1 = new Panel();
            label6 = new Label();
            lblWatchFolder = new Label();
            progressBar1 = new ProgressBar();
            lblProgress = new Label();
            label7 = new Label();
            txtMaxConcurrency = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(20, 60);
            label1.Name = "label1";
            label1.Size = new Size(84, 21);
            label1.TabIndex = 0;
            label1.Text = "生成数量:";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(20, 100);
            label2.Name = "label2";
            label2.Size = new Size(84, 21);
            label2.TabIndex = 1;
            label2.Text = "起始序号:";
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(20, 140);
            label3.Name = "label3";
            label3.Size = new Size(84, 21);
            label3.TabIndex = 2;
            label3.Text = "图片宽度:";
            //
            // label4
            //
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(220, 140);
            label4.Name = "label4";
            label4.Size = new Size(84, 21);
            label4.TabIndex = 3;
            label4.Text = "图片高度:";
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft YaHei", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = Color.FromArgb(100, 100, 100);
            label5.Location = new Point(20, 180);
            label5.Name = "label5";
            label5.Size = new Size(350, 30);
            label5.TabIndex = 4;
            label5.Text = "生成的测试图片将自动保存到监控目录\r\n文件命名格式: TEST_序号_时间戳_用于图片上传验证.jpg";
            //
            // txtCount
            //
            txtCount.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtCount.Location = new Point(110, 57);
            txtCount.Name = "txtCount";
            txtCount.PlaceholderText = "请输入生成数量";
            txtCount.Size = new Size(80, 29);
            txtCount.TabIndex = 5;
            txtCount.Text = "5";
            //
            // txtStartNumber
            //
            txtStartNumber.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtStartNumber.Location = new Point(110, 97);
            txtStartNumber.Name = "txtStartNumber";
            txtStartNumber.PlaceholderText = "起始序号";
            txtStartNumber.Size = new Size(80, 29);
            txtStartNumber.TabIndex = 6;
            txtStartNumber.Text = "1";
            //
            // txtWidth
            //
            txtWidth.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtWidth.Location = new Point(110, 137);
            txtWidth.Name = "txtWidth";
            txtWidth.PlaceholderText = "宽度";
            txtWidth.Size = new Size(80, 29);
            txtWidth.TabIndex = 7;
            txtWidth.Text = "800";
            //
            // txtHeight
            //
            txtHeight.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtHeight.Location = new Point(310, 137);
            txtHeight.Name = "txtHeight";
            txtHeight.PlaceholderText = "高度";
            txtHeight.Size = new Size(80, 29);
            txtHeight.TabIndex = 8;
            txtHeight.Text = "600";
            //
            // btnGenerate
            //
            btnGenerate.BackColor = Color.FromArgb(52, 152, 219);
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Font = new Font("Microsoft YaHei", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.Location = new Point(110, 250);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(100, 35);
            btnGenerate.TabIndex = 9;
            btnGenerate.Text = "生成";
            btnGenerate.UseVisualStyleBackColor = false;
            btnGenerate.Click += btnGenerate_Click;
            //
            // btnCancel
            //
            btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(230, 250);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 35);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "取消";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            //
            // btnCancelGeneration
            //
            btnCancelGeneration.BackColor = Color.FromArgb(192, 57, 43);
            btnCancelGeneration.Enabled = false;
            btnCancelGeneration.FlatAppearance.BorderSize = 0;
            btnCancelGeneration.FlatStyle = FlatStyle.Flat;
            btnCancelGeneration.Font = new Font("Microsoft YaHei", 12F, FontStyle.Bold, GraphicsUnit.Point);
            btnCancelGeneration.ForeColor = Color.White;
            btnCancelGeneration.Location = new Point(350, 250);
            btnCancelGeneration.Name = "btnCancelGeneration";
            btnCancelGeneration.Size = new Size(80, 35);
            btnCancelGeneration.TabIndex = 11;
            btnCancelGeneration.Text = "停止";
            btnCancelGeneration.UseVisualStyleBackColor = false;
            btnCancelGeneration.Click += btnCancelGeneration_Click;
            //
            // progressBar1
            //
            progressBar1.Location = new Point(110, 310);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(320, 23);
            progressBar1.TabIndex = 12;
            progressBar1.Visible = false;
            //
            // lblProgress
            //
            lblProgress.AutoSize = true;
            lblProgress.Font = new Font("Microsoft YaHei", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblProgress.ForeColor = Color.FromArgb(52, 152, 219);
            lblProgress.Location = new Point(110, 290);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(0, 19);
            lblProgress.TabIndex = 13;
            lblProgress.Visible = false;
            //
            // label7
            //
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft YaHei", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(20, 220);
            label7.Name = "label7";
            label7.Size = new Size(84, 21);
            label7.TabIndex = 14;
            label7.Text = "并发数:";
            //
            // txtMaxConcurrency
            //
            txtMaxConcurrency.Font = new Font("Microsoft YaHei", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txtMaxConcurrency.Location = new Point(110, 217);
            txtMaxConcurrency.Name = "txtMaxConcurrency";
            txtMaxConcurrency.PlaceholderText = "并发线程数";
            txtMaxConcurrency.Size = new Size(80, 29);
            txtMaxConcurrency.TabIndex = 15;
            txtMaxConcurrency.Text = Environment.ProcessorCount.ToString();
            //
            // panel1
            //
            panel1.BackColor = Color.FromArgb(52, 152, 219);
            panel1.Controls.Add(label6);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(450, 40);
            panel1.TabIndex = 16;
            //
            // label6
            //
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold, GraphicsUnit.Point);
            label6.ForeColor = Color.White;
            label6.Location = new Point(15, 10);
            label6.Name = "label6";
            label6.Size = new Size(128, 25);
            label6.TabIndex = 0;
            label6.Text = "测试图片生成器";
            //
            // lblWatchFolder
            //
            lblWatchFolder.AutoSize = true;
            lblWatchFolder.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblWatchFolder.ForeColor = Color.FromArgb(80, 80, 80);
            lblWatchFolder.Location = new Point(20, 155);
            lblWatchFolder.Name = "lblWatchFolder";
            lblWatchFolder.Size = new Size(350, 15);
            lblWatchFolder.TabIndex = 17;
            lblWatchFolder.Text = "监控目录: ";
            //
            // TestImageGeneratorForm
            //
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(450, 360);
            Controls.Add(lblProgress);
            Controls.Add(progressBar1);
            Controls.Add(txtMaxConcurrency);
            Controls.Add(label7);
            Controls.Add(btnCancelGeneration);
            Controls.Add(lblWatchFolder);
            Controls.Add(panel1);
            Controls.Add(btnCancel);
            Controls.Add(btnGenerate);
            Controls.Add(txtHeight);
            Controls.Add(txtWidth);
            Controls.Add(txtStartNumber);
            Controls.Add(txtCount);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TestImageGeneratorForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "测试图片生成器";
            TopMost = true;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtCount;
        private TextBox txtStartNumber;
        private TextBox txtWidth;
        private TextBox txtHeight;
        private Button btnGenerate;
        private Button btnCancel;
        private Button btnCancelGeneration;
        private Panel panel1;
        private Label label6;
        private Label lblWatchFolder;
        private ProgressBar progressBar1;
        private Label lblProgress;
        private Label label7;
        private TextBox txtMaxConcurrency;
    }
}
