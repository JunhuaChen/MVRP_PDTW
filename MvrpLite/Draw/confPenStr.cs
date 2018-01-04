using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace MvrpLite.Draw
{
    /// <summary>
    /// 画笔集合
    /// </summary>
    public  struct PenStr
    {

        #region"结构体 数据与索引"
        private int m_Length;
        private string[] m_PenName;
        /// <summary>
        /// 方案号
        /// </summary>
        public static int penPlan;
        /// <summary>
        /// 外围线
        /// </summary>
        public static Pen outerPen;
        public Pen outerPen_var;
        /// <summary>
        /// 外框线
        /// </summary>
        public static Pen framePen;
        public Pen framePen_var;
             
        /// <summary>
        /// 小时线
        /// </summary>
        public static Pen hourPen;
        public Pen hourPen_var;
        /// <summary>
        /// 半小时线
        /// </summary>
        public static Pen thirtyMPen;
        public Pen thirtyMPen_var;
        /// <summary>
        /// 十分格线
        /// </summary>
        public static Pen tenMPen;
        public Pen tenMPen_var;
        /// <summary>
        /// 两分格线
        /// </summary>
        public static Pen twoMPen;
        public Pen twoMPen_var;
        /// <summary>
        /// A级列车线
        /// </summary>
        public static Pen rankAPen;
        public Pen rankAPen_var;
        /// <summary>
        /// B级列车线
        /// </summary>
        public static Pen rankBPen;
        public Pen rankBPen_var;
        /// <summary>
        /// C级列车线
        /// </summary>
        public static Pen rankCPen;
        public Pen rankCPen_var;
        /// <summary>
        /// D级列车级
        /// </summary>    
        public static Pen rankDPen;
        public Pen rankDPen_var;
        /// <summary>
        /// 轨道线
        /// </summary>
        public static Pen trackPen;
        /// <summary>
        /// 结构体自身索引
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Pen this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return outerPen_var;
                    case 1: return framePen_var;
                    case 2: return hourPen_var;
                    case 3: return thirtyMPen_var;
                    case 4: return tenMPen_var;
                    case 5: return twoMPen_var;
                    case 6: return rankAPen_var;
                    case 7: return rankBPen_var;
                    default: return null;
                }
            }
            set
            {
                switch (index)
                {
                    case 0: outerPen_var = value; break;
                    case 1: framePen_var = value; break;
                    case 2: hourPen_var = value; break;
                    case 3: thirtyMPen_var = value; break;
                    case 4: tenMPen_var = value; break;
                    case 5: twoMPen_var = value; break;
                    case 6: rankAPen_var = value; break;
                    case 7: rankBPen_var = value; break;
                    default: break;
                } 
            }
        }
        /// <summary>
        /// 数组长度
        /// </summary>
        public int Length
        {
            get
            {
                m_Length = 8;
                return m_Length;
            }
        }
        public string[] PenName
        {
            get
            {
                m_PenName = new string[8];
                m_PenName[0] = "外围线";
                m_PenName[1] = "外框线";
                m_PenName[2] = "小时线";
                m_PenName[3] = "半小时线";
                m_PenName[4] = "十分格线";
                m_PenName[5] = "两分格线";
                m_PenName[6] = "A级列车线";
                m_PenName[7] = "B级列车线";
                return m_PenName;
            }
        }
        #endregion"结构体 数据与索引"

        #region"配置"
        /// <summary>
        /// 笔刷一默认方案[研究型]
        /// </summary>
        public static void defaultPenConf()
        {
            //////////////画笔
            penPlan = 1;
            PenStr.outerPen = new Pen(Color.FromArgb(128,128,192),4f);
            PenStr.framePen = new Pen(Color.Black, 3);
            //PenStr.hourPen = new Pen(Color.Gray, 3);
            PenStr.hourPen = new Pen(Color.Green, 3);
            PenStr.tenMPen = new Pen(Color.FromArgb(225,225,225), 2f);
            PenStr.twoMPen = new Pen(Color.FromArgb(220,220,220), 1);
            PenStr.thirtyMPen = new Pen(Color.Gray, 2);
            PenStr.thirtyMPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            PenStr.trackPen = new Pen(Color.Gray, 1);
            PenStr.trackPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;            

            PenStr.rankAPen = new Pen(Color.Black, 2);
            PenStr.rankBPen = new Pen(Color.Blue, 2);
            PenStr.rankCPen = new Pen(Color.Gray, 1);
            PenStr.rankDPen = new Pen(Color.Gold, 1);


        }
        /// <summary>
        /// 方案二笔刷（工业型）
        /// </summary>
        public static void planOnePenConf()
        {
            //////////////画笔
            penPlan = 2;
            PenStr.outerPen = new Pen(Color.FromArgb(64,0,128), 4f);
            PenStr.framePen = new Pen(Color.Green, 3);
            PenStr.hourPen = new Pen(Color.Green, 3);
            PenStr.tenMPen = new Pen(Color.Green, 2f);
            PenStr.twoMPen = new Pen(Color.Gray, 1);
            //PenStr.twoMPen = new Pen(Color.Green, 1);
            PenStr.thirtyMPen = new Pen(Color.Green, 2);
            PenStr.thirtyMPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            PenStr.trackPen = new Pen(Color.Gray, 1);
            PenStr.trackPen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot; 

            PenStr.rankAPen = new Pen(Color.Black, 2);
            PenStr.rankBPen = new Pen(Color.Blue, 2);
            PenStr.rankCPen = new Pen(Color.Gray, 1);
            PenStr.rankDPen = new Pen(Color.Gold, 1); 
           
        }
        /// <summary>
        /// 重新配置笔刷
        /// </summary>
        public static void reconfPenStr(PenStr inputPenStr)
        {
            penPlan = 0;
            //////////////画笔
            if (inputPenStr.outerPen_var != null)
                PenStr.outerPen = inputPenStr.outerPen_var;
            if(inputPenStr.framePen_var!=null)
                PenStr.framePen =inputPenStr.framePen_var;
            if(inputPenStr.hourPen_var!=null)
                PenStr.hourPen = inputPenStr.hourPen_var;
            if(inputPenStr.tenMPen_var!=null)
                PenStr.tenMPen = inputPenStr.tenMPen_var;
            if(inputPenStr.twoMPen_var!=null)
                PenStr.twoMPen = inputPenStr.twoMPen_var;
            if(inputPenStr.thirtyMPen_var!=null)
                PenStr.thirtyMPen =inputPenStr.thirtyMPen_var;
            PenStr.thirtyMPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            
            if(inputPenStr.rankAPen_var!=null)
                PenStr.rankAPen =inputPenStr.rankAPen_var;
            if(inputPenStr.rankBPen_var!=null)
                PenStr.rankBPen =inputPenStr.rankBPen_var;
            if(inputPenStr.rankCPen_var!=null)
                PenStr.rankCPen =inputPenStr.rankCPen_var;
            if(inputPenStr.rankDPen_var!=null)
                PenStr.rankDPen =inputPenStr.rankDPen_var;
        }
        #endregion"结束 配置"
    }
}
