using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Reflection;
using System.Runtime.Remoting;
using System.Windows.Forms;
using System.Management;
using System.Security.Cryptography;
using MvrpLite.data;
using System.Web.Security;

namespace MvrpLite.GlobalFuc
{
    public static class function
    {

        #region"初始化"

      
        /// <summary>
        /// 总程序 初始化
        /// </summary>
        public static void init()
        {

            DataPath.CurrentPath = getFilePath(Directory.GetCurrentDirectory());////////获取当前路径
            //DataPath.ThisIp = getDbIpConfig();//////获取IP
            
           

        }

        public static void initPanel(Panel panelContent)
        {
            Control _newControl;
            String _controlName = "MvrpLite.adminFuc.panelRealData";

            //初始化面板
            _newControl = panelContent.Controls[_controlName];
            if (_newControl == null)
            {
                //面板没找到 则生成之.
                ObjectHandle _oh = AppDomain.CurrentDomain.CreateInstance(Assembly.GetExecutingAssembly().FullName, _controlName);
                _newControl = (Control)_oh.Unwrap();
                _newControl.Name = _controlName;
                _newControl.Dock = DockStyle.Fill;
                panelContent.Controls.Add(_newControl);
            } 
        }


       


        /// <summary>
        /// 获得保存的文件路径
        /// </summary>
        /// <param name="fileAllPath"></param>
        /// <returns></returns>
        private static string getFilePath(string fileAllPath)
        {
            string filePath = string.Empty;

            string[] split = fileAllPath.Split(new Char[] { '\\' });

            for (int i = 0; i < split.Length; i++)
            {
                string s = split[i];
                filePath += s;
                filePath += @"\";
            }

            return filePath;
        }


       


        #endregion

        #region"初始化的一 数据库IP  和 登录密码"
        /// <summary>
        /// 设置 数据库 IP
        /// </summary>
        public static string getDbIpConfig()
        {
            string ipConfig = string.Empty;
            StreamReader din = File.OpenText(@"file\ipConfig.txt");
            String oneStr, strNow = string.Empty;
            int lineCount = 0;
            while ((oneStr = din.ReadLine()) != null)
            {
                if(lineCount==0) ipConfig = oneStr;
                lineCount++;
            }
            din.Close();
            return ipConfig;
        }

     
       
        #endregion

       
    }
}
