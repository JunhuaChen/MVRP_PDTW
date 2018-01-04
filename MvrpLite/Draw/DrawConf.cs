using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MvrpLite.Draw
{
    /// <summary>
    /// 绘图配置
    /// </summary>
     public class DrawConf
     {
         #region "属性"
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
        #endregion"属性结束"
        
         /// <summary>
         /// 配置画笔
         /// </summary>
         public static void confPen()
         {
             PenStr.defaultPenConf();
         }
         public static void confPen(string penPlan)
         {
             if (penPlan == "研究型方案")
                 PenStr.defaultPenConf();
             else if(penPlan=="工业型方案")
                 PenStr.planOnePenConf();
         }
         public static void reconfPen(PenStr inputPenStr)
         {
             PenStr.reconfPenStr(inputPenStr);
 
         }
         /// <summary>
         /// 配置字体
         /// </summary>
         public static void confFont()
         {
             
             FontStr.fontDefaultConf();
 
         }
         /// <summary>
         /// 配置初始的画版大小
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
         /// 重新配置画版大小
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
         /// 配置画版显示的相关内容
         /// </summary>
         public static DisplayStr confDisplay()
         {
             DisplayStr displayStr = new DisplayStr();
             displayStr.confDefaultDisplay();
             return displayStr;
         }
         
    }
}
