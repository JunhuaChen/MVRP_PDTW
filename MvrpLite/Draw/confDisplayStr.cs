using System;
using System.Collections.Generic;
using System.Text;

namespace MvrpLite.Draw
{
    public struct DisplayStr
    {
        /// <summary>
        /// 显示的精度。2为显示到2分格，10为显示到10分格，30为显示到30分格
        /// </summary>
        public  int displayPrecision;
        /// <summary>
        /// 显示上下行。"upAndDown"为显示上+下行，"down"为显示下行,"up"为显示上行
        /// </summary>
        public  string displayDirection;
        /// <summary>
        /// 鼠标选择的模式。1为区间选，2为全车选，3为全开行方案选
        /// </summary>
        public  int selectedMode;
        /// <summary>
        /// 选择的方案号
        /// </summary>
        public  int selectedPlanID;
        /// <summary>
        /// 选择的列车号
        /// </summary>
        public  string selectedTrainID;
        /// <summary>
        /// 选择的区间码
        /// </summary>
        public  string selectedSecCode;
        /// <summary>
        /// 是否显示A级车。
        /// </summary>
        public  bool bRankA;
        /// <summary>
        /// 是否显示B级车。
        /// </summary>
        public  bool bRankB;
        /// <summary>
        /// 是否显示C级车。
        /// </summary>
        public  bool bRankC;
        /// <summary>
        /// 是否显示D级车。
        /// </summary>
        public  bool bRankD;
        /// <summary>
        /// 是否显示列车车次
        /// </summary>
        public  bool bTrainWord;
        /// <summary>
        /// 是否显示列车停站时分。
        /// </summary>
        public  bool bTrainStopTime;
        /// <summary>
        /// 调整的列车显示模式[0为不显示，1为移动区间车，2为移动区间点，3为全线移动]
        /// </summary>
        public  int showAdjustTrain;
        /// <summary>
        /// 调整列车X坐标
        /// </summary>
        public  int adjustTrainX;
        /// <summary>
        /// 调整列车Y坐标
        /// </summary>
        public  int adjustTrainY;
        /// <summary>
        /// 是 为运行图模式  否为动车组模式
        /// </summary>
        public bool isTrainMode;
        /// <summary>
        /// 选择的接续列车码
        /// </summary>
        public  string selectedRelayCode;
        /// <summary>
        /// normal,demo,simu
        /// </summary>
        public string displayMode;
        /// <summary>
        /// 默认设置
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
