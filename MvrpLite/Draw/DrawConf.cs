using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MvrpLite.Draw
{
    /// <summary>
    /// ��ͼ����
    /// </summary>
     public class DrawConf
     {
         #region "����"
         public PenStr DrawPenStr
         {
             get
             {
                 throw new System.NotImplementedException();
             }
             set
             {
             }
         }

         public FontStr DrawFontStr
         {
             get
             {
                 throw new System.NotImplementedException();
             }
             set
             {
             }
         }

         public DefaultSizeStr DrawDefaultSizeStr
         {
             get
             {
                 throw new System.NotImplementedException();
             }
             set
             {
             }
         }
         public DisplayStr DisplayStr
         {
             get
             {
                 throw new System.NotImplementedException();
             }
             set
             {
             }
         }
        #endregion"���Խ���"
        
         /// <summary>
         /// ���û���
         /// </summary>
         public static void confPen()
         {
             PenStr.defaultPenConf();
         }
         public static void confPen(string penPlan)
         {
             if (penPlan == "�о��ͷ���")
                 PenStr.defaultPenConf();
             else if(penPlan=="��ҵ�ͷ���")
                 PenStr.planOnePenConf();
         }
         public static void reconfPen(PenStr inputPenStr)
         {
             PenStr.reconfPenStr(inputPenStr);
 
         }
         /// <summary>
         /// ��������
         /// </summary>
         public static void confFont()
         {
             
             FontStr.fontDefaultConf();
 
         }
         /// <summary>
         /// ���ó�ʼ�Ļ����С
         /// </summary>
         public static void confDefaultSize(int totalRunTime)
         {           
             DefaultSizeStr.defaultSizeConf(totalRunTime);                         
         }
         public static void reconfDefaultSize(int totalRunTime,int upHourWord, int rightStationWord, int downHourWord, int leftStationWord)
         {
             DefaultSizeStr.defaultSizeConf(totalRunTime,upHourWord, rightStationWord, downHourWord, leftStationWord);
         }
         /// <summary>
         /// �������û����С
         /// </summary>
         /// <param name="needChangeValue"></param>
         /// <param name="needChangeIndex"></param>
         public static void reconfDefaultSize(int totalRuntime,int needChangeValue, int needChangeIndex)
         {
             int upHourWord=DefaultSizeStr.upHourWordHeight;
             int rightStationWord=DefaultSizeStr.rightStationWordWidth;
             int downHourWord=DefaultSizeStr.downHourWordHeight;
             int leftStationWord=DefaultSizeStr.leftStationWordWidth;
             switch (needChangeIndex)
             {
                 case 1: upHourWord = needChangeValue; break;
                 case 2: rightStationWord = needChangeValue; break;
                 case 3: downHourWord = needChangeValue; break;
                 case 4: leftStationWord = needChangeValue; break;
                 default: break;
             }
             DefaultSizeStr.defaultSizeConf(totalRuntime,upHourWord, rightStationWord, downHourWord, leftStationWord);
         }
         /// <summary>
         /// ���û�����ʾ���������
         /// </summary>
         public static DisplayStr confDisplay()
         {
             DisplayStr displayStr = new DisplayStr();
             displayStr.confDefaultDisplay();
             return displayStr;
         }
         
    }
}
