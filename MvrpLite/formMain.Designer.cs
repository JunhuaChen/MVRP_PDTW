namespace MvrpLite
{
    partial class formMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("DataInput");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("DataOut");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Data_Prepare", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Timtable View");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Data Analyze");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Visual Result", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Main Function", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode6});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.toolStripHead = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusBottom = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitLR = new System.Windows.Forms.SplitContainer();
            this.treeViewMain = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panelContent = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FunctionFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripHead.SuspendLayout();
            this.statusBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLR)).BeginInit();
            this.splitLR.Panel1.SuspendLayout();
            this.splitLR.Panel2.SuspendLayout();
            this.splitLR.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripHead
            // 
            this.toolStripHead.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.toolStripHead.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1});
            this.toolStripHead.Location = new System.Drawing.Point(0, 28);
            this.toolStripHead.Name = "toolStripHead";
            this.toolStripHead.Size = new System.Drawing.Size(1006, 25);
            this.toolStripHead.TabIndex = 0;
            this.toolStripHead.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(72, 22);
            this.toolStripLabel1.Text = "Welcome";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // statusBottom
            // 
            this.statusBottom.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.statusBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusBottom.Location = new System.Drawing.Point(0, 488);
            this.statusBottom.Name = "statusBottom";
            this.statusBottom.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusBottom.Size = new System.Drawing.Size(1006, 25);
            this.statusBottom.TabIndex = 1;
            this.statusBottom.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(50, 20);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // splitLR
            // 
            this.splitLR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLR.Location = new System.Drawing.Point(0, 53);
            this.splitLR.Name = "splitLR";
            // 
            // splitLR.Panel1
            // 
            this.splitLR.Panel1.Controls.Add(this.treeViewMain);
            // 
            // splitLR.Panel2
            // 
            this.splitLR.Panel2.Controls.Add(this.panelContent);
            this.splitLR.Size = new System.Drawing.Size(1006, 435);
            this.splitLR.SplitterDistance = 158;
            this.splitLR.SplitterWidth = 5;
            this.splitLR.TabIndex = 2;
            // 
            // treeViewMain
            // 
            this.treeViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewMain.Font = new System.Drawing.Font("Calibri", 10.47273F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewMain.ImageIndex = 0;
            this.treeViewMain.ImageList = this.imageList1;
            this.treeViewMain.Location = new System.Drawing.Point(0, 0);
            this.treeViewMain.Name = "treeViewMain";
            treeNode1.Name = "dataA";
            treeNode1.Tag = "MvrpLite.adminFuc.panelRealData";
            treeNode1.Text = "DataInput";
            treeNode2.Name = "dataB";
            treeNode2.Tag = "MvrpLite.adminFuc.panelRealData";
            treeNode2.Text = "DataOut";
            treeNode3.Name = "dataA";
            treeNode3.Tag = "MvrpLite.adminFuc.panelRealData";
            treeNode3.Text = "Data_Prepare";
            treeNode4.Name = "dataA";
            treeNode4.Tag = "";
            treeNode4.Text = "Timtable View";
            treeNode5.Name = "dataB";
            treeNode5.Tag = "MvrpLite.adminFuc.panelView";
            treeNode5.Text = "Data Analyze";
            treeNode6.Name = "节点3";
            treeNode6.Tag = "MvrpLite.adminFuc.panelView";
            treeNode6.Text = "Visual Result";
            treeNode7.Name = "节点0";
            treeNode7.Text = "Main Function";
            this.treeViewMain.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
            this.treeViewMain.SelectedImageIndex = 0;
            this.treeViewMain.Size = new System.Drawing.Size(158, 435);
            this.treeViewMain.TabIndex = 0;
            this.treeViewMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMain_AfterSelect);
            this.treeViewMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewMain_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "参数.gif");
            this.imageList1.Images.SetKeyName(1, "路网.gif");
            this.imageList1.Images.SetKeyName(2, "站线作用.gif");
            this.imageList1.Images.SetKeyName(3, "标尺.gif");
            this.imageList1.Images.SetKeyName(4, "动车组.gif");
            this.imageList1.Images.SetKeyName(5, "天窗.gif");
            this.imageList1.Images.SetKeyName(6, "算法.gif");
            // 
            // panelContent
            // 
            this.panelContent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(843, 435);
            this.panelContent.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileFToolStripMenuItem,
            this.FunctionFToolStripMenuItem,
            this.ViewVToolStripMenuItem,
            this.HelpHToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1006, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileFToolStripMenuItem
            // 
            this.FileFToolStripMenuItem.Name = "FileFToolStripMenuItem";
            this.FileFToolStripMenuItem.Size = new System.Drawing.Size(61, 24);
            this.FileFToolStripMenuItem.Text = "File(&F)";
            // 
            // FunctionFToolStripMenuItem
            // 
            this.FunctionFToolStripMenuItem.Name = "FunctionFToolStripMenuItem";
            this.FunctionFToolStripMenuItem.Size = new System.Drawing.Size(88, 24);
            this.FunctionFToolStripMenuItem.Text = "Fuction(&F)";
            // 
            // ViewVToolStripMenuItem
            // 
            this.ViewVToolStripMenuItem.Name = "ViewVToolStripMenuItem";
            this.ViewVToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            this.ViewVToolStripMenuItem.Text = "View(&V)";
            // 
            // HelpHToolStripMenuItem
            // 
            this.HelpHToolStripMenuItem.Name = "HelpHToolStripMenuItem";
            this.HelpHToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.HelpHToolStripMenuItem.Text = "Help(&H)";
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 513);
            this.Controls.Add(this.splitLR);
            this.Controls.Add(this.statusBottom);
            this.Controls.Add(this.toolStripHead);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "formMain";
            this.Text = "M-VRP Lite";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.formMain_Load);
            this.toolStripHead.ResumeLayout(false);
            this.toolStripHead.PerformLayout();
            this.statusBottom.ResumeLayout(false);
            this.statusBottom.PerformLayout();
            this.splitLR.Panel1.ResumeLayout(false);
            this.splitLR.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLR)).EndInit();
            this.splitLR.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripHead;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.StatusStrip statusBottom;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitLR;
        private System.Windows.Forms.TreeView treeViewMain;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FunctionFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpHToolStripMenuItem;
    }
}

