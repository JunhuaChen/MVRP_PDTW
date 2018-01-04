namespace dataTrans
{
    partial class FormMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnXml2CSV = new System.Windows.Forms.Button();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.txbInputFilePath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnTxt2CSV = new System.Windows.Forms.Button();
            this.btnBrowser2 = new System.Windows.Forms.Button();
            this.txbInputFilePath2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txbCustomerNum = new System.Windows.Forms.TextBox();
            this.txbVehicleNum = new System.Windows.Forms.TextBox();
            this.btnTxt2Txt = new System.Windows.Forms.Button();
            this.btnBrowser3 = new System.Windows.Forms.Button();
            this.txbInputFilePath3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "file path:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnXml2CSV);
            this.groupBox1.Controls.Add(this.btnBrowser);
            this.groupBox1.Controls.Add(this.txbInputFilePath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(593, 136);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "xml->csv";
            // 
            // btnXml2CSV
            // 
            this.btnXml2CSV.Location = new System.Drawing.Point(430, 75);
            this.btnXml2CSV.Name = "btnXml2CSV";
            this.btnXml2CSV.Size = new System.Drawing.Size(131, 45);
            this.btnXml2CSV.TabIndex = 3;
            this.btnXml2CSV.Text = "TRANS(&T)";
            this.btnXml2CSV.UseVisualStyleBackColor = true;
            this.btnXml2CSV.Click += new System.EventHandler(this.btnXml2CSV_Click);
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(430, 33);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(131, 23);
            this.btnBrowser.TabIndex = 2;
            this.btnBrowser.Text = "BROWSER(&B)...";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // txbInputFilePath
            // 
            this.txbInputFilePath.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txbInputFilePath.Location = new System.Drawing.Point(89, 34);
            this.txbInputFilePath.Multiline = true;
            this.txbInputFilePath.Name = "txbInputFilePath";
            this.txbInputFilePath.ReadOnly = true;
            this.txbInputFilePath.Size = new System.Drawing.Size(335, 86);
            this.txbInputFilePath.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnTxt2CSV);
            this.groupBox2.Controls.Add(this.btnBrowser2);
            this.groupBox2.Controls.Add(this.txbInputFilePath2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(593, 136);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "txt->csv";
            // 
            // btnTxt2CSV
            // 
            this.btnTxt2CSV.Location = new System.Drawing.Point(430, 75);
            this.btnTxt2CSV.Name = "btnTxt2CSV";
            this.btnTxt2CSV.Size = new System.Drawing.Size(131, 45);
            this.btnTxt2CSV.TabIndex = 3;
            this.btnTxt2CSV.Text = "TRANS(&T)";
            this.btnTxt2CSV.UseVisualStyleBackColor = true;
            this.btnTxt2CSV.Click += new System.EventHandler(this.btnTxt2CSV_Click);
            // 
            // btnBrowser2
            // 
            this.btnBrowser2.Location = new System.Drawing.Point(430, 33);
            this.btnBrowser2.Name = "btnBrowser2";
            this.btnBrowser2.Size = new System.Drawing.Size(131, 23);
            this.btnBrowser2.TabIndex = 2;
            this.btnBrowser2.Text = "BROWSER(&B)...";
            this.btnBrowser2.UseVisualStyleBackColor = true;
            this.btnBrowser2.Click += new System.EventHandler(this.btnBrowser2_Click);
            // 
            // txbInputFilePath2
            // 
            this.txbInputFilePath2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txbInputFilePath2.Location = new System.Drawing.Point(89, 34);
            this.txbInputFilePath2.Multiline = true;
            this.txbInputFilePath2.Name = "txbInputFilePath2";
            this.txbInputFilePath2.Size = new System.Drawing.Size(335, 86);
            this.txbInputFilePath2.TabIndex = 1;
            this.txbInputFilePath2.Text = "E:\\3 PrgStudy\\MrpPlus_dominate\\MvrpLite\\bin\\Debug\\PDPTW-BCP-DataSet1\\AA35";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "file path:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txbCustomerNum);
            this.groupBox3.Controls.Add(this.txbVehicleNum);
            this.groupBox3.Controls.Add(this.btnTxt2Txt);
            this.groupBox3.Controls.Add(this.btnBrowser3);
            this.groupBox3.Controls.Add(this.txbInputFilePath3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 272);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(593, 179);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "txt->txt";
            // 
            // txbCustomerNum
            // 
            this.txbCustomerNum.Location = new System.Drawing.Point(324, 134);
            this.txbCustomerNum.Name = "txbCustomerNum";
            this.txbCustomerNum.Size = new System.Drawing.Size(100, 23);
            this.txbCustomerNum.TabIndex = 4;
            // 
            // txbVehicleNum
            // 
            this.txbVehicleNum.Location = new System.Drawing.Point(89, 134);
            this.txbVehicleNum.Name = "txbVehicleNum";
            this.txbVehicleNum.Size = new System.Drawing.Size(100, 23);
            this.txbVehicleNum.TabIndex = 4;
            // 
            // btnTxt2Txt
            // 
            this.btnTxt2Txt.Location = new System.Drawing.Point(430, 94);
            this.btnTxt2Txt.Name = "btnTxt2Txt";
            this.btnTxt2Txt.Size = new System.Drawing.Size(131, 63);
            this.btnTxt2Txt.TabIndex = 3;
            this.btnTxt2Txt.Text = "TRANS(&T)";
            this.btnTxt2Txt.UseVisualStyleBackColor = true;
            this.btnTxt2Txt.Click += new System.EventHandler(this.btnTxt2Txt_Click);
            // 
            // btnBrowser3
            // 
            this.btnBrowser3.Location = new System.Drawing.Point(430, 38);
            this.btnBrowser3.Name = "btnBrowser3";
            this.btnBrowser3.Size = new System.Drawing.Size(131, 50);
            this.btnBrowser3.TabIndex = 2;
            this.btnBrowser3.Text = "BROWSER(&B)...";
            this.btnBrowser3.UseVisualStyleBackColor = true;
            this.btnBrowser3.Click += new System.EventHandler(this.btnBrowser3_Click);
            // 
            // txbInputFilePath3
            // 
            this.txbInputFilePath3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txbInputFilePath3.Location = new System.Drawing.Point(89, 34);
            this.txbInputFilePath3.Multiline = true;
            this.txbInputFilePath3.Name = "txbInputFilePath3";
            this.txbInputFilePath3.Size = new System.Drawing.Size(335, 86);
            this.txbInputFilePath3.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 14);
            this.label5.TabIndex = 0;
            this.label5.Text = "customer Num:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 28);
            this.label4.TabIndex = 0;
            this.label4.Text = "vehicle\r\n Num:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "file path:";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 494);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormMain";
            this.Text = "MainForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnXml2CSV;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.TextBox txbInputFilePath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnTxt2CSV;
        private System.Windows.Forms.Button btnBrowser2;
        private System.Windows.Forms.TextBox txbInputFilePath2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnTxt2Txt;
        private System.Windows.Forms.Button btnBrowser3;
        private System.Windows.Forms.TextBox txbInputFilePath3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbCustomerNum;
        private System.Windows.Forms.TextBox txbVehicleNum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}

