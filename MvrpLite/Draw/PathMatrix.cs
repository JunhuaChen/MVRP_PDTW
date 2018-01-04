using System;
using System.Collections.Generic;
using System.Text;

namespace MvrpLite.Draw
{
    class PathMatrix
    {
        public static float widthScale;///水平缩放比例
        public static float heightScale;///垂直缩放比例

        public static float offsetX;///左右偏量
        public static float offsetY;///上下偏量

        public static float widthScale_Net;///水平缩放比例
        public static float heightScale_Net;///垂直缩放比例
        public static float offsetX_Net;///左右偏量
        public static float offsetY_Net;///上下偏量
                                        ///
        public static float widthScale_Track;///水平缩放比例
        public static float heightScale_Track;///垂直缩放比例
        public static float offsetX_Track;///左右偏量
        public static float offsetY_Track;///上下偏量
        public PathMatrix()
        {
            widthScale =1f;
            heightScale =1f;

            offsetX = 0f;
            offsetY = 0f;

            widthScale_Net = 1f;
            heightScale_Net = 1f;
            offsetX_Net = 0f;
            offsetY_Net = 0f;

            widthScale_Track = 1f;
            heightScale_Track = 1f;
            offsetX_Track = 0f;
            offsetY_Track = 0f;

        }
        public void setWidthScale(float inputWidth)
        {
            widthScale = inputWidth; 
            if (inputWidth < 0.1f)
                widthScale = 0.1f;

        }
        public void setHeightScale(float inputHeight)
        {
            heightScale = inputHeight;
            if (inputHeight < 0.1f)
                heightScale = 0.1f;
        }
        public  void setWidthScale_Net(float inputWidth)
        {
            widthScale_Net = inputWidth;
            if (inputWidth < 0.1f)
                widthScale_Net = 0.1f;

        }
        public  void setHeightScale_Net(float inputHeight)
        {
            heightScale_Net = inputHeight;
            if (inputHeight < 0.1f)
                heightScale_Net = 0.1f;
        }

        public void setWidthScale_Track(float inputWidth)
        {
            widthScale_Track = inputWidth;
            if (inputWidth < 0.1f)
                widthScale_Track = 0.1f;

        }
        public void setHeightScale_Track(float inputHeight)
        {
            heightScale_Track = inputHeight;
            if (inputHeight < 0.1f)
                heightScale_Track = 0.1f;
        }


        public void setOffsetX(float inputX)
        {
            offsetX = inputX;
        }
        public void setOffsetY(float inputY)
        {
            offsetY = inputY;
        }
        public  void setOffsetX_Net(float inputX)
        {
            offsetX_Net = inputX;
        }
        public  void setOffsetY_Net(float inputY)
        {
            offsetY_Net = inputY;
        }

        public void setOffsetX_Track(float inputX)
        {
            offsetX_Track = inputX;
        }
        public void setOffsetY_Track(float inputY)
        {
            offsetY_Track = inputY;
        }
    }
}
