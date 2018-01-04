using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MvrpLite.Draw
{
    /*
     * 有关字体的相关配置
     * */
    public  struct FontStr
    {
        #region"结构体 数据与索引"
        private int m_Length;
        private string[] m_FontName;
        /// <summary>
        /// 方案号
        /// </summary>
        public static int fontPlan;
        /// <summary>
        /// 图形题头字体
        /// </summary>
        public static FontInfoStr titleFont;
        public FontInfoStr titleFont_var;
        /// <summary>
        /// 小时字体
        /// </summary>
        public static FontInfoStr timeHourFont;
        public FontInfoStr timeHourFont_var;
        /// <summary>
        /// 车站字体
        /// </summary>
        public static FontInfoStr stationNameFont;
        public FontInfoStr stationNameFont_var;
        /// <summary>
        /// 其它一些平常的字体
        /// </summary>
        public static FontInfoStr normalFont;
        public FontInfoStr normalFont_var;

        public FontInfoStr this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return titleFont_var ;
                    case 1: return timeHourFont_var;
                    case 2: return stationNameFont_var;
                    case 3: return normalFont_var;
                    default: return new FontInfoStr();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: titleFont_var = value; break;
                    case 1: timeHourFont_var = value; break;
                    case 2: stationNameFont_var = value; break;
                    case 3: normalFont_var = value; break;
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
                m_Length = 4;
                return m_Length;
            }
        }
        public string[] FontName
        {
            get
            {
                m_FontName = new string[4];
                m_FontName[0] = "运行图题目";
                m_FontName[1] = "时间轴数字";
                m_FontName[2] = "车站轴字体";
                m_FontName[3] = "其它字体";
                return m_FontName;
            }
        }
        #endregion"结构体 数据与索引"

        #region"配置"
        public static void fontDefaultConf()
        {
            FontStr.fontPlan = 1;

            FontFamily family = null;
            int style;
            int emSize;
            StringFormat format;
            SolidBrush brush;
            Pen pen;

            ///////////////////普通的字体样式
            family = new FontFamily("Times New Roman");
            style = (int)FontStyle.Bold;
            emSize = 12;
            format = StringFormat.GenericDefault;
            brush = new SolidBrush(Color.Black);
            pen = new Pen(brush, 1f);
            FontStr.normalFont = new Draw.FontInfoStr(family, style, emSize, format,brush,pen);
            ///////////////////题头的字体样式
            family = new FontFamily("黑体");
            style = (int)FontStyle.Bold;
            emSize = 22;
            format = StringFormat.GenericDefault;
            brush = new SolidBrush(Color.Chocolate);
            pen = new Pen(brush, 1f);
            FontStr.titleFont = new Draw.FontInfoStr(family, style, emSize, format, brush, pen);
            ///////////////////小时点的字体样式
            family = new FontFamily("黑体");
            style = (int)FontStyle.Regular;
            emSize = 15;
            format = StringFormat.GenericDefault;
            brush = new SolidBrush(Color.Gray);
            pen = new Pen(brush, 1f);
            FontStr.timeHourFont = new Draw.FontInfoStr(family, style, emSize, format, brush, pen);
            ///////////////////车站名的字体样式
            family = new FontFamily("Arial");
            style = (int)FontStyle.Regular;
            emSize = 10;
            format = StringFormat.GenericDefault;
            brush = new SolidBrush(Color.Black);
            pen = new Pen(brush, 1f);
            FontStr.stationNameFont = new Draw.FontInfoStr(family, style, emSize, format, brush, pen);
        }
        #endregion"结束 配置"
    }
       

    /*
     * 字体的结构信息
     * 
     * */
    #region"字体的结构信息"
    public struct FontInfoStr
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        
        public FontInfoStr(FontFamily iFamily, int iStyle, float iEmSize, StringFormat iFormat,SolidBrush iBrushFont,Pen iPenFont)
        {
            family = iFamily;
            style = iStyle;
            emSize = iEmSize;
            format = iFormat;
            brushFont = iBrushFont;
            penFont = iPenFont;
        }
        /// <summary>
        /// 绘制文本所用字体的名称
        /// </summary>
        public FontFamily family;
        /// <summary>
        /// fontStyle枚举
        /// </summary>
        public int style;
        /// <summary>
        /// 限定字符的 Em（字体大小）方框的高度
        /// </summary>
        public float emSize;
        /// <summary>
        /// 指定文本格式设置信息（如行间距和对齐方式）的
        /// </summary>
        public StringFormat format;
        /// <summary>
        /// 绘制字体的笔刷
        /// </summary>
        public SolidBrush brushFont;
        /// <summary>
        /// 绘制字体的笔
        /// </summary>
        public Pen penFont;
    }
    #endregion"结束 字体的结构信息"
}
