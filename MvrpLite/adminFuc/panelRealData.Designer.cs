namespace MvrpLite.adminFuc
{
    partial class panelRealData
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControlData = new System.Windows.Forms.TabControl();
            this.tabPrepare = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvConfiguration = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvNode = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvAgent = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chk3 = new System.Windows.Forms.CheckBox();
            this.chk2 = new System.Windows.Forms.CheckBox();
            this.chk1 = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txbVNumber = new System.Windows.Forms.TextBox();
            this.txbPNum = new System.Windows.Forms.TextBox();
            this.txbNodeNumber = new System.Windows.Forms.TextBox();
            this.txbLinkNumber = new System.Windows.Forms.TextBox();
            this.tabSectionM = new System.Windows.Forms.TabPage();
            this.splitTD = new System.Windows.Forms.SplitContainer();
            this.gbUp = new System.Windows.Forms.GroupBox();
            this.z1 = new ZedGraph.ZedGraphControl();
            this.gbDown = new System.Windows.Forms.GroupBox();
            this.splitDownLR = new System.Windows.Forms.SplitContainer();
            this.dgvDecisionX = new System.Windows.Forms.DataGridView();
            this.dgvRoute = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControlData.SuspendLayout();
            this.tabPrepare.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNode)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgent)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.tabSectionM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTD)).BeginInit();
            this.splitTD.Panel1.SuspendLayout();
            this.splitTD.Panel2.SuspendLayout();
            this.splitTD.SuspendLayout();
            this.gbUp.SuspendLayout();
            this.gbDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitDownLR)).BeginInit();
            this.splitDownLR.Panel1.SuspendLayout();
            this.splitDownLR.Panel2.SuspendLayout();
            this.splitDownLR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDecisionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlData
            // 
            this.tabControlData.Controls.Add(this.tabPrepare);
            this.tabControlData.Controls.Add(this.tabSectionM);
            this.tabControlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlData.Location = new System.Drawing.Point(0, 0);
            this.tabControlData.Name = "tabControlData";
            this.tabControlData.SelectedIndex = 0;
            this.tabControlData.Size = new System.Drawing.Size(1182, 777);
            this.tabControlData.TabIndex = 3;
            // 
            // tabPrepare
            // 
            this.tabPrepare.Controls.Add(this.groupBox2);
            this.tabPrepare.Controls.Add(this.groupBox1);
            this.tabPrepare.Controls.Add(this.groupBox5);
            this.tabPrepare.Controls.Add(this.groupBox4);
            this.tabPrepare.Location = new System.Drawing.Point(4, 24);
            this.tabPrepare.Name = "tabPrepare";
            this.tabPrepare.Size = new System.Drawing.Size(1174, 749);
            this.tabPrepare.TabIndex = 3;
            this.tabPrepare.Text = "Data Input";
            this.tabPrepare.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvConfiguration);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 655);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1174, 94);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configuration Info";
            // 
            // dgvConfiguration
            // 
            this.dgvConfiguration.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConfiguration.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvConfiguration.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConfiguration.Location = new System.Drawing.Point(3, 19);
            this.dgvConfiguration.Name = "dgvConfiguration";
            this.dgvConfiguration.RowTemplate.Height = 25;
            this.dgvConfiguration.Size = new System.Drawing.Size(1168, 72);
            this.dgvConfiguration.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvNode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 402);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1174, 253);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Node";
            // 
            // dgvNode
            // 
            this.dgvNode.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNode.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvNode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNode.Location = new System.Drawing.Point(3, 19);
            this.dgvNode.Name = "dgvNode";
            this.dgvNode.RowTemplate.Height = 25;
            this.dgvNode.Size = new System.Drawing.Size(1168, 231);
            this.dgvNode.TabIndex = 7;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dgvAgent);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(0, 141);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1174, 261);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Input Agent";
            // 
            // dgvAgent
            // 
            this.dgvAgent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAgent.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvAgent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAgent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAgent.Location = new System.Drawing.Point(3, 19);
            this.dgvAgent.Name = "dgvAgent";
            this.dgvAgent.RowTemplate.Height = 25;
            this.dgvAgent.Size = new System.Drawing.Size(1168, 239);
            this.dgvAgent.TabIndex = 7;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chk3);
            this.groupBox4.Controls.Add(this.chk2);
            this.groupBox4.Controls.Add(this.chk1);
            this.groupBox4.Controls.Add(this.btnSave);
            this.groupBox4.Controls.Add(this.btnConfirm);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.txbVNumber);
            this.groupBox4.Controls.Add(this.txbPNum);
            this.groupBox4.Controls.Add(this.txbNodeNumber);
            this.groupBox4.Controls.Add(this.txbLinkNumber);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1174, 141);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            // 
            // chk3
            // 
            this.chk3.AutoSize = true;
            this.chk3.Checked = true;
            this.chk3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk3.Location = new System.Drawing.Point(710, 101);
            this.chk3.Name = "chk3";
            this.chk3.Size = new System.Drawing.Size(131, 18);
            this.chk3.TabIndex = 8;
            this.chk3.Text = "Vehicle connect";
            this.chk3.UseVisualStyleBackColor = true;
            // 
            // chk2
            // 
            this.chk2.AutoSize = true;
            this.chk2.Location = new System.Drawing.Point(563, 101);
            this.chk2.Name = "chk2";
            this.chk2.Size = new System.Drawing.Size(124, 18);
            this.chk2.TabIndex = 8;
            this.chk2.Text = "Call Mvrp-plus";
            this.chk2.UseVisualStyleBackColor = true;
            // 
            // chk1
            // 
            this.chk1.AutoSize = true;
            this.chk1.Location = new System.Drawing.Point(425, 101);
            this.chk1.Name = "chk1";
            this.chk1.Size = new System.Drawing.Size(103, 18);
            this.chk1.TabIndex = 8;
            this.chk1.Text = "Grouping_pv";
            this.chk1.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Location = new System.Drawing.Point(6, 75);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(172, 51);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save(&S)";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.SystemColors.Control;
            this.btnConfirm.Location = new System.Drawing.Point(964, 84);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(172, 51);
            this.btnConfirm.TabIndex = 7;
            this.btnConfirm.Text = "Optimization(&O)";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(489, 40);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(119, 14);
            this.label19.TabIndex = 3;
            this.label19.Text = "Input passenger:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(743, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input vehicle:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "Input Node:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(271, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(84, 14);
            this.label18.TabIndex = 3;
            this.label18.Text = "Input Link:";
            // 
            // txbVNumber
            // 
            this.txbVNumber.Enabled = false;
            this.txbVNumber.Location = new System.Drawing.Point(854, 39);
            this.txbVNumber.Name = "txbVNumber";
            this.txbVNumber.Size = new System.Drawing.Size(100, 23);
            this.txbVNumber.TabIndex = 5;
            this.txbVNumber.Text = "1";
            // 
            // txbPNum
            // 
            this.txbPNum.Enabled = false;
            this.txbPNum.Location = new System.Drawing.Point(614, 39);
            this.txbPNum.Name = "txbPNum";
            this.txbPNum.Size = new System.Drawing.Size(58, 23);
            this.txbPNum.TabIndex = 5;
            this.txbPNum.Text = "1";
            // 
            // txbNodeNumber
            // 
            this.txbNodeNumber.Enabled = false;
            this.txbNodeNumber.Location = new System.Drawing.Point(112, 35);
            this.txbNodeNumber.Name = "txbNodeNumber";
            this.txbNodeNumber.Size = new System.Drawing.Size(66, 23);
            this.txbNodeNumber.TabIndex = 5;
            this.txbNodeNumber.Text = "1";
            // 
            // txbLinkNumber
            // 
            this.txbLinkNumber.Enabled = false;
            this.txbLinkNumber.Location = new System.Drawing.Point(361, 38);
            this.txbLinkNumber.Name = "txbLinkNumber";
            this.txbLinkNumber.Size = new System.Drawing.Size(56, 23);
            this.txbLinkNumber.TabIndex = 5;
            this.txbLinkNumber.Text = "- -";
            // 
            // tabSectionM
            // 
            this.tabSectionM.Controls.Add(this.splitTD);
            this.tabSectionM.Location = new System.Drawing.Point(4, 24);
            this.tabSectionM.Name = "tabSectionM";
            this.tabSectionM.Size = new System.Drawing.Size(1174, 749);
            this.tabSectionM.TabIndex = 4;
            this.tabSectionM.Text = "Data Output";
            this.tabSectionM.UseVisualStyleBackColor = true;
            // 
            // splitTD
            // 
            this.splitTD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitTD.Location = new System.Drawing.Point(0, 0);
            this.splitTD.Name = "splitTD";
            this.splitTD.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitTD.Panel1
            // 
            this.splitTD.Panel1.Controls.Add(this.gbUp);
            // 
            // splitTD.Panel2
            // 
            this.splitTD.Panel2.Controls.Add(this.gbDown);
            this.splitTD.Size = new System.Drawing.Size(1174, 749);
            this.splitTD.SplitterDistance = 557;
            this.splitTD.TabIndex = 16;
            // 
            // gbUp
            // 
            this.gbUp.Controls.Add(this.z1);
            this.gbUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbUp.Location = new System.Drawing.Point(0, 0);
            this.gbUp.Name = "gbUp";
            this.gbUp.Size = new System.Drawing.Size(1174, 557);
            this.gbUp.TabIndex = 15;
            this.gbUp.TabStop = false;
            this.gbUp.Text = "Report and Result";
            // 
            // z1
            // 
            this.z1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.z1.Location = new System.Drawing.Point(3, 19);
            this.z1.Name = "z1";
            this.z1.ScrollGrace = 0D;
            this.z1.ScrollMaxX = 0D;
            this.z1.ScrollMaxY = 0D;
            this.z1.ScrollMaxY2 = 0D;
            this.z1.ScrollMinX = 0D;
            this.z1.ScrollMinY = 0D;
            this.z1.ScrollMinY2 = 0D;
            this.z1.Size = new System.Drawing.Size(1168, 535);
            this.z1.TabIndex = 2;
            // 
            // gbDown
            // 
            this.gbDown.Controls.Add(this.splitDownLR);
            this.gbDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDown.Location = new System.Drawing.Point(0, 0);
            this.gbDown.Name = "gbDown";
            this.gbDown.Size = new System.Drawing.Size(1174, 188);
            this.gbDown.TabIndex = 16;
            this.gbDown.TabStop = false;
            this.gbDown.Text = "Route";
            // 
            // splitDownLR
            // 
            this.splitDownLR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitDownLR.Location = new System.Drawing.Point(3, 19);
            this.splitDownLR.Name = "splitDownLR";
            // 
            // splitDownLR.Panel1
            // 
            this.splitDownLR.Panel1.Controls.Add(this.dgvDecisionX);
            // 
            // splitDownLR.Panel2
            // 
            this.splitDownLR.Panel2.Controls.Add(this.dgvRoute);
            this.splitDownLR.Size = new System.Drawing.Size(1168, 166);
            this.splitDownLR.SplitterDistance = 312;
            this.splitDownLR.TabIndex = 8;
            // 
            // dgvDecisionX
            // 
            this.dgvDecisionX.AllowUserToAddRows = false;
            this.dgvDecisionX.AllowUserToDeleteRows = false;
            this.dgvDecisionX.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDecisionX.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvDecisionX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDecisionX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDecisionX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDecisionX.Location = new System.Drawing.Point(0, 0);
            this.dgvDecisionX.MultiSelect = false;
            this.dgvDecisionX.Name = "dgvDecisionX";
            this.dgvDecisionX.ReadOnly = true;
            this.dgvDecisionX.RowTemplate.Height = 25;
            this.dgvDecisionX.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDecisionX.Size = new System.Drawing.Size(312, 166);
            this.dgvDecisionX.TabIndex = 9;          
            this.dgvDecisionX.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDecisionX_CellDoubleClick);
            // 
            // dgvRoute
            // 
            this.dgvRoute.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRoute.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvRoute.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRoute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoute.Location = new System.Drawing.Point(0, 0);
            this.dgvRoute.Name = "dgvRoute";
            this.dgvRoute.ReadOnly = true;
            this.dgvRoute.RowTemplate.Height = 25;
            this.dgvRoute.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoute.Size = new System.Drawing.Size(852, 166);
            this.dgvRoute.TabIndex = 7;
            // 
            // panelRealData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlData);
            this.Name = "panelRealData";
            this.Size = new System.Drawing.Size(1182, 777);
            this.Load += new System.EventHandler(this.panelRealData_Load);
            this.tabControlData.ResumeLayout(false);
            this.tabPrepare.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNode)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgent)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabSectionM.ResumeLayout(false);
            this.splitTD.Panel1.ResumeLayout(false);
            this.splitTD.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitTD)).EndInit();
            this.splitTD.ResumeLayout(false);
            this.gbUp.ResumeLayout(false);
            this.gbDown.ResumeLayout(false);
            this.splitDownLR.Panel1.ResumeLayout(false);
            this.splitDownLR.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitDownLR)).EndInit();
            this.splitDownLR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDecisionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoute)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlData;
        private System.Windows.Forms.TabPage tabPrepare;
        private System.Windows.Forms.TabPage tabSectionM;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvConfiguration;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvNode;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvAgent;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txbVNumber;
        private System.Windows.Forms.TextBox txbPNum;
        private System.Windows.Forms.TextBox txbNodeNumber;
        private System.Windows.Forms.TextBox txbLinkNumber;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.SplitContainer splitTD;
        private System.Windows.Forms.GroupBox gbUp;
        private System.Windows.Forms.GroupBox gbDown;
        private System.Windows.Forms.DataGridView dgvRoute;
        private System.Windows.Forms.SplitContainer splitDownLR;
        private System.Windows.Forms.DataGridView dgvDecisionX;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox chk3;
        private System.Windows.Forms.CheckBox chk2;
        private System.Windows.Forms.CheckBox chk1;
        private ZedGraph.ZedGraphControl z1;

    }
}
