using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace MvrpLite.Draw
{


    /*                           Graphic  Title
     * [left]  0   1   2   3   4   5   6   7   8  ...         23   24    [right]   
     *         ------------------------------------------------------
     * staA    |-----------------------------------------------------|
     *         |               [table space]                         |   
     * staB    |---------------------------------------------------- |
     *         |                                                     |                
     *         |                                                     |       
     * staC    |---------------------------------------------------- |   
     *         |                                                     |   
     * staD    |-----------------------------------------------------|
     *         -------------------------------------------------------
     *         0   1  2   3                        ......... 23    24                      
     * **/
    /// <summary>
    /// ��������ͼ���
    /// </summary>
    public class DrawFrame
    {

        int[] m_all_nodes_name;
        int[] m_all_nodes_y;
        int[] m_set_s;

        private struct StationLocation
        {
            public string stationCode;
            public string stationName;
            public Point pStart;/////////��վ���
            public Point pEnd;///////////��վ�յ� 
        }
        private struct SectionLocation
        {
            public string sectionCode;
            public string sectionName;
            public Point pStart;
            public Point pEnd;
        }
        ArrayList staLocation;
        ArrayList secLocation;
        private Matrix matrix;
        private DisplayStr displayStr;
        /// <summary>
        /// ���캯��
        /// </summary>
        public DrawFrame(int[] all_nodes_name,int[] all_nodes_y,int[] set_s)
        {
            m_all_nodes_name = all_nodes_name;
            m_all_nodes_y = all_nodes_y;
            m_set_s = set_s;
        }
        private void initMatrix()
        {
            float offsetX = PathMatrix.offsetX;
            float offsetY = PathMatrix.offsetY;
            offsetX =offsetX-(float)DefaultSizeStr.leftSpaceWidth*(PathMatrix.widthScale-1f);
            offsetY =offsetY-(float)DefaultSizeStr.upHourWordHeight*(PathMatrix.heightScale-1f);
            
            staLocation = new ArrayList();
            secLocation = new ArrayList();

            matrix = new Matrix();
            matrix.Translate(offsetX, offsetY);///ƽ��
            matrix.Scale(PathMatrix.widthScale, PathMatrix.heightScale);///����   

            ///////////���û�ͼ���[Ϊ�������ռ�]
            //Network.StationStr[] station = Network.SingleLineInfo.StaStr;
            //DefaultSizeStr.graphicHeight=DefaultSizeStr.graphicHeightOri;
            //for (int i = 0; i < station.Length; i++)
            //{
            //    if (station[i].bTrackShow == true && displayStr.isTrainMode==true)
            //        DefaultSizeStr.graphicHeight +=  (station[i].tracksStr.Length + 1)* DefaultSizeStr.oneTrackEqualHeight;
            //}
        }
        /// <summary>
        /// ��������ͼ��ʱ������
        /// </summary>
        /// <param name="g"></param>
        public void drawTimeRuler(ref PictureBox picTime)
        {
          
            int picWidth = picTime.Width;
            int picHeight = picTime.Height;
            if (picWidth == 0 || picHeight == 0)
                return;
            Bitmap bitmap = new Bitmap(picWidth,picHeight);
            Graphics g = Graphics.FromImage(bitmap);

            int oriX = DefaultSizeStr.leftSpaceWidth + (int)PathMatrix.offsetX-(int)FontStr.timeHourFont.emSize/2;
            int oriY = 10;
            Point pStart = new Point(oriX, oriY);

            for (int i = 0; i <= 24; i++)
            {
                int startX = pStart.X + (int)(60f * (float)i * (float)DefaultSizeStr.oneMStepWidth*PathMatrix.widthScale);
                if (i > 10)
                    startX -= (int)FontStr.timeHourFont.emSize / 2;
                int startY = pStart.Y;
                if (startX > picWidth - DefaultSizeStr.rightSpaceWidth+10 || startX < DefaultSizeStr.leftSpaceWidth-10)
                    continue;                
                Point pLocation = new Point(startX, startY);
                Font drawFont = new Font(FontStr.timeHourFont.family, FontStr.timeHourFont.emSize, (FontStyle)FontStr.timeHourFont.style);
                SolidBrush drawBrush = FontStr.timeHourFont.brushFont;
                g.DrawString(i.ToString(), drawFont, drawBrush, pLocation);
            }
            picTime.Image = bitmap;
                    
        }
        /// <summary>
        /// ���Ƴ�վ����
        /// </summary>
        /// <param name="picStation"></param>
        public void drawStationWord(ref PictureBox picStation)
        {
           
            int picWidth = picStation.Width;
            int picHeight = picStation.Height;
            if (picWidth == 0 || picHeight == 0)
                return;
            Bitmap bitmap = new Bitmap(picWidth, picHeight);
            Graphics g = Graphics.FromImage(bitmap);

            Size size = new Size();
            size.Height = (int)FontStr.stationNameFont.emSize;
            size.Width = picWidth - 20;

            int oriX = 10;
            int oriY = (int)((float)DefaultSizeStr.tinySpaceHeight * PathMatrix.heightScale) + +(int)PathMatrix.offsetY - (int)FontStr.stationNameFont.emSize / 2;
            Point pStart = new Point(oriX, oriY-10);

            for (int i = 1; i <= m_all_nodes_y[0]; i++)
            {
                if (m_all_nodes_name[i] < 1000) continue;
                int startX = pStart.X;
                int startY = pStart.Y + (int)(m_all_nodes_y[i] * DefaultSizeStr.oneKStepHeight * PathMatrix.heightScale);
                Point pLocation = new Point(startX, startY);
                String stationName = m_all_nodes_name[i].ToString();

                Font drawFont = new Font(FontStr.stationNameFont.family, FontStr.stationNameFont.emSize, (FontStyle)FontStr.stationNameFont.style);
                SolidBrush drawBrush = FontStr.stationNameFont.brushFont;
                g.DrawString(stationName, drawFont, drawBrush, pLocation);
            }
             picStation.Image = bitmap;
            
        }
        /// <summary>
        /// ��������ͼ�Ŀ��
        /// </summary>
        /// <param name="panel"></param>
        public void drawTable(Graphics g,int panelWidth,int panelHeight,DisplayStr displayStr)
        {
            this.displayStr = displayStr;
            initMatrix();
            Pen pen = new Pen(Color.Indigo, 4f);
            pen=PenStr.outerPen;
            GraphicsPath gPathOuter = createOuter(panelWidth,panelHeight);//�������
            g.DrawPath(pen, gPathOuter);
                      
            pen = PenStr.hourPen;
            GraphicsPath gPathStationLine = createStationLine();//���ɳ�վ��
            g.DrawPath(pen,gPathStationLine);

            pen = PenStr.trackPen;
            GraphicsPath gPathDummyStation = createDummyStationLine();//�������⳵վ��
            g.DrawPath(pen, gPathDummyStation);

            pen = PenStr.hourPen;
            GraphicsPath gPathHourLine = createHourLine();//����Сʱ��
            g.DrawPath(pen, gPathHourLine);

            pen = PenStr.thirtyMPen;
            GraphicsPath gPathThirtyMLine = createThirtyMLine();//���ɰ�Сʱ��
            g.DrawPath(pen, gPathThirtyMLine);

            if(displayStr.displayPrecision==2||displayStr.displayPrecision==10)
            {
                pen = PenStr.tenMPen;
                GraphicsPath gPathTenMLine = createTenMLine();//����ʮ�ָ���
                g.DrawPath(pen, gPathTenMLine);
            }

            if (displayStr.displayPrecision == 2)
            {
                pen = PenStr.twoMPen;
                GraphicsPath gPathTwoMLine = createTwoMLine();//�������ָ���
                g.DrawPath(pen, gPathTwoMLine);
            }

         

        }

        #region"����ѡ"
        /// <summary>
        /// ��������ȡ��վID
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string getStationCode(int x, int y)
        {
            string stationCode ="null";
            float offsetX = PathMatrix.offsetX;
            float offsetY = PathMatrix.offsetY;
            offsetX = offsetX- (float)DefaultSizeStr.leftSpaceWidth * (PathMatrix.widthScale - 1f);
            offsetY = offsetY - (float)DefaultSizeStr.upHourWordHeight * (PathMatrix.heightScale - 1f);
            x = x - (int)offsetX;
            y = y - (int)offsetY;
            if (staLocation == null || staLocation.Count <= 0)
                return stationCode;
            
            int y1, y2, x1, x2;
            for (int i = 0; i < staLocation.Count; i++)
            {
                StationLocation oneIDLocation = (StationLocation)staLocation[i];
                x1 = oneIDLocation.pStart.X;
                x2 = oneIDLocation.pEnd.X;
                y1 = oneIDLocation.pStart.Y;
                y2 = oneIDLocation.pEnd.Y;
                if (x2 - x1 == 0)
                    continue;
                
                if (x>=x1 && x<=x2 && y>=y1-5 && y<=y1+5)
                {
                    stationCode = oneIDLocation.stationCode;
                    return stationCode;
                }
            }
            return stationCode;
        }
        /// <summary>
        /// ��������ȡ���δ���
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string getSectionCode(int x, int y)
        {
            string secCode = "null";
            float offsetX = PathMatrix.offsetX;
            float offsetY = PathMatrix.offsetY;
            offsetX = offsetX - (float)DefaultSizeStr.leftSpaceWidth * (PathMatrix.widthScale - 1f);
            offsetY = offsetY - (float)DefaultSizeStr.upHourWordHeight * (PathMatrix.heightScale - 1f);
            x = x - (int)offsetX;
            y = y - (int)offsetY;
            if (secLocation == null || secLocation.Count <= 0)
                return secCode;

            int y1, y2, x1, x2;
            for (int i = 0; i < secLocation.Count; i++)
            {
                SectionLocation oneIDLocation = (SectionLocation)secLocation[i];
                x1 = oneIDLocation.pStart.X;
                x2 = oneIDLocation.pEnd.X;
                y1 = oneIDLocation.pStart.Y;
                y2 = oneIDLocation.pEnd.Y;
                if (y2 - y1 == 0)
                    continue;

                if (y >= y1 && y <= y2 && x >= x1 - 5 && x <= x1 + 5)
                {
                    secCode = oneIDLocation.sectionCode;
                    return secCode;
                }
            }
            return secCode;
        }
        #endregion
        
        #region"·������"
        /// <summary>
        /// ������panel���·��[˽�к���]
        /// </summary>
        /// <param name="panelWidth"></param>
        /// <param name="panelHeight"></param>
        /// <returns></returns>
        private GraphicsPath createOuter(int panelWidth, int panelHeight)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            Point pStart = new Point(DefaultSizeStr.leftStationWordWidth, DefaultSizeStr.upHourWordHeight+30);
            panelWidth =panelWidth- DefaultSizeStr.leftStationWordWidth-DefaultSizeStr.rightStationWordWidth;
            panelHeight =panelHeight- DefaultSizeStr.upHourWordHeight-DefaultSizeStr.downHourWordHeight-30;
            Size size = new Size(panelWidth,panelHeight);
            Rectangle rec = new Rectangle(pStart, size);
            
            path.AddRectangle(rec);
            path.CloseFigure();
                        
            return path;
            
        }
       
      
        /// <summary>
        /// ����վ��[˽�к���]
        /// </summary>
        /// <returns></returns>
        private GraphicsPath createStationLine()
        {
            GraphicsPath path = new GraphicsPath();
           
            int startX1 = DefaultSizeStr.leftSpaceWidth;
            int startY1 = DefaultSizeStr.topSpaceHeight;
            int startX2 = DefaultSizeStr.graphicWidth - DefaultSizeStr.rightSpaceWidth;
            int startY2 = DefaultSizeStr.topSpaceHeight;

          
            for (int i = 1; i <= m_all_nodes_y[0]; i++)
            {
                Point pStart, pEnd;

                pStart = new Point(startX1, startY1 + (int)(m_all_nodes_y[i] * DefaultSizeStr.oneKStepHeight));
                pEnd = new Point(startX2, startY2 + (int)(m_all_nodes_y[i] * DefaultSizeStr.oneKStepHeight));
                      
                
                if (m_all_nodes_name[i] > 100 && m_all_nodes_name[i] < 1000) continue;
                path.StartFigure();
                path.AddLine(pStart, pEnd);
                //////////�����¼
                addToStaLocation(pStart, pEnd, m_all_nodes_name[i].ToString(), i.ToString());

            }

            path.Transform(matrix);
            return path; 
        }


        /// <summary>
        /// �����⳵վ��[˽�к���]
        /// </summary>
        /// <returns></returns>
        private GraphicsPath createDummyStationLine()
        {
            GraphicsPath path = new GraphicsPath();

            int startX1 = DefaultSizeStr.leftSpaceWidth;
            int startY1 = DefaultSizeStr.topSpaceHeight;
            int startX2 = DefaultSizeStr.graphicWidth - DefaultSizeStr.rightSpaceWidth;
            int startY2 = DefaultSizeStr.topSpaceHeight;

           
            for (int i = 1; i <= m_all_nodes_y[0]; i++)
            {
                
                Point pStart, pEnd;
                pStart = new Point(startX1, startY1 + (int)(m_all_nodes_y[i] * DefaultSizeStr.oneKStepHeight));
                pEnd = new Point(startX2, startY2 + (int)(m_all_nodes_y[i] * DefaultSizeStr.oneKStepHeight));
               
               
                if (m_all_nodes_name[i] < 100 || m_all_nodes_name[i] > 1000) continue;
                path.StartFigure();
                path.AddLine(pStart, pEnd);
                //////////�����¼
                addToStaLocation(pStart, pEnd, m_all_nodes_name[i].ToString(), i.ToString());
            }

            path.Transform(matrix);
            return path;
        }

              
        /// <summary>
        /// ��Сʱ��[˽�к���]
        /// </summary>
        /// <returns></returns>
        private GraphicsPath createHourLine()
        {
            GraphicsPath path = new GraphicsPath();
            int startX = DefaultSizeStr.leftSpaceWidth;
            int startY = DefaultSizeStr.topSpaceHeight;
            Point pStart, pEnd;

            /////////////0��
            pStart = new Point(startX, startY);
            pEnd = new Point(startX, DefaultSizeStr.graphicHeight-DefaultSizeStr.bottomSpaceHeight);
            path.StartFigure();
            path.AddLine(pStart, pEnd);

            /////////////24��
            pStart = new Point(startX + 60 * DefaultSizeStr.oneMStepWidth * 24, startY);
            pEnd = new Point(startX + 60 * DefaultSizeStr.oneMStepWidth * 24, DefaultSizeStr.graphicHeight - DefaultSizeStr.bottomSpaceHeight);
            path.StartFigure();
            path.AddLine(pStart, pEnd);
            
            ////////1�㵽23��
            for (int hour = 1; hour <= 23; hour++)
            {

                int x1 = startX + 60 * DefaultSizeStr.oneMStepWidth * hour;
                int x2 = x1;
                int y1 = startY;
                int y2 = startY;
                for(int s=1;s<m_set_s[0];s++)
                {
                    int nodeOrder = 0;
                    for (int i = 1; i <= m_all_nodes_name[0];i++ )
                        if (m_all_nodes_name[i] == m_set_s[s] + 1000)
                        {
                            nodeOrder = i; break;
                        }
                    y1 =startY+ (int)(m_all_nodes_y[nodeOrder]*DefaultSizeStr.oneKStepHeight);


                    for (int i = 1; i <= m_all_nodes_name[0]; i++)
                        if (m_all_nodes_name[i] == m_set_s[s+1])
                        {
                            nodeOrder = i; break;
                        }
                    y2 =startY+ (int)(m_all_nodes_y[nodeOrder] * DefaultSizeStr.oneKStepHeight);

                    pStart = new Point(x1, y1);
                    pEnd = new Point(x2, y2);
                    path.StartFigure();
                    path.AddLine(pStart, pEnd);
                                  
                }
               
            }
            path.Transform(matrix);
            return path;
        }

        /// <summary>
        /// ����Сʱ��[˽�к���]
        /// </summary>
        /// <returns></returns>
        private GraphicsPath createThirtyMLine()
        {
            GraphicsPath path = new GraphicsPath();
            int startX = DefaultSizeStr.leftSpaceWidth;
            int startY = DefaultSizeStr.topSpaceHeight;
            Point pStart, pEnd;
            for (int minute = 0; minute <= 1440; minute+=30)
            {
                if (minute % 60 == 0)
                    continue;

                int x1 = startX + DefaultSizeStr.oneMStepWidth * minute;
                int x2 = x1;
                int y1 = startY;
                int y2 = startY;
                for (int s = 1; s < m_set_s[0]; s++)
                {
                    int nodeOrder = 0;
                    for (int i = 1; i <= m_all_nodes_name[0]; i++)
                        if (m_all_nodes_name[i] == m_set_s[s] + 1000)
                        {
                            nodeOrder = i; break;
                        }
                    y1 = startY + (int)(m_all_nodes_y[nodeOrder] * DefaultSizeStr.oneKStepHeight);


                    for (int i = 1; i <= m_all_nodes_name[0]; i++)
                        if (m_all_nodes_name[i] == m_set_s[s + 1])
                        {
                            nodeOrder = i; break;
                        }
                    y2 = startY + (int)(m_all_nodes_y[nodeOrder] * DefaultSizeStr.oneKStepHeight);

                    pStart = new Point(x1, y1);
                    pEnd = new Point(x2, y2);
                    path.StartFigure();
                    path.AddLine(pStart, pEnd);

                }

            }
            path.Transform(matrix);
            return path;
        }
        /// <summary>
        /// ��ʮ�ָ���[˽�к���]
        /// </summary>
        /// <returns></returns>
        private GraphicsPath createTenMLine()
        {
            GraphicsPath path = new GraphicsPath();
            int startX = DefaultSizeStr.leftSpaceWidth;
            int startY = DefaultSizeStr.topSpaceHeight;
            Point pStart, pEnd;
            for (int minute = 0; minute <= 1440; minute += 10)
            {
                if (minute % 30 == 0)
                    continue;
                 int x1 = startX +  DefaultSizeStr.oneMStepWidth * minute;
                int x2 = x1;
                int y1 = startY;
                int y2 = startY;
                for (int s = 1; s < m_set_s[0]; s++)
                {
                    int nodeOrder = 0;
                    for (int i = 1; i <= m_all_nodes_name[0]; i++)
                        if (m_all_nodes_name[i] == m_set_s[s] + 1000)
                        {
                            nodeOrder = i; break;
                        }
                    y1 = startY + (int)(m_all_nodes_y[nodeOrder] * DefaultSizeStr.oneKStepHeight);


                    for (int i = 1; i <= m_all_nodes_name[0]; i++)
                        if (m_all_nodes_name[i] == m_set_s[s + 1])
                        {
                            nodeOrder = i; break;
                        }
                    y2 = startY + (int)(m_all_nodes_y[nodeOrder] * DefaultSizeStr.oneKStepHeight);

                    pStart = new Point(x1, y1);
                    pEnd = new Point(x2, y2);
                    path.StartFigure();
                    path.AddLine(pStart, pEnd);
                }
            }
            path.Transform(matrix);
            return path;
        }
        /// <summary>
        /// �����ָ���[˽�к���]
        /// </summary>
        /// <returns></returns>
        private GraphicsPath createTwoMLine()
        {       
            GraphicsPath path = new GraphicsPath();
            int startX = DefaultSizeStr.leftSpaceWidth;
            int startY = DefaultSizeStr.topSpaceHeight;
            Point pStart, pEnd;
            for (int minute = 0; minute <= 1440; minute += 2)
            {
                if (minute % 10 == 0)
                    continue;
                int x1 = startX + DefaultSizeStr.oneMStepWidth * minute;
                int x2 = x1;
                int y1 = startY;
                int y2 = startY;
                for (int s = 1; s < m_set_s[0]; s++)
                {
                    int nodeOrder = 0;
                    for (int i = 1; i <= m_all_nodes_name[0]; i++)
                        if (m_all_nodes_name[i] == m_set_s[s] + 1000)
                        {
                            nodeOrder = i; break;
                        }
                    y1 = startY + (int)(m_all_nodes_y[nodeOrder] * DefaultSizeStr.oneKStepHeight);


                    for (int i = 1; i <= m_all_nodes_name[0]; i++)
                        if (m_all_nodes_name[i] == m_set_s[s + 1])
                        {
                            nodeOrder = i; break;
                        }
                    y2 = startY + (int)(m_all_nodes_y[nodeOrder] * DefaultSizeStr.oneKStepHeight);

                    pStart = new Point(x1, y1);
                    pEnd = new Point(x2, y2);
                    path.StartFigure();
                    path.AddLine(pStart, pEnd);
                }
            }
            path.Transform(matrix);
            return path;
        }
        /// <summary>
        /// ���뵽��վ��¼��
        /// </summary>
        /// <param name="pStart"></param>
        /// <param name="pEnd"></param>
        private void addToStaLocation(Point pStart,Point pEnd,string stationCode,string stationName)
        {
            StationLocation stationLocation = new StationLocation();
            pStart.X = (int)((float)pStart.X * PathMatrix.widthScale);
            pStart.Y = (int)((float)pStart.Y * PathMatrix.heightScale);
            pEnd.X = (int)((float)pEnd.X * PathMatrix.widthScale);
            pEnd.Y = (int)((float)pEnd.Y * PathMatrix.heightScale);
            stationLocation.stationCode = stationCode;
            stationLocation.stationName = stationName;
            stationLocation.pStart = pStart;
            stationLocation.pEnd = pEnd;
            staLocation.Add(stationLocation);
        }
        ///// <summary>
        ///// ������ͷ·��[˽�к���]
        ///// </summary>
        ///// <returns></returns>
        //private GraphicsPath createTitlePath()
        //{
        //    GraphicsPath path = new GraphicsPath();
        //    string titleName = Network.NetworkStr.startStationName + " --> " + Network.NetworkStr.endStationName+ " �� �� ר �� �� �� ͼ ";
        //    Size size = new Size();
        //    size.Height =DefaultSizeStr.titleWordHeight;
        //    size.Width =DefaultSizeStr.tableWidth/4;
        //    Point pStart = new Point((DefaultSizeStr.graphicWidth - size.Width) / 2, 50);
        //    Rectangle rec = new Rectangle(pStart,size);            
        //    path.StartFigure();
        //    path.AddString(titleName, FontStr.titleFont.family, FontStr.titleFont.style, FontStr.titleFont.emSize, rec, FontStr.titleFont.format);

        //    path.Transform(matrix);
        //    return path;
        //}
        #endregion"���� ·������"

    

    }
}
