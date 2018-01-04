using System;
using System.Collections.Generic;
using System.Text;

namespace MvrpLite.Draw
{
    public struct DefaultSizeStr
    {
        /// <summary>
        /// �����
        /// </summary>
        public static int graphicWidth;       
        /// <summary>
        /// �����
        /// </summary>
        public static int graphicHeight;
        public static int graphicHeightOri;
        /// <summary>
        /// һ������
        /// </summary>
        public static float oneSStepWidth;
        /// <summary>
        /// һ�ָ����
        /// </summary>
        public static int oneMStepWidth;
        /// <summary>
        /// һ�������
        /// </summary>
        public static float oneKStepHeight;
        /// <summary>
        /// ��׼������ʱ�֡�Ϊ���������
        /// </summary>
        public static int[] stdRunTime;
        /// <summary>
        /// һ�����ռ�ĸ߶�
        /// </summary>
        public static int oneTrackEqualHeight;
        /// <summary>
        ///ʱ�̱�� 
        /// </summary>
        public static int tableWidth;
        /// <summary>
        /// ��߾��
        /// </summary>
        public static int leftSpaceWidth;
        /// <summary>
        /// �ұ߾��
        /// </summary>
        public static int rightSpaceWidth;
        /// <summary>
        /// ��վ���ֿ�
        /// </summary>
        public static int leftStationWordWidth;
        /// <summary>
        /// �ҳ�վ���ֿ�
        /// </summary>
        public static int rightStationWordWidth;
        /// <summary>
        /// ����ͼ�߿��
        /// </summary>
        public static int tinySpaceWidth;
        /// <summary>
        /// ��ͷ���ָ�
        /// </summary>
        public static int titleWordHeight;
        /// <summary>
        /// ��Сʱ���ָ�
        /// </summary>
        public static int upHourWordHeight;
        /// <summary>
        /// ��Сʱ���ָ�
        /// </summary>
        public static int downHourWordHeight;
        /// <summary>
        /// ����ͼ�߿��
        /// </summary>
        public static int tinySpaceHeight;
        /// <summary>
        /// ʱ�̱��ϲ���
        /// </summary>
        public static int topSpaceHeight;
        /// <summary>
        /// ʱ�̱��²���
        /// </summary>
        public static int bottomSpaceHeight;
        /// <summary>
        /// ʱ�̱��
        /// </summary>
        public static int tableHeight;
        /// <summary>
        /// Ĭ��ֵ����
        /// </summary>
        public static void defaultSizeConf(int totalRuntime)
        {
            confRuntime();//���ñ�׼�ݺ�ʱ��

            float stepHeight = 30f;//һ��׼ʱ��ռ0.3������
            float stepWidth = 0.05f;//һ��ռ0.05����
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
            confRuntime();//���ñ�׼�ݺ�ʱ��

            float stepHeight = 30f;//һ��׼ʱ��ռ0.3������
            float stepWidth = 0.05f;//һ��ռ0.05����
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
        /// ���ñ�׼�ݺ�ʱ��[˽��]
        /// </summary>
        /// <returns></returns>
        public static void confRuntime()
        {
            //int stationCount = Network.SingleLineInfo.StaStr.Length;////��վ��Ŀ
            //StandardTime.StdTimeSecStr stdTimeSec = new MvrpLite.StandardTime.StdTimeSecStr();///���

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
