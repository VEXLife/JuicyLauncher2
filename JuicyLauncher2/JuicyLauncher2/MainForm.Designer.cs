namespace JuicyLauncher2
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Launch = new System.Windows.Forms.Button();
            this.VerSel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Nick = new System.Windows.Forms.TextBox();
            this.Stts = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tlbl = new System.Windows.Forms.PictureBox();
            this.cb = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // Launch
            // 
            this.Launch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Launch.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Launch.Location = new System.Drawing.Point(43, 368);
            this.Launch.Name = "Launch";
            this.Launch.Size = new System.Drawing.Size(364, 47);
            this.Launch.TabIndex = 0;
            this.Launch.Text = "启动";
            this.Launch.UseVisualStyleBackColor = true;
            this.Launch.Click += new System.EventHandler(this.Launch_Click);
            // 
            // VerSel
            // 
            this.VerSel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.VerSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VerSel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.VerSel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VerSel.FormattingEnabled = true;
            this.VerSel.Location = new System.Drawing.Point(58, 31);
            this.VerSel.Name = "VerSel";
            this.VerSel.Size = new System.Drawing.Size(160, 24);
            this.VerSel.Sorted = true;
            this.VerSel.TabIndex = 1;
            this.VerSel.SelectedIndexChanged += new System.EventHandler(this.VerSel_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "版本";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(224, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "名字";
            // 
            // Nick
            // 
            this.Nick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Nick.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Nick.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Nick.Location = new System.Drawing.Point(270, 31);
            this.Nick.Name = "Nick";
            this.Nick.Size = new System.Drawing.Size(137, 23);
            this.Nick.TabIndex = 4;
            this.Nick.Text = "Steve";
            this.Nick.TextChanged += new System.EventHandler(this.Nick_TextChanged);
            // 
            // Stts
            // 
            this.Stts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Stts.Location = new System.Drawing.Point(12, 368);
            this.Stts.Name = "Stts";
            this.Stts.Size = new System.Drawing.Size(25, 47);
            this.Stts.TabIndex = 5;
            this.Stts.Text = "设置";
            this.Stts.UseVisualStyleBackColor = true;
            this.Stts.Click += new System.EventHandler(this.Stts_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::JuicyLauncher2.Properties.Resources.timg;
            this.pictureBox1.Location = new System.Drawing.Point(-1, 61);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(420, 301);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // tlbl
            // 
            this.tlbl.BackColor = System.Drawing.Color.Transparent;
            this.tlbl.Location = new System.Drawing.Point(-1, 0);
            this.tlbl.Name = "tlbl";
            this.tlbl.Size = new System.Drawing.Size(420, 55);
            this.tlbl.TabIndex = 7;
            this.tlbl.TabStop = false;
            this.tlbl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fMouseDown);
            this.tlbl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fMouseMove);
            this.tlbl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fMouseUp);
            // 
            // cb
            // 
            this.cb.BackColor = System.Drawing.Color.Red;
            this.cb.FlatAppearance.BorderSize = 0;
            this.cb.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cb.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb.Location = new System.Drawing.Point(395, 0);
            this.cb.Name = "cb";
            this.cb.Size = new System.Drawing.Size(24, 25);
            this.cb.TabIndex = 8;
            this.cb.Text = "X";
            this.cb.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cb.UseVisualStyleBackColor = false;
            this.cb.Click += new System.EventHandler(this.cb_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(-1, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 25);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fMouseDown);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fMouseMove);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fMouseUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(143, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 21);
            this.label3.TabIndex = 10;
            this.label3.Text = "果汁启动器 第2版";
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fMouseDown);
            this.label3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.fMouseMove);
            this.label3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.fMouseUp);
            // 
            // MainForm
            // 
            this.AcceptButton = this.Launch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lime;
            this.ClientSize = new System.Drawing.Size(419, 426);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.cb);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Stts);
            this.Controls.Add(this.Nick);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VerSel);
            this.Controls.Add(this.Launch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tlbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "果汁启动器 第2版";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tlbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Launch;
        private System.Windows.Forms.ComboBox VerSel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Nick;
        private System.Windows.Forms.Button Stts;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox tlbl;
        private System.Windows.Forms.Button cb;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
    }
}

