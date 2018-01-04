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

        #region"��ʼ��"

      
        /// <summary>
        /// �ܳ��� ��ʼ��
        /// </summary>
        public static void init()
        {

            DataPath.CurrentPath = getFilePath(Directory.GetCurrentDirectory());////////��ȡ��ǰ·��
            //DataPath.ThisIp = getDbIpConfig();//////��ȡIP
            
           

        }

        public static void initPanel(Panel panelContent)
        {
            Control _newControl;
            String _controlName = "MvrpLite.adminFuc.panelRealData";

            //��ʼ�����
            _newControl = panelContent.Controls[_controlName];
            if (_newControl == null)
            {
                //���û�ҵ� ������֮.
                ObjectHandle _oh = AppDomain.CurrentDomain.CreateInstance(Assembly.GetExecutingAssembly().FullName, _controlName);
                _newControl = (Control)_oh.Unwrap();
                _newControl.Name = _controlName;
                _newControl.Dock = DockStyle.Fill;
                panelContent.Controls.Add(_newControl);
            } 
        }


       


        /// <summary>
        /// ��ñ�����ļ�·��
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

        #region"��ʼ����һ ���ݿ�IP  �� ��¼����"
        /// <summary>
        /// ���� ���ݿ� IP
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
