using System;
using System.Collections.Generic;
using System.Text;

namespace MvrpLite.Draw
{
    public struct DisplayStr
    {
        /// <summary>
        /// ��ʾ�ľ��ȡ�2Ϊ��ʾ��2�ָ�10Ϊ��ʾ��10�ָ�30Ϊ��ʾ��30�ָ�
        /// </summary>
        public  int displayPrecision;
        /// <summary>
        /// ��ʾ�����С�"upAndDown"Ϊ��ʾ��+���У�"down"Ϊ��ʾ����,"up"Ϊ��ʾ����
        /// </summary>
        public  string displayDirection;
        /// <summary>
        /// ���ѡ���ģʽ��1Ϊ����ѡ��2Ϊȫ��ѡ��3Ϊȫ���з���ѡ
        /// </summary>
        public  int selectedMode;
        /// <summary>
        /// ѡ��ķ�����
        /// </summary>
        public  int selectedPlanID;
        /// <summary>
        /// ѡ����г���
        /// </summary>
        public  string selectedTrainID;
        /// <summary>
        /// ѡ���������
        /// </summary>
        public  string selectedSecCode;
        /// <summary>
        /// �Ƿ���ʾA������
        /// </summary>
        public  bool bRankA;
        /// <summary>
        /// �Ƿ���ʾB������
        /// </summary>
        public  bool bRankB;
        /// <summary>
        /// �Ƿ���ʾC������
        /// </summary>
        public  bool bRankC;
        /// <summary>
        /// �Ƿ���ʾD������
        /// </summary>
        public  bool bRankD;
        /// <summary>
        /// �Ƿ���ʾ�г�����
        /// </summary>
        public  bool bTrainWord;
        /// <summary>
        /// �Ƿ���ʾ�г�ͣվʱ�֡�
        /// </summary>
        public  bool bTrainStopTime;
        /// <summary>
        /// �������г���ʾģʽ[0Ϊ����ʾ��1Ϊ�ƶ����䳵��2Ϊ�ƶ�����㣬3Ϊȫ���ƶ�]
        /// </summary>
        public  int showAdjustTrain;
        /// <summary>
        /// �����г�X����
        /// </summary>
        public  int adjustTrainX;
        /// <summary>
        /// �����г�Y����
        /// </summary>
        public  int adjustTrainY;
        /// <summary>
        /// �� Ϊ����ͼģʽ  ��Ϊ������ģʽ
        /// </summary>
        public bool isTrainMode;
        /// <summary>
        /// ѡ��Ľ����г���
        /// </summary>
        public  string selectedRelayCode;
        /// <summary>
        /// normal,demo,simu
        /// </summary>
        public string displayMode;
        /// <summary>
        /// Ĭ������
        /// </summary>
        public void confDefaultDisplay()
        {
            this.displayPrecision = 2;
            this.displayDirection = "upAndDown";
            this.selectedMode = -1;
            this.selectedPlanID = -1;
            this.selectedSecCode ="null";
            this.selectedTrainID = "null";
            this.selectedRelayCode = "null";
            this.isTrainMode = true;
            this.bRankA = true;
            this.bRankB = true;
            this.bRankC = true;
            this.bRankD = true;
            this.bTrainWord = true;
            this.bTrainStopTime = true;
            this.displayMode = "normal";

            this.showAdjustTrain =0;
            this.adjustTrainX = -1;
            this.adjustTrainY = -1;
        }


    }
}
