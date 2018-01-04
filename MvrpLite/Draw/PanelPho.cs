using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MvrpLite.Draw
{
    public class PanelPho : Panel
    {
        public PanelPho()
            : base()
        {
            //  SetStyle(ControlStyles.UserPaint, true);
            //  SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景. 
            ////  SetStyle(
            //  SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲 

            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
    }
}
