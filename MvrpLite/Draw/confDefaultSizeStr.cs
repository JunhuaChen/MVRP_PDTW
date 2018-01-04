using System;
using System.Collections.Generic;
using System.Text;

namespace MvrpLite.Draw
{
    public struct DefaultSizeStr
    {
        /// <summary>
        /// 画版宽
        /// </summary>
        public static int graphicWidth;       
        /// <summary>
        /// 画版高
        /// </summary>
        public static int graphicHeight;
        public static int graphicHeightOri;
        /// <summary>
        /// 一秒格间距宽
        /// </summary>
        public static float oneSStepWidth;
        /// <summary>
        /// 一分格间距宽
        /// </summary>
        public static int oneMStepWidth;
        /// <summary>
        /// 一公里间距高
        /// </summary>
        public static float oneKStepHeight;
        /// <summary>
        /// 标准的运行时分。为画纵轴服务
        /// </summary>
        public static int[] stdRunTime;
        /// <summary>
        /// 一个轨道占的高度
        /// </summary>
        public static int oneTrackEqualHeight;
        /// <summary>
        ///时刻表宽 
        /// </summary>
        public static int tableWidth;
        /// <summary>
        /// 左边距宽
        /// </summary>
        public static int leftSpaceWidth;
        /// <summary>
        /// 右边距宽
        /// </summary>
        public static int rightSpaceWidth;
        /// <summary>
        /// 左车站文字宽
        /// </summary>
        public static int leftStationWordWidth;
        /// <summary>
        /// 右车站文字宽
        /// </summary>
        public static int rightStationWordWidth;
        /// <summary>
        /// 运行图边框宽
        /// </summary>
        public static int tinySpaceWidth;
        /// <summary>
        /// 题头文字高
        /// </summary>
        public static int titleWordHeight;
        /// <summary>
        /// 上小时文字高
        /// </summary>
        public static int upHourWordHeight;
        /// <summary>
        /// 下小时文字高
        /// </summary>
        public static int downHourWordHeight;
        /// <summary>
        /// 运行图边框高
        /// </summary>
        public static int tinySpaceHeight;
        /// <summary>
        /// 时刻表上部高
        /// </summary>
        public static int topSpaceHeight;
        /// <summary>
        /// 时刻表下部高
        /// </summary>
        public static int bottomSpaceHeight;
        /// <summary>
        /// 时刻表高
        /// </summary>
        public static int tableHeight;
        /// <summary>
        /// 默认值设置
        /// </summary>
        public static void defaultSizeConf(int totalRuntime)
        {
            confRuntime();//配置标准纵横时分

            float stepHeight = 30f;//一标准时分占0.3个象素
            float stepWidth = 0.05f;//一秒占0.05象素
            DefaultSizeStr.oneKStepHeight = stepHeight;
            DefaultSizeStr.oneSStepWidth = stepWidth;
            DefaultSizeStr.oneMStepWidth = (int)(stepWidth * 60f);
            DefaultSizeStr.oneTrackEqualHeight = (int)(36*DefaultSizeStr.oneKStepHeight);

            DefaultSizeStr.leftStationWordWidth = 15 * (int)(60f * stepWidth);
            DefaultSizeStr.rightStationWordWidth = 15 * (int)(60f * stepWidth);
            DefaultSizeStr.tinySpaceWidth = 25;
            DefaultSizeStr.leftSpaceWidth = DefaultSizeStr.leftStationWordWidth + DefaultSizeStr.tinySpaceWidth;
            DefaultSizeStr.rightSpaceWidth = DefaultSizeStr.rightStationWordWidth+DefaultSizeStr.tinySpaceWidth;
            DefaultSizeStr.tableWidth = 1440 * (int)(60 * stepWidth);
            DefaultSizeStr.graphicWidth = DefaultSizeStr.leftSpaceWidth + DefaultSizeStr.rightSpaceWidth + DefaultSizeStr.tableWidth;

            DefaultSizeStr.titleWordHeight = 0;
            DefaultSizeStr.upHourWordHeight = 40;
            DefaultSizeStr.downHourWordHeight = 40;
            DefaultSizeStr.tinySpaceHeight = 80;
            DefaultSizeStr.tableHeight = (int)(totalRuntime * stepHeight);
            DefaultSizeStr.topSpaceHeight = DefaultSizeStr.titleWordHeight + DefaultSizeStr.upHourWordHeight + DefaultSizeStr.tinySpaceHeight;
            DefaultSizeStr.bottomSpaceHeight = DefaultSizeStr.downHourWordHeight + DefaultSizeStr.tinySpaceHeight;
            DefaultSizeStr.graphicHeightOri = DefaultSizeStr.topSpaceHeight + DefaultSizeStr.tableHeight + DefaultSizeStr.bottomSpaceHeight;
            DefaultSizeStr.graphicHeight = DefaultSizeStr.graphicHeightOri;
        }
        public static void defaultSizeConf(int totalRuntime,int upHourWord,int rightStationWord,int downHourWord,int leftStationWord)
        {
            confRuntime();//配置标准纵横时分

            float stepHeight = 30f;//一标准时分占0.3个象素
            float stepWidth = 0.05f;//一秒占0.05象素
            DefaultSizeStr.oneKStepHeight = stepHeight;
            DefaultSizeStr.oneSStepWidth = stepWidth;
            DefaultSizeStr.oneMStepWidth = (int)(stepWidth * 60f);
            DefaultSizeStr.oneTrackEqualHeight = (int)(36 * DefaultSizeStr.oneKStepHeight);

            DefaultSizeStr.leftStationWordWidth = leftStationWord;
            DefaultSizeStr.rightStationWordWidth = rightStationWord;
            DefaultSizeStr.tinySpaceWidth = 25;
            DefaultSizeStr.leftSpaceWidth = DefaultSizeStr.leftStationWordWidth + DefaultSizeStr.tinySpaceWidth;
            DefaultSizeStr.rightSpaceWidth = DefaultSizeStr.rightStationWordWidth + DefaultSizeStr.tinySpaceWidth;
            DefaultSizeStr.tableWidth = 1440 * (int)(60 * stepWidth);
            DefaultSizeStr.graphicWidth = DefaultSizeStr.leftSpaceWidth + DefaultSizeStr.rightSpaceWidth + DefaultSizeStr.tableWidth;

            DefaultSizeStr.titleWordHeight = 0;
            DefaultSizeStr.upHourWordHeight = upHourWord;
            DefaultSizeStr.downHourWordHeight = downHourWord;
            DefaultSizeStr.tinySpaceHeight = 80;
            DefaultSizeStr.tableHeight = (int)(totalRuntime* stepHeight);
            DefaultSizeStr.topSpaceHeight = DefaultSizeStr.titleWordHeight + DefaultSizeStr.upHourWordHeight + DefaultSizeStr.tinySpaceHeight;
            DefaultSizeStr.bottomSpaceHeight = DefaultSizeStr.downHourWordHeight + DefaultSizeStr.tinySpaceHeight;
            DefaultSizeStr.graphicHeightOri = DefaultSizeStr.topSpaceHeight + DefaultSizeStr.tableHeight + DefaultSizeStr.bottomSpaceHeight;
            DefaultSizeStr.graphicHeight = DefaultSizeStr.graphicHeightOri;
        }
        /// <summary>
        /// 配置标准纵横时分[私有]
        /// </summary>
        /// <returns></returns>
        public static void confRuntime()
        {
            //int stationCount = Network.SingleLineInfo.StaStr.Length;////车站数目
            //StandardTime.StdTimeSecStr stdTimeSec = new MvrpLite.StandardTime.StdTimeSecStr();///标尺

            //string[] sectionCODEs = new string[stationCount - 1];

           
            //DefaultSizeStr.stdRunTime = new int[stationCount];
            //int runTime = 0;
            //DefaultSizeStr.stdRunTime[0] = runTime;
            //for (int i = 1; i < stationCount; i++)
            //{
            //    stdTimeSec = StandardTime.StdTimeInfo.ChainStdSec_DOWN.rightData(sectionCODEs[i - 1], "CRH1");
            //    if (i == 1)
            //        runTime += stdTimeSec.TStdFT;
            //    else if (i == stationCount - 1)
            //        runTime += stdTimeSec.TStdTD;
            //    else
            //        runTime += stdTimeSec.TStdCN;
            //    DefaultSizeStr.stdRunTime[i] = runTime;

            //}

        }
    }    
}
