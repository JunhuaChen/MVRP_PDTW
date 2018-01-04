using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MvrpLite.Draw
{
    /*
     * �й�������������
     * */
    public  struct FontStr
    {
        #region"�ṹ�� ����������"
        private int m_Length;
        private string[] m_FontName;
        /// <summary>
        /// ������
        /// </summary>
        public static int fontPlan;
        /// <summary>
        /// ͼ����ͷ����
        /// </summary>
        public static FontInfoStr titleFont;
        public FontInfoStr titleFont_var;
        /// <summary>
        /// Сʱ����
        /// </summary>
        public static FontInfoStr timeHourFont;
        public FontInfoStr timeHourFont_var;
        /// <summary>
        /// ��վ����
        /// </summary>
        public static FontInfoStr stationNameFont;
        public FontInfoStr stationNameFont_var;
        /// <summary>
        /// ����һЩƽ��������
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
        /// ���鳤��
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
                m_FontName[0] = "����ͼ��Ŀ";
                m_FontName[1] = "ʱ��������";
                m_FontName[2] = "��վ������";
                m_FontName[3] = "��������";
                return m_FontName;
            }
        }
        #endregion"�ṹ�� ����������"

        #region"����"
        public static void fontDefaultConf()
        {
            FontStr.fontPlan = 1;

            FontFamily family = null;
            int style;
            int emSize;
            StringFormat format;
            SolidBrush brush;
            Pen pen;

            ///////////////////��ͨ��������ʽ
            family = new FontFamily("Times New Roman");
            style = (int)FontStyle.Bold;
            emSize = 12;
            format = StringFormat.GenericDefault;
            brush = new SolidBrush(Color.Black);
            pen = new Pen(brush, 1f);
            FontStr.normalFont = new Draw.FontInfoStr(family, style, emSize, format,brush,pen);
            ///////////////////��ͷ��������ʽ
            family = new FontFamily("����");
            style = (int)FontStyle.Bold;
            emSize = 22;
            format = StringFormat.GenericDefault;
            brush = new SolidBrush(Color.Chocolate);
            pen = new Pen(brush, 1f);
            FontStr.titleFont = new Draw.FontInfoStr(family, style, emSize, format, brush, pen);
            ///////////////////Сʱ���������ʽ
            family = new FontFamily("����");
            style = (int)FontStyle.Regular;
            emSize = 15;
            format = StringFormat.GenericDefault;
            brush = new SolidBrush(Color.Gray);
            pen = new Pen(brush, 1f);
            FontStr.timeHourFont = new Draw.FontInfoStr(family, style, emSize, format, brush, pen);
            ///////////////////��վ����������ʽ
            family = new FontFamily("Arial");
            style = (int)FontStyle.Regular;
            emSize = 10;
            format = StringFormat.GenericDefault;
            brush = new SolidBrush(Color.Black);
            pen = new Pen(brush, 1f);
            FontStr.stationNameFont = new Draw.FontInfoStr(family, style, emSize, format, brush, pen);
        }
        #endregion"���� ����"
    }
       

    /*
     * ����Ľṹ��Ϣ
     * 
     * */
    #region"����Ľṹ��Ϣ"
    public struct FontInfoStr
    {
        /// <summary>
        /// ���캯��
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
        /// �����ı��������������
        /// </summary>
        public FontFamily family;
        /// <summary>
        /// fontStyleö��
        /// </summary>
        public int style;
        /// <summary>
        /// �޶��ַ��� Em�������С������ĸ߶�
        /// </summary>
        public float emSize;
        /// <summary>
        /// ָ���ı���ʽ������Ϣ�����м��Ͷ��뷽ʽ����
        /// </summary>
        public StringFormat format;
        /// <summary>
        /// ��������ı�ˢ
        /// </summary>
        public SolidBrush brushFont;
        /// <summary>
        /// ��������ı�
        /// </summary>
        public Pen penFont;
    }
    #endregion"���� ����Ľṹ��Ϣ"
}
