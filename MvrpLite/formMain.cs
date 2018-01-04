using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Reflection;
using System.Runtime.Remoting;
using MvrpLite.data;
namespace MvrpLite
{
    public partial class formMain : Form
    {
        #region"初始化"
        private Control _currentPanel;
        public bool isLoad = false;
        public formMain()
        {
            InitializeComponent();
            
            GlobalFuc.function.init();////总程序初始化

            this.treeViewMain.Visible = true; this.treeViewMain.Dock = DockStyle.Fill;
            this.treeViewMain.ExpandAll();

            
        }


        private void formMain_Load(object sender, EventArgs e)
        {


            this.Text = "M-VRP LITE 1.51";
            this.treeViewMain.Visible = true; this.treeViewMain.Dock = DockStyle.Fill; this.treeViewMain.ExpandAll();
               
            MvrpLite.adminFuc.panelRealData panelPre = new MvrpLite.adminFuc.panelRealData();
            panelPre.Tag = "MvrpLite.adminFuc.panelRealData";
            this.panelContent.Controls.Add(panelPre);
            panelPre.Dock = DockStyle.Fill;
            _currentPanel = panelPre;

                    
        }

        #endregion
        
        private void treeViewMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Control _newControl;
            //ToolStripButton _currentButton = (ToolStripButton)sender;
            if (e.Node.Tag == null || e.Node.Tag.ToString() =="")
                return;
            String _controlName = e.Node.Tag.ToString();

            // 首选 保证不是重复的工作 - 该面板是否已加载？
            if (!(_controlName == _currentPanel.Name))
            {
                //初始化面板
                _newControl = this.panelContent.Controls[_controlName];
                if (_newControl == null)
                {
                    //面板没找到 则生成之.
                    ObjectHandle _oh = AppDomain.CurrentDomain.CreateInstance(Assembly.GetExecutingAssembly().FullName, _controlName);
                    _newControl = (Control)_oh.Unwrap();
                    _newControl.Name = _controlName;
                    _newControl.Dock = DockStyle.Fill;

                                  
                    this.panelContent.Controls.Add(_newControl);
                }

                //替换旧的，加入新的 
                _currentPanel.Visible = false;
                _newControl.Visible = true;
                _currentPanel = _newControl;
            }

            if (_currentPanel is adminFuc.panelView)
            {
                int index = 0;
                if (e.Node.Name == "dataA") index = 0;
                else if (e.Node.Name == "dataB") index = 1;
                ((adminFuc.panelView)_currentPanel).setTabControlSelectedIndex(index);
            }

            if (_currentPanel is adminFuc.panelRealData)
            {
                int index = 0;
                if (e.Node.Name == "dataA") index = 0;
                else if (e.Node.Name == "dataB") index = 1;
               
                ((adminFuc.panelRealData)_currentPanel).setTabControlSelectedIndex(index);
            }
           
           

        }

        private void treeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        
    }
}