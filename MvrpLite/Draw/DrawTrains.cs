using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
//using MvrpLite.Train;

namespace MvrpLite.Draw
{
    /// <summary>
    /// ��������ͼ�е��г�
    /// </summary>
    public class DrawTrains
    {
        /// �洢���������г�ID
        private struct TrainIDLocation
        {
            public string trainID;////////�г�ID��
            public string sectionCode;//////������
            public Point pStart;/////////�г�����������
            public Point pEnd;///////////�г���������յ�
            public Point pStartOri;//////ԭʼ���
            public Point pEndOri;///////ԭʼĩ��
        }
        ArrayList idLocation;

        private Matrix matrix;
     
        GraphicsPath pathTrainLabel;/////0 ��ʾ�г��ı�ʶ
        GraphicsPath pathTrainInTrack;
        GraphicsPath pathRankA_down;
        GraphicsPath pathRankA_up;
        GraphicsPath pathRankB_down;
        GraphicsPath pathRankB_up;
        GraphicsPath pathRankC_down;
        GraphicsPath pathRankC_up;
        
        GraphicsPath pathSelectedPlan;
        GraphicsPath pathSelectedTrain;
        GraphicsPath pathSelectedSecTrain;

       
        public DrawTrains()
        {
            pathTrainLabel = new GraphicsPath();
            pathTrainInTrack = new GraphicsPath();
            pathRankA_down = new GraphicsPath();
            pathRankA_up = new GraphicsPath();
            pathRankB_down = new GraphicsPath();
            pathRankB_up = new GraphicsPath();
            pathRankC_down = new GraphicsPath();
            pathRankC_up = new GraphicsPath();
            pathSelectedPlan = new GraphicsPath();
            pathSelectedTrain = new GraphicsPath();
            pathSelectedSecTrain = new GraphicsPath();

            //pathTrainDemo = new GraphicsPath();
        }
      
        /// <summary>
        /// ���� �г������� �������֡������ʶ���г��ߡ�ѡ����г����䳵������
        /// </summary>
        public void execute(Graphics g,int[,] train_route,int[,] train_arrive_time,int[,]train_depart_time,int[] all_nodes_name,int[] all_nodes_y,
            int[,]con_start_node,int[,]con_end_node,int[,]con_start_time,int[,]con_end_time,int[,]con_start_u,int[,]con_end_u,
            DisplayStr displayStr)
        {
           
            idLocation = new ArrayList();
           
            //////////��ͼ��ʼ��
            initDraw();
            ////////////////////////////////////////////////////////////////////////////////

            Pen penRankA = PenStr.rankAPen;
            Pen penRankB = PenStr.rankBPen;
            Pen penRankC = PenStr.rankCPen;
            //idLocation = new ArrayList();
         
            GraphicsPath gPath=new GraphicsPath();
            ///////////////create train           
            for (int train_i = 1; train_i <= train_route[0, 0]; train_i++)
            {
                for(int n=1;n<=train_route[train_i,0];n++)
                {
                    int nodeName = train_route[train_i, n];
                    int arriveTime = train_arrive_time[train_i, n];
                    int deartTime = train_depart_time[train_i, n];

                    int space=0;
                    for (int k = 1; k <= all_nodes_name[0]; k++)
                        if (all_nodes_name[k] == nodeName)
                        {
                            space = all_nodes_y[k];
                            break;
                        }
                                        
                    Point pArrive = getPoint(space, arriveTime);
                    Point pDeparture = getPoint(space, deartTime);

                    if (nodeName < 100 || nodeName > 1000)
                    {
                        gPath.StartFigure();
                        gPath.AddLine(pArrive, pDeparture);
                    }
                    if (n == train_route[train_i, 0]) continue;
                    ////////next node
                    nodeName = train_route[train_i, n+1];
                    arriveTime = train_arrive_time[train_i, n+1];
                   
                    for (int k = 1; k <= all_nodes_name[0]; k++)
                        if (all_nodes_name[k] == nodeName)
                        {
                            space = all_nodes_y[k];
                            break;
                        }
                    pArrive = getPoint(space, arriveTime);
                    gPath.StartFigure();
                    gPath.AddLine(pDeparture, pArrive);


                }
            }
            gPath.Transform(matrix);
            g.DrawPath(penRankA, gPath);

            ///////////////////create locomotive connection line
            gPath = new GraphicsPath();
            Point p1 = new Point();
            Point p2 = new Point();
            for (int k = 1; k <= con_start_node[0, 0];k++ )
            {
                for(int n=1;n<=con_start_node[k,0];n++)
                {
                    int start_nodeName = con_start_node[k, n];
                    int start_space = 0;
                    for (int kk = 1; kk <= all_nodes_name[0]; kk++)
                        if (all_nodes_name[kk] == start_nodeName)
                        {
                            start_space = all_nodes_y[kk];
                            break;
                        }
                    int end_nodeName = con_end_node[k, n];
                    int end_space = 0;
                    for (int kk = 1; kk <= all_nodes_name[0]; kk++)
                        if (all_nodes_name[kk] == end_nodeName)
                        {
                            end_space = all_nodes_y[kk];
                            break;
                        }

                    int startTime = con_start_time[k, n];
                    int endTime = con_end_time[k, n];
                    ////////////////////////////////////////////////////////
                    if(start_nodeName>100 && start_nodeName<200)
                    {
                        startTime = 0; start_space = end_space;
                        p1 = getPoint(start_space, startTime);
                        p2 = getPoint(end_space, endTime);
                        gPath.StartFigure();
                        gPath.AddLine(p1, p2);
                    }
                    else if (end_nodeName > 200 && end_nodeName < 300)
                    {
                        endTime = 500; end_space = start_space;
                        p1 = getPoint(start_space, startTime);
                        p2 = getPoint(end_space, endTime);
                        gPath.StartFigure();
                        gPath.AddLine(p1, p2);
                    }
                    else
                    {
                        //p1 = getPoint(start_space, startTime);
                        //p2 = getPoint(end_space, endTime);
                        //gPath.StartFigure();
                        //gPath.AddLine(p1, p2);
                        //continue;

                        if(train_route[con_start_u[k, n],2]>1000)//////down direction
                        {
                            if (start_space< end_space)
                            {
                                p1 = getPoint(start_space, startTime);
                                p2 = getPoint(end_space, startTime);
                            }
                            else
                            {
                                p1 = getPoint(start_space, endTime);
                                p2 = getPoint(end_space, endTime);
                            }
                            gPath.StartFigure();
                            gPath.AddLine(p1, p2);

                            if (start_space < end_space)
                            {
                                p1 = getPoint(end_space, startTime);
                                p2 = getPoint(end_space, endTime);
                            }
                            else
                            {
                                p1 = getPoint(start_space, startTime);
                                p2 = getPoint(start_space, endTime);
                            }
                            gPath.StartFigure();
                            gPath.AddLine(p1, p2);
                        }
                        else
                        {
                            if (start_space >end_space)
                            {
                                p1 = getPoint(start_space, startTime);
                                p2 = getPoint(end_space, startTime);
                            }
                            else
                            {
                                p1 = getPoint(start_space, endTime);
                                p2 = getPoint(end_space, endTime);
                            }
                            gPath.StartFigure();
                            gPath.AddLine(p1, p2);

                            if (start_space > end_space)
                            {
                                p1 = getPoint(end_space, startTime);
                                p2 = getPoint(end_space, endTime);
                            }
                            else
                            {
                                p1 = getPoint(start_space, startTime);
                                p2 = getPoint(start_space, endTime);
                            }
                            gPath.StartFigure();
                            gPath.AddLine(p1, p2);
                        }
                      
                    }

                                        


                }

            }

            gPath.Transform(matrix);
            g.DrawPath(penRankB, gPath);
            ////////���Ƴ�������
            //if(displayStr.bTrainWord)
            //    createTrainWord(g, stationTrains);
                      

            #region"�����г�"
            //////��������A���г�            
            //if ((displayStr.displayDirection == "upAndDown" ||displayStr.displayDirection == "down") && displayStr.bRankA)
            //{
            //    try
            //    {
            //        pathRankA_down.Transform(matrix);
            //        g.DrawPath(penRankA, pathRankA_down);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(),"������Ϣ");
            //        return;                    
            //    }
            //}
            //////��������B���г�
            //if ((displayStr.displayDirection == "upAndDown" || displayStr.displayDirection == "down") && displayStr.bRankB)
            //{
            //    try
            //    {
            //        pathRankB_down.Transform(matrix);
            //        g.DrawPath(penRankB, pathRankB_down);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "������Ϣ");
            //        return;
            //    }
            //}
            //////��������C���г�
            //if ((displayStr.displayDirection== "upAndDown" || displayStr.displayDirection == "down") &&displayStr.bRankC)
            //{
            //    try
            //    {
            //        pathRankC_down.Transform(matrix);
            //        g.DrawPath(penRankC, pathRankC_down);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "������Ϣ");
            //        return;
            //    }
            //}
           
            //////��������A���г�
            //if ((displayStr.displayDirection == "upAndDown" || displayStr.displayDirection == "up") && displayStr.bRankA)
            //{
            //    try
            //    {
            //        pathRankA_up.Transform(matrix);
            //        g.DrawPath(penRankA, pathRankA_up);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "������Ϣ");
            //        return;
            //    }
            //}
            //////��������B���г�
            //if ((displayStr.displayDirection == "upAndDown" || displayStr.displayDirection == "up") &&displayStr.bRankB)
            //{
            //    try
            //    {
            //        pathRankB_up.Transform(matrix);
            //        g.DrawPath(penRankB, pathRankB_up);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "������Ϣ");
            //        return;
            //    }
            //}
            //////��������C���г�
            //if ((displayStr.displayDirection == "upAndDown" || displayStr.displayDirection == "up") &&displayStr.bRankC)
            //{
            //    try
            //    {
            //        pathRankC_up.Transform(matrix);
            //        g.DrawPath(penRankC, pathRankC_up);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "������Ϣ");
            //        return;
            //    }
            //}
            #endregion"���������г�"

            #region"����ѡ����г������䳵��������"

            //////////����ѡ���secTrain
            //if (displayStr.selectedSecCode != "null" &&displayStr.selectedMode==1)
            //{
            //    Pen penSlected = new Pen(Color.Red, 3f);
            //    try
            //    {
            //        if (pathSelectedSecTrain != null)
            //        {
            //            pathSelectedSecTrain.Transform(matrix);
            //            g.DrawPath(penSlected, pathSelectedSecTrain);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "������Ϣ");
            //        return;
            //    }
            //}

            ////////����ѡ���train
            //if (displayStr.selectedTrainID != "null" && displayStr.selectedMode==2)
            //{
            //    Pen penSlected = new Pen(Color.Red, 3f);
            //    try
            //    {
            //        if (pathSelectedTrain != null)
            //        {
            //            pathSelectedTrain.Transform(matrix);
            //            g.DrawPath(penSlected, pathSelectedTrain);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "������Ϣ");
            //        return;
            //    }
 
            //}           
            /////����ѡ��Ŀ��з���
            //if (displayStr.selectedPlanID!= -1)
            //{
            //    Pen penSlected = new Pen(Color.Red, 2f);
            //    try
            //    {
            //        if (pathSelectedPlan != null)
            //        {
            //            pathSelectedPlan.Transform(matrix);
            //            g.DrawPath(penSlected, pathSelectedPlan);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString(), "������Ϣ");
            //        return;
            //    }
            //}
            #endregion"���� ����ѡ����г������䳵��������"
        }
   

        #region"��������ʱ���г�"

         /// <summary>
        /// ���� ���䳵�ƶ���
        /// </summary>
        /// <param name="g"></param>
        /// <param name="displayStr"></param>
        public void drawMoveSecPoint(Graphics g, DisplayStr displayStr)
        {
            Point[] points = getTrainPoints(displayStr.selectedTrainID, displayStr.selectedSecCode);
            Point pStart = points[0];
            Point pEnd = points[1];
            Point pInput = pointToIDLocation_scale(displayStr.adjustTrainX, displayStr.adjustTrainY);/////�����idLocation��ƥ��ĵ�����
            int x = pInput.X;
            int y = pInput.Y;
            int datX=0;
            
            if (Math.Abs(pStart.Y - y) <= Math.Abs(pEnd.Y - y))///////ȡstart��
            {
                datX = x - pStart.X;
                datX = (int)((float)datX / PathMatrix.widthScale);///////////Ϊ��ʾ�õľ���
                pStart = points[2];////ԭʼ����
                pEnd = points[3];////ԭʼ����
                pStart.X += datX;
                //////////��ʱ���
                drawTrainTime(g, pStart);
            }
            else
            {
                datX = x - pEnd.X;
                datX = (int)((float)datX / PathMatrix.widthScale);///////////Ϊ��ʾ�õľ���
                pStart = points[2];////ԭʼ����
                pEnd = points[3];////ԭʼ����
                pEnd.X += datX;
                //////////��ʱ���
                drawTrainTime(g, pEnd);
            } 
            
            ////////////����
            GraphicsPath gPath = new GraphicsPath();
            gPath.AddLine(pStart, pEnd);
            gPath.Transform(matrix);

            Pen pen = new Pen(Color.Red, 2f);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawPath(pen, gPath);
        }

        /// <summary>
        /// ���� ���䳵�ƶ���
        /// </summary>
        /// <param name="g"></param>
        /// <param name="selectedTrainID"></param>
        /// <param name="pThrough">ͨ���ĵ�</param>
        //public void drawMoveSecTrain(Graphics g, DisplayStr displayStr)
        //{
        //    Point[] points = getTrainPoints(displayStr.selectedTrainID,displayStr.selectedSecCode);
        //    Point pStart =points[0];
        //    Point pEnd = points[1];
        //    Point pInput = pointToIDLocation_scale(displayStr.adjustTrainX, displayStr.adjustTrainY);/////�����idLocation��ƥ��ĵ�����
        //    int x = pInput.X;
        //    int y = pInput.Y;
        //    if (pEnd.Y == pStart.Y)
        //        return;
        //    int stdX = (y - pStart.Y) * (pEnd.X - pStart.X) / (pEnd.Y - pStart.Y) + pStart.X;
        //    int datX = x - stdX;
        //    datX = (int)((float)datX / PathMatrix.widthScale);///////////Ϊ��ʾ�õľ���


        //    ChainStationTrain thisTrain=null;
        //    for(int i=0;i<this.stationTrains.Count;i++)
        //    {
        //        if (((ChainStationTrain)stationTrains[i]).trainStr.trainID == displayStr.selectedTrainID)
        //            thisTrain = (ChainStationTrain)stationTrains[i];
        //    }
        //    thisTrain.moveFirst();
        //    while (true)
        //    {
        //        if (thisTrain.currentData().secTrainInfo.sectionCode == displayStr.selectedSecCode)
        //            break;
        //        thisTrain.moveNext();
        //    }
        //    ArrayList secCodeList = new ArrayList();
        //    while (true)
        //    {
        //        if (thisTrain.isEof())
        //            break;
        //        string oneSecCode = thisTrain.currentData().secTrainInfo.sectionCode;
        //        secCodeList.Add(oneSecCode);
        //        thisTrain.moveNext();
        //    }           
        //    Point[,] trainPoints = getTrainPoints(displayStr.selectedTrainID,secCodeList);
        //    GraphicsPath gPath = new GraphicsPath();
        //    for (int i = 0; i < trainPoints.Length / 4; i++)
        //    {               
        //        /////////ȷ��ԭʼ����
        //        pStart = trainPoints[i, 2];
        //        pEnd = trainPoints[i, 3];
        //        pStart.X += datX;
        //        pEnd.X += datX;
        //        ///////////��ʱ���
        //        drawTrainTime(g, pStart);
        //        drawTrainTime(g, pEnd);
        //        ////////////����
        //        gPath.StartFigure();
        //        gPath.AddLine(pStart, pEnd);

        //    }
        //    gPath.Transform(matrix);
        //    Pen pen = new Pen(Color.Red, 2f);
        //    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        //    g.DrawPath(pen, gPath);        


        
        //}
       
        /// <summary>
        /// ����ȫ���ƶ���
        /// </summary>
        /// <param name="g"></param>
        /// <param name="displayStr"></param>
        public void drawMoveStaTrain(Graphics g, DisplayStr displayStr)
        {

            Point[] pointSec = getTrainPoints(displayStr.selectedTrainID, displayStr.selectedSecCode);
            Point pStart = pointSec[0];
            Point pEnd = pointSec[1];
            Point pInput = pointToIDLocation_scale(displayStr.adjustTrainX, displayStr.adjustTrainY);/////�����idLocation��ƥ��ĵ�����
            int x = pInput.X;
            int y = pInput.Y;
            if (pEnd.Y == pStart.Y)
                return;
            int stdX = (y - pStart.Y) * (pEnd.X - pStart.X) / (pEnd.Y - pStart.Y) + pStart.X;
            int datX = x - stdX;
            datX = (int)((float)datX / PathMatrix.widthScale);///////////Ϊ��ʾ�õľ���

            Point[,] trainPoints = getTrainPoints(displayStr.selectedTrainID);
            GraphicsPath gPath = new GraphicsPath();
            for (int i = 0; i < trainPoints.Length/4;i++ )
            {
                //Point[] points = trainPoints[i];
                /////////ȷ��ԭʼ����
                pStart =trainPoints[i,2];
                pEnd =trainPoints[i,3];
                pStart.X += datX;
                pEnd.X += datX;
                ///////////��ʱ���
                drawTrainTime(g, pStart);
                drawTrainTime(g, pEnd);
                ////////////����
                gPath.StartFigure();    
                gPath.AddLine(pStart, pEnd);
                               
            }            
            gPath.Transform(matrix);
            Pen pen = new Pen(Color.Red, 2f);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawPath(pen, gPath);         
            
            
        }
        /// <summary>
        /// ����ʱ���[���к���]
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pOri"></param>
        private void drawTrainTime(Graphics g, Point pOri)
        {            
            float emSize = FontStr.normalFont.emSize;
            if (PathMatrix.widthScale >= 1f)
                emSize = FontStr.normalFont.emSize * PathMatrix.widthScale;
            Font drawFont = new Font(FontStr.normalFont.family, emSize, (FontStyle)FontStr.normalFont.style);
            SolidBrush drawBrush = FontStr.normalFont.brushFont;

            /////��ʱ���
            string strTime = getTime(pOri.X);
            Point pWordStart = pointToMatrix(pOri);
            g.DrawString(strTime, drawFont, drawBrush, pWordStart);            
 
        }
        #endregion"���� ��������ʱ���г�"

        #region"������ID��ID������� "
        /// <summary>
        /// ��������ȡ�г�ID
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public string[] getTrainAndSecCode(int x, int y)
        {        
           
            string[] strIDs = new string[2];
            Point pInput = pointToIDLocation_scale(x, y);/////�����idLocation��ƥ��ĵ�����
            x = pInput.X;
            y = pInput.Y;
            if (idLocation == null || idLocation.Count <= 0)
            {
                strIDs[0] = "null";
                strIDs[1] = "null";
                return strIDs;
            }
            int y1, y2, x1, x2, stdY;
            for (int i = 0; i < idLocation.Count; i++)
            {
                TrainIDLocation oneIDLocation = (TrainIDLocation)idLocation[i];
                x1 = oneIDLocation.pStart.X;
                x2 = oneIDLocation.pEnd.X;
                y1 = oneIDLocation.pStart.Y;
                y2 = oneIDLocation.pEnd.Y;
                if (x2 - x1 == 0)
                    continue;
                stdY = (x - x1) * (y2 - y1) / (x2 - x1) + y1;
                if (Math.Abs(y) <= Math.Abs(stdY + 15) && Math.Abs(y) >= Math.Abs(stdY - 15) && ((stdY >= y1 && stdY <= y2) || (stdY >= y2 && stdY <= y1)))
                {
                    strIDs[0] = oneIDLocation.trainID;
                    strIDs[1] = oneIDLocation.sectionCode;
                    return strIDs;
                }
            }
            strIDs[0] = "null";
            strIDs[1] = "null";
            return strIDs;
        }
        /// <summary>
        /// ���г�ID�ŵ��г��������
        /// </summary>
        /// <param name="selectedTrainID"></param>
        /// <returns></returns>
        public Point getTrainPoint(string selectedTrainID)
        {
            if (idLocation == null || idLocation.Count <= 0)
                return new Point(0, 0);
            for (int i = 0; i < idLocation.Count; i++)
            {
                TrainIDLocation thisIDLocation = (TrainIDLocation)idLocation[i];
                if (thisIDLocation.trainID == selectedTrainID)
                    return new Point(thisIDLocation.pStart.X, thisIDLocation.pStart.Y);
            }
            return new Point(0, 0);
        }
        /// <summary>
        /// ���г�ID�ŵ���ʼ���꣨����ԭʼ�ĺ����ŵģ�
        /// </summary>
        /// <param name="selectedTrainID"></param>
        /// <returns></returns>
        public Point[] getTrainPoints(string selectedTrainID,string selectedSecCode)
        {
            Point[] points = new Point[4];
            points[0] = new Point(0,0);
            points[1] = new Point(0,0);
            points[2] = new Point(0,0);
            points[3] = new Point(0,0);
            if (idLocation == null || idLocation.Count <= 0)
                return points;
            for (int i = 0; i < idLocation.Count; i++)
            {
                TrainIDLocation thisIDLocation = (TrainIDLocation)idLocation[i];
                if (thisIDLocation.trainID == selectedTrainID && thisIDLocation.sectionCode==selectedSecCode)
                {
                    points[0] = thisIDLocation.pStart;
                    points[1] = thisIDLocation.pEnd;
                    points[2] = thisIDLocation.pStartOri;
                    points[3] = thisIDLocation.pEndOri;
                    return points;
                }                   
            }
            return points;
        }
        public Point[,] getTrainPoints(string selectedTrainID)
        {
            if (idLocation == null || idLocation.Count <= 0)
                return new Point[1,1];
            int nCount = 0;
            string secCode = "";
            for (int i = 0; i < idLocation.Count; i++)
            {
                TrainIDLocation thisIDLocation = (TrainIDLocation)idLocation[i];
                if (thisIDLocation.trainID == selectedTrainID && thisIDLocation.sectionCode!=secCode)
                {
                    nCount++;
                    secCode = thisIDLocation.sectionCode; 
                }
            }
            Point[,] points = new Point[nCount,4];
            nCount = 0;
            secCode="";
            for (int i = 0; i < idLocation.Count; i++)
            {
                TrainIDLocation thisIDLocation = (TrainIDLocation)idLocation[i];
                if (thisIDLocation.trainID == selectedTrainID && thisIDLocation.sectionCode!=secCode)
                {
                    points[nCount,0] =thisIDLocation.pStart;
                    points[nCount,1] =thisIDLocation.pEnd;
                    points[nCount,2] =thisIDLocation.pStartOri;
                    points[nCount,3] =thisIDLocation.pEndOri;
                    nCount++;
                    secCode = thisIDLocation.sectionCode; 
                }
            }
            return points;

        }
        /// <summary>
        /// ���г�ID�ź���ʼ���εõ� ���������µ��г�����
        /// </summary>
        /// <param name="selectedTrainID"></param>
        /// <returns></returns>
        public Point[,] getTrainPoints(string selectedTrainID, ArrayList secCodeList)
        {
            Point[,] points=new Point[secCodeList.Count, 4];
            if (idLocation == null || idLocation.Count <= 0)
                return points;
            for (int n = 0; n < secCodeList.Count; n++)
            {
                string thisSecCode = (string)secCodeList[n];
                for (int i = 0; i < idLocation.Count; i++)
                {
                    TrainIDLocation thisIDLocation = (TrainIDLocation)idLocation[i];
                    if (thisIDLocation.trainID == selectedTrainID && thisIDLocation.sectionCode == thisSecCode)
                    {
                        points[n,0] = thisIDLocation.pStart;
                        points[n,1] = thisIDLocation.pEnd;
                        points[n,2] = thisIDLocation.pStartOri;
                        points[n,3] = thisIDLocation.pEndOri;                        
                    }
                }
            }
            return points;
        }
        #endregion"���� ������ID��ID�������"

        #region"��ͼ˽��"
        /// <summary>
        /// ����graphicPath �� matrix
        /// </summary>
        private void initDraw()
        {
            pathTrainLabel.Reset();
            pathTrainInTrack.Reset();
            pathRankA_down.Reset();
            pathRankA_up.Reset();
            pathRankB_down.Reset();
            pathRankB_up.Reset();
            pathRankC_down.Reset();
            pathRankC_up.Reset();
            pathSelectedPlan.Reset();
            pathSelectedSecTrain.Reset();
            pathSelectedTrain.Reset();
            //pathTrainDemo.Reset();

            float offsetX = PathMatrix.offsetX;
            float offsetY = PathMatrix.offsetY;
            offsetX = offsetX - (float)DefaultSizeStr.leftSpaceWidth * (PathMatrix.widthScale - 1f);
            offsetY = offsetY - (float)DefaultSizeStr.upHourWordHeight * (PathMatrix.heightScale - 1f);
            matrix = new Matrix();
            matrix.Translate(offsetX, offsetY);///ƽ��
            matrix.Scale(PathMatrix.widthScale, PathMatrix.heightScale);///����        
        }
        /// <summary>
        /// �����г��������� 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="stationTrains"></param>
        private void createTrainWord(Graphics g,ArrayList stationTrains)
        {
            //Point pOri;
            //int deadLine = getPoint(24 * 3600).X;
            //int startX = 0;
            //int startY = 0;
            //TrainUnit trainUnit;
            //StaTrainInfoStr staTrain;
            //ChainStationTrain oneTrain;
            
            //for (int i = 0; i < stationTrains.Count; i++)
            //{
            //    oneTrain = (ChainStationTrain)stationTrains[i];
            //    oneTrain.moveFirst();
            //    trainUnit = (TrainUnit)oneTrain.currentData();
            //    staTrain = trainUnit.staTrainInfo;
            //    pOri = getStartPoint(staTrain);////���
            //    if (pOri.X > deadLine)
            //        pOri.X = pOri.X - deadLine + DefaultSizeStr.leftSpaceWidth;

            //    ////////////////ȷ��д�ֵ��������
            //    float myOffsetX = (float)pOri.X * (PathMatrix.widthScale - 1f);//////�õ������Ŷ�����������ֵ
            //    float myOffsetY = (float)pOri.Y * (PathMatrix.heightScale - 1f);
            //    if (oneTrain.trainStr.trainDirection == "down")
            //    {
            //        startX = (int)((float)pOri.X + matrix.OffsetX + myOffsetX - 8f);
            //        startY = (int)((float)pOri.Y +matrix.OffsetY + myOffsetY - 12f * PathMatrix.heightScale - 40f);
            //    }
            //    else
            //    {
            //        startX = (int)((float)pOri.X +matrix.OffsetX + myOffsetX - 8f);
            //        startY = (int)((float)pOri.Y +matrix.OffsetY + myOffsetY + 12f * PathMatrix.heightScale);
            //    }
            //    Point pLocation = new Point(startX, startY);
                
            //    //////////////////////////////////////////����ͼƬ��
            //    if (PathMatrix.widthScale <= 0.5)
            //    {
            //        FontStr.normalFont.emSize = 8;
            //        FontStr.normalFont.style = (int)FontStyle.Regular;
            //    }
            //    else
            //    {
            //        FontStr.normalFont.emSize = 12;
            //        FontStr.normalFont.style = (int)FontStyle.Bold;
            //    }                
            //    Font drawFont = new Font(FontStr.normalFont.family, FontStr.normalFont.emSize, (FontStyle)FontStr.normalFont.style);
            //    SolidBrush drawBrush = FontStr.normalFont.brushFont;

            //    Image image = new Bitmap(40, (int)FontStr.normalFont.emSize*2);
            //    Graphics gImage = Graphics.FromImage(image);
            //    gImage.DrawString(oneTrain.trainStr.trainID, drawFont, drawBrush, new Point(0, 0));
            //    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                               
            //    ///////////////////////////////////////��ͼƬ����
            //    g.DrawImage(image, pLocation);
            //    gImage.Dispose();
            //}
           
                      
        }
        /// <summary>
        /// ���ɳ�����·��
        /// </summary>
        /// <param name="pathArray"></param>
        /// <param name="planID"></param>
        /// <param name="selectedSecIndex">��ѡ����������(���)</param>
        private void createTrainPath(int planID,string trainID,string selectedSecCode)
        {
            //Point pStart, pEnd;
            //TrainUnit trainUnit;
            //StaTrainInfoStr staTrain;
            //ChainStationTrain oneTrain;
            //int staType;/////�г��ڳ�վ�����͡�0Ϊ��ʼվ��1Ϊ;��վ��2Ϊ�յ�վ
            //for (int i = 0; i < stationTrains.Count; i++)
            //{
            //    oneTrain = (ChainStationTrain)stationTrains[i];
            //    oneTrain.moveFirst();
            //    trainUnit = (TrainUnit)oneTrain.currentData();
            //    staTrain = trainUnit.staTrainInfo;
            //    pStart = getStartPoint(staTrain);////���
            //    staType = 0;
            //    pathTrainLabel.StartFigure();                               
            //    pathTrainLabel.AddPath(createTrainLabelPath(pStart, oneTrain.trainStr, staType),false);///////�����г���ʶ
            //    //////////////////�����ڹ���е���  ����
            //    Point pArrive = getEndPoint(staTrain);
            //    Point pDepart = getStartPoint(staTrain);
            //    GraphicsPath pathTrack = createTrainInTrackPath(staTrain, "start", pArrive, pDepart);
            //    if (pathTrack != null)
            //        pathTrainInTrack.AddPath(pathTrack, false);

            //    oneTrain.moveNext();                                
            //    while (true)
            //    {
            //        trainUnit = (TrainUnit)oneTrain.currentData();
            //        staTrain = trainUnit.staTrainInfo;
            //        pEnd = getEndPoint(staTrain);/////�յ�

                   
            //        /////////��õ�ǰsecCode
            //        string thisSecCode = "null";

            //        if (oneTrain.trainStr.trainDirection == "down")
            //            thisSecCode =Network.SingleLineInfo.upSectionCode(staTrain.stationCode);
            //        else
            //            thisSecCode = Network.SingleLineInfo.downSectionCode(staTrain.stationCode);

            //        /////////////////////////////�����ڹ���е���
            //        string trainRouteState=string.Empty;;
            //        if (oneTrain.isEof())
            //            trainRouteState = "end";
            //        else
            //            trainRouteState = "normal";
            //        pArrive = getEndPoint(staTrain);
            //        pDepart = getStartPoint(staTrain);
            //        pathTrack = createTrainInTrackPath(staTrain, trainRouteState, pArrive,pDepart);
            //        if(pathTrack!=null)
            //            pathTrainInTrack.AddPath(pathTrack,false);

            //        #region"�����г�"
            //        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //        if (oneTrain.trainStr.trainID == trainID)//////����ѡ��trainID
            //        {
            //            pathSelectedTrain.StartFigure();
            //            pathSelectedTrain.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
            //        }


            //        if (oneTrain.trainStr.trainID == trainID && thisSecCode == selectedSecCode)//////����ѡ��secTrainID
            //        {
            //            pathSelectedSecTrain.StartFigure();
            //            pathSelectedSecTrain.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
            //        }

            //        if (oneTrain.trainStr.trainPlanID == planID)///////����ѡ��PlanID
            //        {
            //            selectTrainRank = oneTrain.trainStr.trainRank;
            //            pathSelectedPlan.StartFigure();
            //            pathSelectedPlan.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
            //        }
            //        else/////////////////////////////����ѡ��PlanID
            //        {
                        
            //            switch (oneTrain.trainStr.trainRank)
            //            {
            //                case "A":
            //                    if (oneTrain.trainStr.trainDirection == "down")
            //                    {
            //                        pathRankA_down.StartFigure();
            //                        pathRankA_down.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
                                                                                                          
            //                    }
            //                    else
            //                    {
            //                        pathRankA_up.StartFigure();
            //                        pathRankA_up.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
            //                    }
            //                    break;
            //                case "B":
            //                    if (oneTrain.trainStr.trainDirection == "down")
            //                    {
            //                        pathRankB_down.StartFigure();
            //                        pathRankB_down.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
            //                    }
            //                    else
            //                    {
            //                        pathRankB_up.StartFigure();
            //                        pathRankB_up.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
            //                    }
            //                    break;

            //                case "C":
            //                    if (oneTrain.trainStr.trainDirection == "down")
            //                    {
            //                        pathRankC_down.StartFigure();
            //                        pathRankC_down.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
            //                    }
            //                    else
            //                    {
            //                        pathRankC_up.StartFigure();
            //                        pathRankC_up.AddPath(createTrainLinePath(oneTrain.trainStr.trainID, thisSecCode, pStart, pEnd), false);
            //                    }
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //        #endregion
                    
            //        pStart = getStartPoint(staTrain);
            //        if (oneTrain.isEof())
            //        {
            //            pathTrainLabel.StartFigure();
            //            pathTrainLabel.AddPath(createTrainLabelPath(pEnd, oneTrain.trainStr, 2),false);
            //            break;
            //        }
            //        oneTrain.moveNext();
            //    }
            //}

        }
       

        /// <summary>
        /// �����г���ʶ·��[˽��]
        /// </summary>
        /// <param name="pStart"></param>
        /// <param name="pEnd"></param>
        /// <param name="direction"></param>
        /// <param name="staType">0Ϊ��ʼվ��1Ϊ;��վ��2Ϊ�յ�վ</param>
        /// <returns></returns>
        //private GraphicsPath createTrainLabelPath(Point pOri, TrainStr trainStr,int staType)
        //{
        //    GraphicsPath gPath = new GraphicsPath();
        //    int deadLine = getPoint(24 * 3600).X;
        //    Point pStart = new Point(0, 0);
        //    Point pEnd = new Point(0,0);
        //    int beginLineHeight = 8;
        //    int endLineHeight = 10;
        //    int beginLineWidth = 6;
        //    int endLineWidth = 6;
        //    if(pOri.X>deadLine)
        //        pOri.X = pOri.X - deadLine+DefaultSizeStr.leftSpaceWidth;
        //    if (trainStr.trainDirection== "down")///���е�
        //    {
        //        switch(staType)
        //        {
        //            case 0:///////////��ʼվ
        //                pStart = pOri;
        //                pEnd.X = pOri.X;
        //                pEnd.Y = pOri.Y - beginLineHeight;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);
                        
        //                pStart.X = pOri.X - beginLineWidth;
        //                pStart.Y = pOri.Y - beginLineHeight;
        //                pEnd.X = pOri.X + beginLineWidth;
        //                pEnd.Y = pOri.Y - beginLineHeight;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);                                               
        //                break;
        //            case 1://///////Ϊ;��վ
        //                break;
        //            case 2:///////2Ϊ�յ�վ
        //                pStart = pOri;
        //                pEnd.X = pOri.X;
        //                pEnd.Y = pOri.Y + endLineHeight;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);

        //                pStart.X = pOri.X - endLineWidth;
        //                pStart.Y = pOri.Y + endLineHeight;
        //                pEnd.X = pOri.X + endLineWidth;
        //                pEnd.Y = pOri.Y + endLineHeight;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);

        //                pStart.X = pOri.X - endLineWidth;
        //                pStart.Y = pOri.Y + endLineHeight;
        //                pEnd.X = pOri.X;
        //                pEnd.Y = pOri.Y + (endLineHeight+1)*2;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);

        //                pStart.X = pOri.X + endLineWidth;
        //                pStart.Y = pOri.Y + endLineHeight;
        //                pEnd.X = pOri.X;
        //                pEnd.Y = pOri.Y + (endLineHeight + 1) * 2;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);
        //                break;
        //            default: break;
        //        }
        //    }
        //    else if(trainStr.trainDirection=="up")////����
        //    {
        //        switch(staType)
        //        {
        //            case 0:///////////��ʼվ
        //                pStart = pOri;
        //                pEnd.X = pOri.X;
        //                pEnd.Y = pOri.Y +beginLineHeight;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);
                        
        //                pStart.X = pOri.X - beginLineWidth;
        //                pStart.Y = pOri.Y + beginLineHeight;
        //                pEnd.X = pOri.X + beginLineWidth;
        //                pEnd.Y = pOri.Y + beginLineHeight;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);                                               
        //                break;
        //            case 1://///////Ϊ;��վ
        //                break;
        //            case 2:///////2Ϊ�յ�վ
        //                pStart = pOri;
        //                pEnd.X = pOri.X;
        //                pEnd.Y = pOri.Y - endLineHeight;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);

        //                pStart.X = pOri.X - endLineWidth;
        //                pStart.Y = pOri.Y - endLineHeight;
        //                pEnd.X = pOri.X + endLineWidth;
        //                pEnd.Y = pOri.Y - endLineHeight;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);

        //                pStart.X = pOri.X - endLineWidth;
        //                pStart.Y = pOri.Y - endLineHeight;
        //                pEnd.X = pOri.X;
        //                pEnd.Y = pOri.Y - (endLineHeight + 1) * 2;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd);

        //                pStart.X = pOri.X + endLineWidth;
        //                pStart.Y = pOri.Y - endLineHeight;
        //                pEnd.X = pOri.X;
        //                pEnd.Y = pOri.Y - (endLineHeight + 1) * 2;
        //                gPath.StartFigure();
        //                gPath.AddLine(pStart, pEnd); 
        //                break;
        //            default: break;
        //        }
        //    }
        //    return gPath;
        //}
        
        /// <summary>
        /// �����г���·��[˽��]
        /// </summary>
        /// <param name="pStart"></param>
        /// <param name="pEnd"></param>
        /// <returns></returns>
        private GraphicsPath createTrainLinePath(string thisTrainID,string thisSecCode, Point pStart,Point pEnd)
        {
            GraphicsPath gPath=new GraphicsPath();
            int deadLine = getPoint(24 * 3600).X;
            if (pStart.X >= deadLine && pEnd.X >= deadLine)/////////ǰ����
            {
                pStart.X =pStart.X-deadLine+DefaultSizeStr.leftSpaceWidth;
                pEnd.X =pEnd.X- deadLine+DefaultSizeStr.leftSpaceWidth;
                gPath.StartFigure();
                gPath.AddLine(pStart,pEnd);
                setIDLocation(thisTrainID, thisSecCode, pStart, pEnd);
            }
            else if (pStart.X < deadLine && pEnd.X >= deadLine)/////ǰ������
            {
                Point pStart1, pStart2, pEnd1, pEnd2;
                int x1,x2,y1,y2;
                x1=pStart.X;x2=pEnd.X;y1=pStart.Y;y2=pEnd.Y;
                pStart1 = pStart;
                pEnd1 = pStart;
                pEnd1.X = deadLine;
                pEnd1.Y = (pEnd1.X - x1) * (y2 - y1) / (x2 - x1) + y1;
                pStart2 = pEnd1;
                pStart2.X = DefaultSizeStr.leftSpaceWidth;
                pEnd2 = pEnd;
                pEnd2.X = pEnd2.X - deadLine +pStart2.X;
                gPath.StartFigure();
                gPath.AddLine(pStart1, pEnd1);
                setIDLocation(thisTrainID, thisSecCode, pStart1, pEnd1);
                gPath.StartFigure();
                gPath.AddLine(pStart2, pEnd2);
                setIDLocation(thisTrainID, thisSecCode, pStart2, pEnd2);
            }
            else if (pStart.X >= deadLine && pEnd.X < deadLine)/////ǰ���󲻳�
            {
                throw new Exception();
            }
            else if (pStart.X < deadLine && pEnd.X < deadLine)////ǰ�����󲻳�
            {
                gPath.StartFigure();
                gPath.AddLine(pStart, pEnd);
                setIDLocation(thisTrainID, thisSecCode, pStart, pEnd);
            }
            return gPath;
        }

        /// <summary>
        /// ���Ƴ�վ�ڹ���е���
        /// </summary>
        /// <param name="staTrain"></param>
        /// <param name="trainRouteState"></param>
        /// <param name="pArrive"></param>
        /// <param name="pDepart"></param>
        /// <returns></returns>
        //private GraphicsPath createTrainInTrackPath(StaTrainInfoStr staTrain,string trainRouteState, Point pArrive,Point pDepart)
        //{
        //    GraphicsPath gPath = new GraphicsPath();
                        
        //    int stationIndex = Network.SingleLineInfo.getStationIndex(staTrain.stationCode);
        //    Network.StationStr station = Network.SingleLineInfo.StaStr[stationIndex];

        //    if (station.bTrackShow == false)
        //        return null;            
        //    Point pStart, pEnd;
        //    string trackID = staTrain.stationTrackID;

        //    int deadLine = getPoint(24 * 3600).X;
                           
        //    if (pDepart.X > deadLine)
        //    {
        //        pArrive.X -= 24 * 60 * DefaultSizeStr.oneMStepWidth;
        //        pDepart.X -= 24 * 60 * DefaultSizeStr.oneMStepWidth;
        //    }
        //    if (trainRouteState == "normal")
        //    {
        //        if (pArrive.X == pDepart.X)///////////ͨ������
        //        {
        //            pStart = pArrive;
        //            pEnd = pDepart;                                       
        //            gPath.AddLine(pStart, pEnd);
        //        }
        //        else/////////////ͣ������
        //        {
        //            pStart = pArrive;
        //            pEnd = pStart;
        //            int trackSort = 0;
        //            for (int i = 0; i < station.tracksStr.Length; i++)
        //            {
        //                trackSort++;
        //                if (trackID == station.tracksStr[i].trackID.ToString())
        //                    break;
        //            }
        //            if (staTrain.trainDirection == "down")
        //                pEnd.Y += trackSort * DefaultSizeStr.oneTrackEqualHeight;
        //            else
        //                pEnd.Y -= (station.tracksStr.Length + 1 - trackSort) * DefaultSizeStr.oneTrackEqualHeight;
        //            gPath.StartFigure();
        //            gPath.AddLine(pStart, pEnd);

        //            pStart = pEnd;
        //            pEnd.X = pDepart.X;
        //            gPath.StartFigure();
        //            gPath.AddLine(pStart, pEnd);

        //            pStart = pEnd;
        //            pEnd = pDepart;
        //            gPath.StartFigure();
        //            gPath.AddLine(pStart, pEnd); 
        //        }
        //    }
        //        ///////////////////��Ӧ���
        //    else if(trainRouteState=="start")
        //    {
               
        //        pStart = pDepart;
        //        pEnd = pStart;
        //        int trackSort = 0;
        //        for (int i = station.tracksStr.Length-1; i >=0; i--)
        //        {
        //            trackSort++;
        //            if (trackID == station.tracksStr[i].trackID.ToString())
        //                break;
        //        }
        //        if (staTrain.trainDirection == "down")
        //            pEnd.Y -= trackSort * DefaultSizeStr.oneTrackEqualHeight;
        //        else
        //            pEnd.Y += (station.tracksStr.Length+1- trackSort) * DefaultSizeStr.oneTrackEqualHeight;
        //        gPath.StartFigure();
        //        gPath.AddLine(pStart, pEnd);

        //        pStart = pEnd;
        //        pEnd.X = pArrive.X;
        //        gPath.StartFigure();
        //        gPath.AddLine(pStart, pEnd);

        //        gPath.StartFigure();
        //        Point location = pEnd;
        //        location.Y -= 2;
        //        Rectangle rec = new Rectangle(location, new Size(4, 4));
        //        gPath.AddRectangle(rec);


        //    }
        //        ////////////////��Ӧ�� �յ�
        //    else if (trainRouteState == "end")
        //    {
        //        pStart = pArrive;
        //        pEnd = pStart;
        //        int trackSort = 0;
        //        for (int i = 0; i < station.tracksStr.Length; i++)
        //        {
        //            trackSort++;
        //            if (trackID == station.tracksStr[i].trackID.ToString())
        //                break;
        //        }
        //        if (staTrain.trainDirection == "down")
        //            pEnd.Y += trackSort * DefaultSizeStr.oneTrackEqualHeight;
        //        else
        //            pEnd.Y -= (station.tracksStr.Length+1-trackSort) * DefaultSizeStr.oneTrackEqualHeight;
        //        gPath.StartFigure();
        //        gPath.AddLine(pStart, pEnd);

        //        pStart = pEnd;
        //        pEnd.X = pDepart.X;
        //        gPath.StartFigure();
        //        gPath.AddLine(pStart, pEnd);

        //        gPath.StartFigure();
        //        Point location=pEnd;
        //        location.Y-=2;
               
        //        Rectangle rec=new Rectangle(location,new Size(4,4));
        //        gPath.AddRectangle(rec);
        //    }
        //    return gPath;
        //}
        /// <summary>
        /// ����������[˽�к���]
        /// </summary>
        /// <param name="staTrain"></param>
        /// <returns></returns>
        //private Point getStartPoint(StaTrainInfoStr staTrain)
        //{
        //    //int stationID = staTrain.stationID;
        //    string stationCode = staTrain.stationName;
        //    int departTime = staTrain.departTime;
        //    return getPoint("start",stationCode,departTime,staTrain.trainDirection);
        //}
        /// <summary>
        /// ����յ�����[˽�к���]
        /// </summary>
        /// <param name="staTrain"></param>
        /// <returns></returns>
        //private Point getEndPoint(StaTrainInfoStr staTrain)
        //{
        //    string stationCode = staTrain.stationName;
        //    int arriveTime = staTrain.arriveTime;
        //    return getPoint("end",stationCode,arriveTime,staTrain.trainDirection);          
        //}



        private Point getPoint(int space,int time)
        {
            time *= 60;
            Point point = new Point();
            point.X = (int)((float)time * DefaultSizeStr.oneSStepWidth + DefaultSizeStr.leftSpaceWidth);
            point.Y = (int)((float)space * DefaultSizeStr.oneKStepHeight + DefaultSizeStr.topSpaceHeight);
            return point;
        }

        private Point getPoint(int time)
        {
            Point point = new Point();

            point.X = (int)((float)time * DefaultSizeStr.oneSStepWidth + DefaultSizeStr.leftSpaceWidth);
            point.Y = 0;
            return point;
        }
        /// <summary>
        /// ����ԭʼ��X���ȡ��Ӧʱ�䣬��Ϊ��λ[˽�к���]
        /// </summary>
        /// <param name="pointX"></param>
        /// <returns></returns>
        private string getTime(int pointX)
        {
            //float fTimeS = (((float)pointX - (float)DefaultSizeStr.leftSpaceWidth) / (float)DefaultSizeStr.oneSStepWidth);
            //int timeS = (int)Math.Round((decimal)(fTimeS / 10f)*10);///////////��������
            //return Function.GlobalFunction.setFormatTime(timeS,"mm:ss");
            return "0";
        }
        /// <summary>
        /// ����idLocation��̬����[˽�к���]
        /// </summary>
        /// <param name="trainPlanID"></param>
        /// <param name="pStart"></param>
        /// <param name="pEnd"></param>
        private void setIDLocation(string trainID, string secCode, Point pStart, Point pEnd)
        {
            TrainIDLocation oneLocation = new TrainIDLocation();
            oneLocation.trainID = trainID;
            oneLocation.sectionCode = secCode;
            oneLocation.pStartOri = pStart;
            oneLocation.pEndOri = pEnd;
            pStart.X =(int)((float)pStart.X* PathMatrix.widthScale);
            pStart.Y = (int)((float)pStart.Y * PathMatrix.heightScale);
            pEnd.X = (int)((float)pEnd.X * PathMatrix.widthScale);
            pEnd.Y=(int)((float)pEnd.Y*PathMatrix.heightScale);

            oneLocation.pStart = pStart;
            oneLocation.pEnd = pEnd;
            idLocation.Add(oneLocation);
        }
        /// <summary>
        /// �����idLocation��ƥ��ĵ�[����]
        /// </summary>
        /// <param name="pOri"></param>
        /// <returns></returns>
        private Point pointToIDLocation_scale(Point pOri)
        {
            float offsetX = PathMatrix.offsetX;
            float offsetY = PathMatrix.offsetY;
            offsetX = offsetX - (float)DefaultSizeStr.leftSpaceWidth * (PathMatrix.widthScale - 1f);
            offsetY = offsetY - (float)DefaultSizeStr.upHourWordHeight * (PathMatrix.heightScale - 1f);
            int x = pOri.X - (int)offsetX;
            int y = pOri.Y- (int)offsetY;
            return new Point(x, y); 
        }
        private Point pointToIDLocation_scale(int x, int y)
        {
            float offsetX = PathMatrix.offsetX;
            float offsetY = PathMatrix.offsetY;
            offsetX = offsetX - (float)DefaultSizeStr.leftSpaceWidth * (PathMatrix.widthScale - 1f);
            offsetY = offsetY - (float)DefaultSizeStr.upHourWordHeight * (PathMatrix.heightScale - 1f);
            x = x - (int)offsetX;
            y =y - (int)offsetY;
            return new Point(x, y); 
 
        }
        /// <summary>
        /// �����matrix��ƥ��ĵ�[����]
        /// </summary>
        /// <param name="pOri"></param>
        /// <returns></returns>
        private Point pointToMatrix(Point pOri)
        {
            float offsetX = PathMatrix.offsetX;
            float offsetY = PathMatrix.offsetY;
            offsetX = offsetX - (float)DefaultSizeStr.leftSpaceWidth * (PathMatrix.widthScale - 1f);
            offsetY = offsetY - (float)DefaultSizeStr.upHourWordHeight * (PathMatrix.heightScale - 1f);
            int x = (int)(((float)pOri.X* PathMatrix.widthScale + offsetX) );
            int y = (int)(((float)pOri.Y* PathMatrix.heightScale + offsetY) );
            return new Point(x,y);
        }
        #endregion"���� ��ͼ˽��"
    }
}
