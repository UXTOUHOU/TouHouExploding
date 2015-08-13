namespace TouHou_Exploding
{
    partial class tstForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.logTxtBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.commondTxtBox = new System.Windows.Forms.TextBox();
            this.commondBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.logTxtBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 461);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // logTxtBox
            // 
            this.logTxtBox.AcceptsTab = true;
            this.logTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTxtBox.Location = new System.Drawing.Point(3, 3);
            this.logTxtBox.Multiline = true;
            this.logTxtBox.Name = "logTxtBox";
            this.logTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTxtBox.Size = new System.Drawing.Size(378, 423);
            this.logTxtBox.TabIndex = 0;
            this.logTxtBox.TextChanged += new System.EventHandler(this.logTxtBox_TextChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.Controls.Add(this.commondTxtBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.commondBtn, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 432);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 26);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // commondTxtBox
            // 
            this.commondTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commondTxtBox.Location = new System.Drawing.Point(3, 3);
            this.commondTxtBox.Name = "commondTxtBox";
            this.commondTxtBox.Size = new System.Drawing.Size(312, 21);
            this.commondTxtBox.TabIndex = 0;
            // 
            // commondBtn
            // 
            this.commondBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commondBtn.Location = new System.Drawing.Point(321, 3);
            this.commondBtn.Name = "commondBtn";
            this.commondBtn.Size = new System.Drawing.Size(54, 20);
            this.commondBtn.TabIndex = 1;
            this.commondBtn.Text = "Send";
            this.commondBtn.UseVisualStyleBackColor = true;
            this.commondBtn.Click += new System.EventHandler(this.commondBtn_Click);
            // 
            // tstForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "tstForm";
            this.Text = "东方你妈瞬间爆炸测试系统-技术预览V1";
            this.Load += new System.EventHandler(this.tstForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox logTxtBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox commondTxtBox;
        private System.Windows.Forms.Button commondBtn;

    }
}

