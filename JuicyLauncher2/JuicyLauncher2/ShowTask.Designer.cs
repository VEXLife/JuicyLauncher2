namespace JuicyLauncher2
{
    partial class ShowTask
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowTask));
            this.tl = new System.Windows.Forms.ListBox();
            this.curPG = new System.Windows.Forms.ProgressBar();
            this.cpgPct = new System.Windows.Forms.Label();
            this.tpgPct = new System.Windows.Forms.Label();
            this.totPG = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // tl
            // 
            this.tl.FormattingEnabled = true;
            this.tl.ItemHeight = 12;
            this.tl.Location = new System.Drawing.Point(0, 1);
            this.tl.Name = "tl";
            this.tl.Size = new System.Drawing.Size(422, 304);
            this.tl.TabIndex = 0;
            // 
            // curPG
            // 
            this.curPG.Location = new System.Drawing.Point(13, 312);
            this.curPG.Name = "curPG";
            this.curPG.Size = new System.Drawing.Size(367, 23);
            this.curPG.TabIndex = 1;
            // 
            // cpgPct
            // 
            this.cpgPct.AutoSize = true;
            this.cpgPct.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cpgPct.Location = new System.Drawing.Point(386, 315);
            this.cpgPct.Name = "cpgPct";
            this.cpgPct.Size = new System.Drawing.Size(24, 16);
            this.cpgPct.TabIndex = 2;
            this.cpgPct.Text = "0%";
            // 
            // tpgPct
            // 
            this.tpgPct.AutoSize = true;
            this.tpgPct.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tpgPct.Location = new System.Drawing.Point(386, 344);
            this.tpgPct.Name = "tpgPct";
            this.tpgPct.Size = new System.Drawing.Size(24, 16);
            this.tpgPct.TabIndex = 4;
            this.tpgPct.Text = "0%";
            // 
            // totPG
            // 
            this.totPG.Location = new System.Drawing.Point(13, 341);
            this.totPG.Name = "totPG";
            this.totPG.Size = new System.Drawing.Size(367, 23);
            this.totPG.TabIndex = 3;
            // 
            // ShowTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 375);
            this.Controls.Add(this.tpgPct);
            this.Controls.Add(this.totPG);
            this.Controls.Add(this.cpgPct);
            this.Controls.Add(this.curPG);
            this.Controls.Add(this.tl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShowTask";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "正在执行任务，请稍候";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox tl;
        private System.Windows.Forms.ProgressBar curPG;
        private System.Windows.Forms.Label cpgPct;
        private System.Windows.Forms.Label tpgPct;
        private System.Windows.Forms.ProgressBar totPG;
    }
}