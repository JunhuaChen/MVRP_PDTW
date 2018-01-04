using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MvrpLite.data;
using MvrpLite.Draw;
namespace MvrpLite.adminFuc
{
    public partial class panelView : UserControl
    {
        private static int _MAX_STATION_NUM=10;
        private static int _MAX_TRAIN_NUM = 20;
        private static int _MAX_LOCOMOTIVE_NUM = 10;
        int[] m_all_nodes_name = new int[_MAX_STATION_NUM*5];
        int[] m_all_nodes_y = new int[_MAX_STATION_NUM * 5];
        int m_total_runTime;
        int[,] m_train_runTime = new int[_MAX_STATION_NUM + 1, _MAX_STATION_NUM + 1];
        int[] m_set_s = new int[_MAX_STATION_NUM*2 + 1];
        int[] m_set_p = new int[_MAX_STATION_NUM*2 + 1];
        int[] m_set_o_1 = new int[_MAX_STATION_NUM*2 + 1];
        int[] m_set_o_2 = new int[_MAX_STATION_NUM*2 + 1];
        int[] m_set_d = new int[_MAX_STATION_NUM*2 + 1];

        int[,] m_train_route = new int[_MAX_STATION_NUM*2 + 1, _MAX_STATION_NUM*2 + 1];
        int[,] m_train_arrive_time = new int[_MAX_STATION_NUM*2 + 1, _MAX_STATION_NUM*2 + 1];
        int[,] m_train_depart_time = new int[_MAX_STATION_NUM*2 + 1, _MAX_STATION_NUM*2 + 1];


        int[] m_o_1_station = new int[_MAX_LOCOMOTIVE_NUM+ 1];
        int[] m_o_2_station = new int[_MAX_LOCOMOTIVE_NUM + 1];
        int[] m_p_station = new int[_MAX_TRAIN_NUM + 1];
        int[] m_d_station = new int[_MAX_TRAIN_NUM + 1];

        int[,] m_con_start_node = new int[_MAX_LOCOMOTIVE_NUM, _MAX_STATION_NUM + 1];
        int[,] m_con_start_u = new int[_MAX_LOCOMOTIVE_NUM, _MAX_STATION_NUM + 1];
        int[,] m_con_end_node = new int[_MAX_LOCOMOTIVE_NUM, _MAX_STATION_NUM + 1];
        int[,] m_con_end_u = new int[_MAX_LOCOMOTIVE_NUM, _MAX_STATION_NUM + 1];
        int[,] m_con_start_time = new int[_MAX_LOCOMOTIVE_NUM, _MAX_STATION_NUM + 1];
        int[,] m_con_end_time = new int[_MAX_LOCOMOTIVE_NUM, _MAX_STATION_NUM + 1];

        private Draw.DrawFrame Draw_Frame;
        private Draw.DrawTrains Draw_Trains;
       
        private Draw.PathMatrix pathMatrix;
        private Draw.DisplayStr displayStr;
        bool[] chkTimeStation;////显示车站时间轴
        double vScaleGraph;   //垂直方向的真实移动距离与滚动条改变值的比例
        double hScaleGraph;  //水平方向的真实移动距离与滚动条改变值的比例

        //////////////////////////////////////////////////////////////鼠标控件相关的变量
        Point pLocation;//鼠标按下时的坐标             
        bool bFirstAajust = true;
        int mouseState;//鼠标形态，1表示点击放大；2表示点击缩小；3表示拖放放大缩小;4表示手动
        int adjustState;//调整类型 1 移动区间车；2 移动区间车的点；3 移动全线车
        int moveState;/////移动状态，1表示向上，2表示向右，3表示向下，4表示向左
        ToolTip tipTrain;/////列车提示
        Cursor _moveCur;//手移的指针
        Cursor _zoomBCur;//放大的指针
        Cursor _zoomSCur;//缩小的指针
        Cursor _zoomDCur;//动态变换的指针
        Cursor _moveToRight;//右移
        Cursor _moveToLeft;//左移
        Cursor _moveToUp;//上移
        Cursor _moveToDown;//下移
        Cursor _adjust;////调整
        Cursor _adjustPoint;////点调整
        Cursor _adjustTrain;////全线调整


        #region"初始化"
        public panelView()
        {
            InitializeComponent();
            this.tabControlData.SelectedIndex = 0; 
        }
        public void setTabControlSelectedIndex(int selectIndex)
        {
            this.tabControlData.SelectedIndex = selectIndex;
        }
        /// <summary>
        /// draw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelView_Load(object sender, EventArgs e)
        {
            //inputData();
            //resetDataForDarw();

            //Draw_Frame = new DrawFrame(m_all_nodes_name,m_all_nodes_y,m_set_s);
            //Draw_Trains = new DrawTrains();
            //drawConf();
            //initDrawTSB();        ////对绘图工具栏的初始化

        }

        private void panelGraph_Paint(object sender, PaintEventArgs e)
        {
            redimScroll();
            displayStr.isTrainMode = true;

            Draw_Frame.drawTable(e.Graphics, this.panelGraph.Width, this.panelGraph.Height, displayStr);
            Draw_Frame.drawTimeRuler(ref this.picTimeUp);
            Draw_Frame.drawStationWord(ref this.picStationLeft);

            Draw_Trains.execute(e.Graphics,m_train_route,m_train_arrive_time,m_train_depart_time,m_all_nodes_name,m_all_nodes_y,
                 m_con_start_node, m_con_end_node, m_con_start_time, m_con_end_time, m_con_start_u, m_con_end_u,
            displayStr);
            e.Dispose();
        }
              
        #endregion
        #region"draw initial"
        private void drawConf()
        {

            m_total_runTime = 0;
            for (int i = 1; i <= m_all_nodes_y[0]; i++)
            {
                if (m_all_nodes_y[i] > m_total_runTime)
                    m_total_runTime = m_all_nodes_y[i];
            }

            pathMatrix = new Draw.PathMatrix();
            Draw.DrawConf.confDefaultSize(m_total_runTime);
            Draw.DrawConf.reconfDefaultSize(m_total_runTime, 40, 0, 0, 90);
            displayStr = Draw.DrawConf.confDisplay();
            Draw.DrawConf.confPen();
            Draw.DrawConf.confFont();

            //tipTrain = new ToolTip();

            //bSectionName = false;
            //graphicMode = "train";
            //panelGraph.Dock = DockStyle.Fill;


        }
        /// <summary>
        /// 初始化绘图的工具栏
        /// </summary>
        private void initDrawTSB()
        {
            chkTimeStation = new bool[4];
            if (Draw.DefaultSizeStr.upHourWordHeight == 0)
            {
                chkTimeStation[0] = false;
                this.tsmTimeUp.Image = getTimeStationGif();
            }
            else
            {
                chkTimeStation[0] = true;
                this.tsmTimeUp.Image = getTimeStationGif();
            }
            if (Draw.DefaultSizeStr.rightStationWordWidth == 0)
            {
                chkTimeStation[1] = false;
                this.tsmStationRight.Image = getTimeStationGif();
            }
            else
            {
                chkTimeStation[1] = true;
                this.tsmStationRight.Image = getTimeStationGif();
            }
            if (Draw.DefaultSizeStr.downHourWordHeight == 0)
            {
                chkTimeStation[2] = false;
                this.tsmTimeDown.Image = getTimeStationGif();
            }
            else
            {
                chkTimeStation[2] = true;
                this.tsmTimeDown.Image = getTimeStationGif();
            }
            if (Draw.DefaultSizeStr.leftStationWordWidth == 0)
            {
                chkTimeStation[3] = false;
                this.tsmStationLeft.Image = getTimeStationGif();
            }
            else
            {
                chkTimeStation[3] = true;
                this.tsmStationLeft.Image = getTimeStationGif();
            }

            this.tsbTimeStation.Image = getTimeStationGif();
        }
        private Image getTimeStationGif()
        {
            string timeStationGif = "timeStation";
            if (chkTimeStation[0])
                timeStationGif += "1";
            else
                timeStationGif += "0";
            if (chkTimeStation[1])
                timeStationGif += "1";
            else
                timeStationGif += "0";
            if (chkTimeStation[2])
                timeStationGif += "1";
            else
                timeStationGif += "0";
            if (chkTimeStation[3])
                timeStationGif += "1";
            else
                timeStationGif += "0";
            Bitmap image = new Bitmap(DataPath.CurrentPath + @"img\timeStation\" + timeStationGif + ".gif", true);
            return (Image)image;
        }

        #endregion

        #region "input data"
        private void inputData()
        {
            for (int i = 0; i < m_all_nodes_name.Length; i++) m_all_nodes_name[i] = 0;
            for (int i = 0; i < m_all_nodes_y.Length; i++) m_all_nodes_y[i] = 0;
            m_total_runTime=0;
            for (int i = 0; i <= _MAX_STATION_NUM; i++)
                for (int j = 0; j <= _MAX_STATION_NUM; j++) m_train_runTime[i, j] = 0;
            for (int i = 0; i < m_set_s.Length; i++) m_set_s[i] = 0;
            for (int i = 0; i < m_set_p.Length; i++) m_set_p[i] = 0;
            for (int i = 0; i < m_set_o_1.Length; i++) m_set_o_1[i] = 0;
            for (int i = 0; i < m_set_o_2.Length; i++) m_set_o_2[i] = 0;
            for (int i = 0; i < m_set_d.Length; i++) m_set_d[i] = 0;

            for (int i = 0; i < m_o_1_station.Length; i++) m_o_1_station[i] = 0;
            for (int i = 0; i < m_o_2_station.Length; i++) m_o_2_station[i] = 0;
            for (int i = 0; i < m_p_station.Length; i++) m_p_station[i] = 0;
            for (int i = 0; i < m_d_station.Length; i++) m_d_station[i] = 0;

            for (int i = 0; i <= _MAX_STATION_NUM * 2; i++)
                for (int j = 0; j <= _MAX_STATION_NUM * 2; j++) m_train_route[i, j] = 0;
            for (int i = 0; i <= _MAX_STATION_NUM * 2; i++)
                for (int j = 0; j <= _MAX_STATION_NUM * 2; j++) m_train_arrive_time[i, j] = 0;
            for (int i = 0; i <= _MAX_STATION_NUM * 2; i++)
                for (int j = 0; j <= _MAX_STATION_NUM * 2; j++) m_train_depart_time[i, j] = 0;

            for (int i = 0; i < _MAX_LOCOMOTIVE_NUM; i++)
                for (int j = 0; j <= _MAX_STATION_NUM; j++) m_con_start_node[i, j] = 0;
            for (int i = 0; i < _MAX_LOCOMOTIVE_NUM; i++)
                for (int j = 0; j <= _MAX_STATION_NUM; j++) m_con_start_u[i, j] = 0;
            for (int i = 0; i < _MAX_LOCOMOTIVE_NUM; i++)
                for (int j = 0; j <= _MAX_STATION_NUM; j++) m_con_end_node[i, j] = 0;
            for (int i = 0; i < _MAX_LOCOMOTIVE_NUM; i++)
                for (int j = 0; j <= _MAX_STATION_NUM; j++) m_con_end_u[i, j] = 0;
            for (int i = 0; i < _MAX_LOCOMOTIVE_NUM; i++)
                for (int j = 0; j <= _MAX_STATION_NUM; j++) m_con_start_time[i, j] = 0;
            for (int i = 0; i < _MAX_LOCOMOTIVE_NUM; i++)
                for (int j = 0; j <= _MAX_STATION_NUM; j++) m_con_end_time[i, j] = 0;
            
           

            /////////////////////////////////////////
            string fileName = DataPath.CustomerPath + "data_running_time.csv";//文件路径
            DataTable m_dtNode = get_data_from_CSV(fileName);
           
            foreach (DataRow dr in m_dtNode.Rows)
            {
                int fromStation = int.Parse(dr["from_station"].ToString());
                int toStation = int.Parse(dr["to_station"].ToString());
                int runningTime = int.Parse(dr["running_time"].ToString());

                m_train_runTime[fromStation, toStation] = runningTime;               
            }

            fileName = DataPath.CustomerPath + "data_train_plan.csv";//文件路径
            DataTable m_dtAgent = new DataTable();
            m_dtAgent = get_data_from_CSV(fileName);

            fileName = DataPath.CustomerPath + "data_locomotive.csv";//文件路径
            DataTable m_dtConfiguration = new DataTable();
            m_dtConfiguration = get_data_from_CSV(fileName);
           
            ////////set p_station,d_station
           
            int nLocomotive = 0;
            foreach (DataRow dr in m_dtConfiguration.Rows)
            {
                int locomotiveNum = int.Parse(dr["locomotiveNumbers"].ToString());
                int origin_station = int.Parse(dr["origin_station"].ToString());
                int desti_station = int.Parse(dr["desti_station"].ToString());
                for (int i = 1; i <= locomotiveNum; i++)
                {
                    m_o_1_station[nLocomotive + i] = origin_station;
                    m_o_2_station[nLocomotive + i] = desti_station;

                    m_o_1_station[0]++;
                    m_o_2_station[0]++;
                }
                nLocomotive += locomotiveNum;
            }


           
            int nTrainCount = 0;
            foreach (DataRow dr in m_dtAgent.Rows)
            {
                int nowTrainNumbers = int.Parse(dr["trainNumbers"].ToString());

                int origin_station = int.Parse(dr["origin_station"].ToString());
                int desti_station = int.Parse(dr["desti_station"].ToString());
                for (int i = 1; i <= nowTrainNumbers; i++)
                {
                    m_p_station[nTrainCount + i] = origin_station;
                    m_d_station[nTrainCount + i] = desti_station;

                    m_p_station[0]++;
                    m_d_station[0]++;
                }
                nTrainCount += nowTrainNumbers;
            }


            ///////////////////////////read set
           
            fileName = DataPath.CustomerPath + "gams_output_ospd.txt";//文件路径
            StreamReader din = File.OpenText(fileName);
            String oneStr= string.Empty;
            string set_type = "";

            m_set_d[0] = 0; m_set_o_1[0] = 0; m_set_o_2[0] = 0; m_set_p[0] = 0; m_set_s[0] = 0;
            while ((oneStr = din.ReadLine()) != null)
            {
                if (oneStr == "s" || oneStr == "o_1" || oneStr == "o_2" || oneStr == "p" || oneStr == "d")
                    set_type = oneStr;
                else
                {
                    if(set_type=="s")
                    {
                        m_set_s[++m_set_s[0]] = int.Parse(oneStr);
                    }
                    else if(set_type=="o_1")
                    {
                        m_set_o_1[++m_set_o_1[0]] = int.Parse(oneStr);
                    }
                    else if (set_type == "o_2")
                    {
                        m_set_o_2[++m_set_o_2[0]] = int.Parse(oneStr);
                    }
                    else if (set_type == "p")
                    {
                        m_set_p[++m_set_p[0]] = int.Parse(oneStr);
                    }
                    else if (set_type == "d")
                    {
                        m_set_d[++m_set_d[0]] = int.Parse(oneStr);
                    }
                }               
            }
            din.Close();

            #region"read route and time"
            ///////////////////////////read route and time

            int[] train_route_start = new int[_MAX_STATION_NUM*2]; int[] train_route_end = new int[_MAX_STATION_NUM*2];
            int[] train_depart_start = new int[_MAX_STATION_NUM*2];int[] train_depart_end = new int[_MAX_STATION_NUM*2];
            int[] train_arrive_start = new int[_MAX_STATION_NUM*2];int[] train_arrive_end = new int[_MAX_STATION_NUM*2];

            int[,] train_route = new int[_MAX_STATION_NUM*2 + 1, _MAX_STATION_NUM*2 + 1];
            int[,] train_arrive_time = new int[_MAX_STATION_NUM*2 + 1, _MAX_STATION_NUM*2 + 1];
            int[,] train_depart_time = new int[_MAX_STATION_NUM*2 + 1, _MAX_STATION_NUM*2 + 1];


            int[,] train_arriveTime_matrix = new int[_MAX_TRAIN_NUM, 500];
            int[,] train_departTime_matrix = new int[_MAX_TRAIN_NUM, 500];

            fileName = DataPath.CustomerPath + "gams_output_tad.txt";//文件路径
            din = File.OpenText(fileName);
            oneStr = string.Empty;
            int _max_trainNum=0,_max_stationNum=0;
            
            while ((oneStr = din.ReadLine()) != null)
            {
                string[] strLines = oneStr.ToString().Split(' ');
                string[] strLinesValue = new string[4];
                int nValue = 0;
                for(int i=0;i<strLines.Length;i++)
                {
                    if (strLines[i] != "")
                    {
                        strLinesValue[nValue] = strLines[i]; nValue++;
                    }
                }

                int trainNum = int.Parse(strLinesValue[0]);
                int stationNum = int.Parse(strLinesValue[1]);
                int arriveTime = int.Parse(strLinesValue[2].Split('.')[0]);
                int depatureTime = int.Parse(strLinesValue[3].Split('.')[0]);

                train_arriveTime_matrix[trainNum, stationNum] = arriveTime;
                train_departTime_matrix[trainNum, stationNum] = depatureTime;

                if (stationNum>300 && stationNum<400) // it is start point
                {
                    train_route_start[trainNum] = stationNum;
                    train_depart_start[trainNum] = depatureTime;
                    train_arrive_start[trainNum] = arriveTime; 
                }
                else if (stationNum > 400 && stationNum < 500) // it is end point
                {
                    train_route_end[trainNum] = stationNum;
                    train_depart_end[trainNum] = depatureTime;
                    train_arrive_end[trainNum] = arriveTime;
                }
                else
                {
                    train_route[trainNum, ++train_route[trainNum,0]] = stationNum;
                    train_arrive_time[trainNum, train_route[trainNum, 0]] = arriveTime;
                    train_depart_time[trainNum, train_route[trainNum, 0]] = depatureTime;
                    train_arrive_time[trainNum, 0]++;  train_depart_time[trainNum, 0]++;
                    if(trainNum>_max_trainNum)_max_trainNum=trainNum;
                    if(stationNum>_max_stationNum)_max_stationNum=stationNum;
                }

            }
            train_route[0, 0] = _max_trainNum;
            din.Close();

            for(int i=1;i<=_max_trainNum;i++)
            {
                
                int[] sortArriveTime=new int[train_arrive_time[i,0]];
                for (int j = 0; j < train_arrive_time[i, 0]; j++) sortArriveTime[j] = train_arrive_time[i, j + 1];
                sortArriveTime = BubbleSort(sortArriveTime);

                /////sort trainRoute
                int[] newTrainRoute=new int[train_route[i,0]+1];
                for (int j = 0; j < sortArriveTime.Length; j++)
                {
                    for(int k=1;k<=train_route[i,0];k++)
                    {
                        if (train_arrive_time[i, k] == sortArriveTime[j])
                        {                          
                            newTrainRoute[j + 1] = train_route[i, k];
                        }
                    }
                }
                train_route[i, 1] = train_route_start[i];
                train_route[i, train_route[i,0]+2] = train_route_end[i];
                for (int j = 1; j <= train_route[i, 0]; j++)
                    train_route[i, j+1] = newTrainRoute[j];

                ////sort arrivetime
                train_arrive_time[i, 1] = train_arrive_start[i];
                train_arrive_time[i, train_arrive_time[i,0]+2] = train_arrive_end[i];
                for (int j = 0; j < sortArriveTime.Length; j++)
                    train_arrive_time[i, j + 2] = sortArriveTime[j];


                ////////////sort depart time

                int[] sortDepartTime = new int[train_depart_time[i, 0]];
                for (int j = 0; j < train_depart_time[i, 0]; j++) sortDepartTime[j] = train_depart_time[i, j + 1];
                sortDepartTime = BubbleSort(sortDepartTime);

                train_depart_time[i, 1] = train_depart_start[i];
                train_depart_time[i, train_depart_time[i, 0] + 2] = train_depart_end[i];
                for (int j = 0; j < sortDepartTime.Length; j++)
                    train_depart_time[i, j + 2] = sortDepartTime[j];
            }
            //////////////////////////////all sort route ,we should add dummy nodes for draw
            m_train_route[0, 0] = train_route[0, 0];
            for(int i=1;i<=train_route[0,0]+2;i++)
            {
                m_train_route[i, 0] = train_route[i, 0] + 2;
                if(train_route[i,3]>train_route[i,2])//////the direction is down
                {
                    int addNodeCount = 0;
                    for(int j=1;j<=m_train_route[i,0];j++)
                    {
                        if (j == 1 || j == m_train_route[i, 0])
                        {
                            m_train_route[i, j + addNodeCount] = train_route[i, j];
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                        }
                        else if (j == 2)
                        {
                            m_train_route[i, j + addNodeCount] = train_route[i, j] + 1000;
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                        }
                        else if (j == m_train_route[i, 0] - 1)
                        {
                            m_train_route[i, j + addNodeCount] = train_route[i, j];
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                        }
                        else   /////////need add dummy nodes
                        {
                            m_train_route[i, j + addNodeCount] = train_route[i, j];
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                            
                            addNodeCount++;
                            m_train_route[i, j + addNodeCount] = train_route[i, j]+1000;
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                        }
                    }
                    m_train_route[i, 0] += addNodeCount;

                    
                }
                else         //////////the direction is up
                {
                    int addNodeCount = 0;
                    for (int j = 1; j <= m_train_route[i, 0]; j++)
                    {
                        if (j == 1 || j == m_train_route[i, 0])
                        {
                            m_train_route[i, j + addNodeCount] = train_route[i, j];
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                        }
                        else if (j == 2)
                        {
                            m_train_route[i, j + addNodeCount] = train_route[i, j] ;
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                        }
                        else if (j == m_train_route[i, 0] - 1)
                        {
                            m_train_route[i, j + addNodeCount] = train_route[i, j]+1000;
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                        }
                        else   /////////need add dummy nodes
                        {
                            m_train_route[i, j + addNodeCount] = train_route[i, j]+1000;
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];

                            addNodeCount++;
                            m_train_route[i, j + addNodeCount] = train_route[i, j] ;
                            m_train_arrive_time[i, j + addNodeCount] = train_arrive_time[i, j];
                            m_train_depart_time[i, j + addNodeCount] = train_depart_time[i, j];
                        }
                    }
                    m_train_route[i, 0] += addNodeCount;
                }

            }

            #endregion

            //////////////////////////read x(k,i,j,u,v)
            ////
            fileName = DataPath.CustomerPath + "gams_output_x.txt";//文件路径
            din = File.OpenText(fileName);
            oneStr = string.Empty;

           
            while ((oneStr = din.ReadLine()) != null)
            {
                string[] strLines = oneStr.ToString().Split(' ');
                string[] strLinesValue = new string[6];
                int nValue = 0;
                for (int i = 0; i < strLines.Length; i++)
                {
                    if (strLines[i] != "")
                    {
                        strLinesValue[nValue] = strLines[i]; nValue++;
                    }
                }

                int k = int.Parse(strLinesValue[0]);
                int startNode = int.Parse(strLinesValue[1]);
                int endNode = int.Parse(strLinesValue[2]);
                int startU = int.Parse(strLinesValue[3]);
                int endU = int.Parse(strLinesValue[4]);

                if (startU == endU) continue;
                m_con_start_node[k, ++m_con_start_node[k, 0]] = startNode;
                m_con_end_node[k, ++m_con_end_node[k, 0]] = endNode;
                m_con_start_u[k, ++m_con_start_u[k, 0]] = startU;
                m_con_end_u[k, ++m_con_end_u[k, 0]] = endU;

                m_con_start_time[k, ++m_con_start_time[k, 0]] = train_departTime_matrix[startU, startNode];
                m_con_end_time[k, ++m_con_end_time[k, 0]] = train_departTime_matrix[endU, endNode];

                m_con_start_node[0, 0] = k; m_con_end_node[0, 0] = k; m_con_start_time[0, 0] = k; m_con_end_time[0, 0] = k;

            }
            din.Close();



        }


        private int[] BubbleSort(int[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                for (int j = i; j < list.Length; j++)
                {
                    if (list[i] >= list[j])
                    {
                        int temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }

        public DataTable get_data_from_CSV(string pCsvPath)
        {
            String line;
            String[] split = null;
            DataTable dtInfo = new DataTable();
            DataRow row = null;
            StreamReader sr = new StreamReader(pCsvPath, System.Text.Encoding.Default);
            //创建与数据源对应的数据列 
            line = sr.ReadLine();
            split = line.Split(',');
            foreach (String colname in split)
            {
                dtInfo.Columns.Add(colname, System.Type.GetType("System.String"));
            }
            //将数据填入数据表 
            int j = 0;
            while ((line = sr.ReadLine()) != null)
            {
                j = 0;
                row = dtInfo.NewRow();
                split = line.Split(',');
                foreach (String colname in split)
                {
                    row[j] = colname;
                    j++;
                }
                dtInfo.Rows.Add(row);
            }
            sr.Close();

            return dtInfo;
        }

        #endregion

        #region"reset data for draw"
        private void resetDataForDarw()
        {
            int dummyNodeHeight = 1;
            int[] max_dummyStation_number = new int[m_set_s[0] + 1];
            for(int i=1;i<=m_set_s[0];i++)
            {
                int max_number_depart=0,max_number_arrive=0;
                int nowStationName = m_set_s[i];
                for (int j = 1; j <= m_o_1_station[0]; j++)
                    if (m_o_1_station[j] == nowStationName) max_number_depart++;
                for (int j = 1; j <= m_p_station[0]; j++)
                    if (m_p_station[j] == nowStationName) max_number_depart++;

                for (int j = 1; j <= m_o_2_station[0]; j++)
                    if (m_o_2_station[j] == nowStationName) max_number_arrive++;
                for (int j = 1; j <= m_d_station[0]; j++)
                    if (m_d_station[j] == nowStationName) max_number_arrive++;

                max_dummyStation_number[i] = max_number_arrive > max_number_depart ? max_number_arrive : max_number_depart;
                if (max_dummyStation_number[i]>0)
                    max_dummyStation_number[i] +=1;
            }
            int totalRunTime=0;
            int[] tempY_for_station = new int[m_set_s[0] + 1];
            int[] tempY2_for_station = new int[m_set_s[0] + 1];
            for (int i = 1; i <= m_set_s[0]; i++)
            {
                int totalDummyNumber=0;
                for(int j=1;j<=i;j++)totalDummyNumber+=max_dummyStation_number[j];

                tempY2_for_station[i] = dummyNodeHeight * totalDummyNumber + totalRunTime;
                tempY_for_station[i] = tempY2_for_station[i] - dummyNodeHeight * max_dummyStation_number[i];

                if(i!=m_set_s[0])
                    totalRunTime+=m_train_runTime[i,i+1];
            }
            ///////////////begin set
            m_all_nodes_name[0]=0;
            m_all_nodes_y[0]=0;
            int[] dummyNodeCountA =new int[m_set_s[0]+1];
            int[] dummyNodeCountB = new int[m_set_s[0] + 1];

            for(int i=1;i<=m_set_o_1[0];i++)
            {
                m_all_nodes_name[++m_all_nodes_name[0]] = m_set_o_1[i];
                int nowStationOrder = m_o_1_station[i];
                m_all_nodes_y[++m_all_nodes_y[0]] = tempY_for_station[nowStationOrder] + (++dummyNodeCountA[nowStationOrder]) * dummyNodeHeight;
            }
         
            for (int i = 1; i <= m_set_p[0]; i++)
            {
                m_all_nodes_name[++m_all_nodes_name[0]] = m_set_p[i];
                int nowStationOrder = m_p_station[i];
                m_all_nodes_y[++m_all_nodes_y[0]] = tempY_for_station[nowStationOrder] + (++dummyNodeCountA[nowStationOrder]) * dummyNodeHeight;
            }
           
            for (int i = 1; i <= m_set_d[0]; i++)
            {
                m_all_nodes_name[++m_all_nodes_name[0]] = m_set_d[i];
                int nowStationOrder = m_d_station[i];
                m_all_nodes_y[++m_all_nodes_y[0]] = tempY2_for_station[nowStationOrder] - (++dummyNodeCountB[nowStationOrder]) * dummyNodeHeight;
            }
           
            for (int i = 1; i <= m_set_o_2[0]; i++)
            {
                m_all_nodes_name[++m_all_nodes_name[0]] = m_set_o_2[i];
                int nowStationOrder = m_o_2_station[i];
                m_all_nodes_y[++m_all_nodes_y[0]] = tempY2_for_station[nowStationOrder] - (++dummyNodeCountB[nowStationOrder]) * dummyNodeHeight;
            }

           
            for(int i=1;i<tempY_for_station.Length;i++)
            {
                m_all_nodes_name[++m_all_nodes_name[0]] =m_set_s[i];
                m_all_nodes_y[++m_all_nodes_y[0]] = tempY_for_station[i];
            }
            for (int i = 1; i < tempY2_for_station.Length; i++)
            {
                m_all_nodes_name[++m_all_nodes_name[0]] = m_set_s[i]+1000;
                m_all_nodes_y[++m_all_nodes_y[0]] = tempY2_for_station[i];
            }

        }

        #endregion




        #region"scrolls" 
        /// <summary>
        /// 水平滚动条动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollGraph_Scroll(object sender, ScrollEventArgs e)
        {
            double diff = (double)(this.hScrollGraph.Value - this.hScrollGraph.Minimum);

            pathMatrix.setOffsetX((int)(this.hScaleGraph * diff));


            this.panelGraph.Invalidate();

        }
        /// <summary>
        /// 垂直滚动条动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vScrollGraph_Scroll(object sender, ScrollEventArgs e)
        {
            double diff = (double)(this.vScrollGraph.Value - this.vScrollGraph.Minimum);

            pathMatrix.setOffsetY((int)(this.vScaleGraph * diff));
            this.panelGraph.Invalidate();

            //this.panelGraph_TimeUp.Invalidate();

        }

        /// <summary>
        /// 重定义滚动条大小
        /// </summary>
        private void redimScroll()
        {

            //设定滚动条最大变化的值
            this.vScrollGraph.LargeChange = this.vScrollGraph.Maximum * this.panelGraph.Height / (int)(Draw.DefaultSizeStr.graphicHeight * Draw.PathMatrix.heightScale);
            this.hScrollGraph.LargeChange = this.hScrollGraph.Maximum * this.panelGraph.Width / (int)(Draw.DefaultSizeStr.graphicWidth * Draw.PathMatrix.widthScale);


            //计算vScaleGraph,hScale的值
            hScaleGraph = (double)(this.hScrollGraph.Width - (int)(Draw.DefaultSizeStr.graphicWidth * Draw.PathMatrix.widthScale)) /
                     (this.hScrollGraph.Maximum - this.hScrollGraph.Minimum - this.hScrollGraph.LargeChange + 1);
            vScaleGraph = (double)(this.vScrollGraph.Height - (int)(Draw.DefaultSizeStr.graphicHeight * Draw.PathMatrix.heightScale) + 1) /
                     (this.vScrollGraph.Maximum - this.vScrollGraph.Minimum - this.vScrollGraph.LargeChange + 1);

        }
        #endregion

        #region"图形移动与缩放"
        private void tsDbZoom_Click(object sender, EventArgs e)
        {
            mouseState = 0;///默认状态
            this.tsDbZoom.Image = tsmDynamic.Image;
        }
        private void tsmBigger_Click(object sender, EventArgs e)
        {
            this.tsDbZoom.Image = tsmBigger.Image;
            mouseState = 1;/////放大状态
        }
        private void tsmSmaller_Click(object sender, EventArgs e)
        {
            this.tsDbZoom.Image = tsmSmaller.Image;
            mouseState = 2;///缩小状态
        }
        private void tsmDynamic_Click(object sender, EventArgs e)
        {
            this.tsDbZoom.Image = tsmDynamic.Image;
            mouseState = 3;///动态拖放状态
        }

        private void tsbMoveState_Click(object sender, EventArgs e)
        {
            mouseState = 4;///手移状态            
        }
        /// <summary>
        /// 原始大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDefaultSize_Click(object sender, EventArgs e)
        {
            mouseState = 0;
            pathMatrix.setHeightScale(1);
            pathMatrix.setWidthScale(1);
            pathMatrix.setOffsetX(0);
            pathMatrix.setOffsetY(0);
            this.tsTbScale.Text = "100%";
            this.panelGraph.Invalidate();
        }
        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbBigger_Click(object sender, EventArgs e)
        {
            mouseState = 0;
            pathMatrix.setHeightScale(Draw.PathMatrix.heightScale + 0.2f);
            pathMatrix.setWidthScale(Draw.PathMatrix.widthScale + 0.2f);
            this.tsTbScale.Text = (Draw.PathMatrix.widthScale + 0.2f) * 100 + "%";
            this.panelGraph.Refresh();
        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSmaller_Click(object sender, EventArgs e)
        {
            mouseState = 0;
            float heightScale = Draw.PathMatrix.heightScale;
            float widthScale = Draw.PathMatrix.widthScale;
            heightScale -= 0.2f;
            widthScale -= 0.2f;
            if (heightScale <= 0)
                heightScale = 1f;
            if (widthScale <= 0)
                widthScale = 1f;
            pathMatrix.setOffsetX(0f);
            pathMatrix.setOffsetY(0f);
            pathMatrix.setHeightScale(heightScale);
            pathMatrix.setWidthScale(widthScale);
            this.tsTbScale.Text = (int)(widthScale * 100) + "%";
            this.panelGraph.Refresh();
        }
        /// <summary>
        /// 全天模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAllHour_Click(object sender, EventArgs e)
        {

            float widthScale = (float)(this.panelGraph.Width - this.picStationLeft.Width - 20f) / (float)Draw.DefaultSizeStr.graphicWidth;
            pathMatrix.setWidthScale(widthScale);
            pathMatrix.setOffsetX(0);
            pathMatrix.setOffsetY(0);
            this.panelGraph.Invalidate();
        }
        /// <summary>
        /// 全站模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAllStation_Click(object sender, EventArgs e)
        {
            float heihtScale = (float)(this.panelGraph.Height - this.picTimeUp.Height + 10f) / (float)Draw.DefaultSizeStr.graphicHeight;
            pathMatrix.setHeightScale(heihtScale);
            this.panelGraph.Invalidate();
        }
        /// <summary>
        /// 是否显示panelZoom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowNavOrNot_Click(object sender, EventArgs e)
        {
          
            //this.panelZoom.Location = new Point(this.vScrollGraph.Location.X - this.panelZoom.Width - 2, this.panelHScrollBottom.Location.Y - this.panelZoom.Height - 2);

        }
        #endregion"结束 图形移动与缩放"

        #region"时刻精度 与 列车显示"
        /// <summary>
        /// 十分的精度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbShowTenM_Click(object sender, EventArgs e)
        {
            displayStr.displayPrecision = 10;
            uncheckedTsbShow();
            this.tsbShowTenM.Checked = true;
            this.panelGraph.Invalidate();
        }
        /// <summary>
        /// 两分的精度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbShowTwoM_Click(object sender, EventArgs e)
        {
            displayStr.displayPrecision = 2;
            uncheckedTsbShow();
            this.tsbShowTwoM.Checked = true;
            this.panelGraph.Invalidate();
        }
        /// <summary>
        /// 三十分精度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbShowThirtyM_Click(object sender, EventArgs e)
        {
            displayStr.displayPrecision = 30;
            uncheckedTsbShow();
            this.tsbShowThirtyM.Checked = true;
            this.panelGraph.Invalidate();
        }
        /// <summary>
        /// 所有的精度按钮 置false
        /// </summary>
        private void uncheckedTsbShow()
        {
            this.tsbShowTwoM.Checked = false;
            this.tsbShowThirtyM.Checked = false;
            this.tsbShowTenM.Checked = false;
        }

        /// ///////////////////////////////////////////////////显示上下行

        private void tsbShowUpAndDown_Click(object sender, EventArgs e)
        {
            displayStr.displayDirection = "upAndDown";
            uncheckedTsbDirectionShow();
            this.tsbShowUpAndDown.Checked = true;
            this.panelGraph.Invalidate();

        }

        private void tsbShowDown_Click(object sender, EventArgs e)
        {
            displayStr.displayDirection = "down";
            uncheckedTsbDirectionShow();
            this.tsbShowDown.Checked = true;
            this.panelGraph.Invalidate();

        }

        private void tsbShowUp_Click(object sender, EventArgs e)
        {
            displayStr.displayDirection = "up";
            uncheckedTsbDirectionShow();
            this.tsbShowUp.Checked = true;
            this.panelGraph.Invalidate();
        }
        /// <summary>
        /// 所有上下行显示 置false
        /// </summary>
        private void uncheckedTsbDirectionShow()
        {
            this.tsbShowUpAndDown.Checked = false;
            this.tsbShowDown.Checked = false;
            this.tsbShowUp.Checked = false;
        }

        /// ///////////////////////////////////////////////////显示/隐藏等级列车
        private void tsbShowRankA_Click(object sender, EventArgs e)
        {
            if (displayStr.bRankA)
            {
                this.tsbShowRankA.Image = global::MvrpLite.Properties.Resources.unSelect;
                displayStr.bRankA = false;
            }
            else
            {
                this.tsbShowRankA.Image = global::MvrpLite.Properties.Resources.select;
                displayStr.bRankA = true;
            }
            this.panelGraph.Invalidate();
        }

        private void tsbShowRankB_Click(object sender, EventArgs e)
        {
            if (displayStr.bRankB)
            {
                this.tsbShowRankB.Image = global::MvrpLite.Properties.Resources.unSelect;
                displayStr.bRankB = false;
            }
            else
            {
                this.tsbShowRankB.Image = global::MvrpLite.Properties.Resources.select;
                displayStr.bRankB = true;
            }
            this.panelGraph.Invalidate();
        }

        private void tsbShowRankC_Click(object sender, EventArgs e)
        {
            if (displayStr.bRankC)
            {
                this.tsbShowRankC.Image = global::MvrpLite.Properties.Resources.unSelect;
                displayStr.bRankC = false;
            }
            else
            {
                this.tsbShowRankC.Image = global::MvrpLite.Properties.Resources.select;
                displayStr.bRankC = true;
            }
            this.panelGraph.Invalidate();
        }
        /// <summary>
        /// 显示/隐藏车次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbShowTrainID_Click(object sender, EventArgs e)
        {
            if (displayStr.bTrainWord)
            {
                this.tsbShowTrainID.Image = global::MvrpLite.Properties.Resources.unSelect;
                displayStr.bTrainWord = false;
            }
            else
            {
                this.tsbShowTrainID.Image = global::MvrpLite.Properties.Resources.select;
                displayStr.bTrainWord = true;
            }
            this.panelGraph.Invalidate();
        }
        /// ///////////////////////////////////////////////////显示/隐藏时间车站窗口
        private void tsmTimeUp_Click(object sender, EventArgs e)
        {
            if (chkTimeStation[0])
            {
                this.tsmTimeUp.Image = global::MvrpLite.Properties.Resources.timeStationUp0;
                chkTimeStation[0] = false;
                Draw.DrawConf.reconfDefaultSize(m_total_runTime, 0, 1);
            }
            else
            {
                this.tsmTimeUp.Image = global::MvrpLite.Properties.Resources.timeStationUp1;
                chkTimeStation[0] = true;
                Draw.DrawConf.reconfDefaultSize(m_total_runTime,40, 1);
            }
            this.tsbTimeStation.Image = getTimeStationGif();
            this.panelGraph.Invalidate();
        }

        private void tsmStationRight_Click(object sender, EventArgs e)
        {
            if (chkTimeStation[1])
            {
                this.tsmStationRight.Image = global::MvrpLite.Properties.Resources.timeStationRight0;
                chkTimeStation[1] = false;
                Draw.DrawConf.reconfDefaultSize(m_total_runTime,0, 2);
            }
            else
            {
                this.tsmStationRight.Image = global::MvrpLite.Properties.Resources.timeStationRight1;
                chkTimeStation[1] = true;
                Draw.DrawConf.reconfDefaultSize(m_total_runTime,90, 2);
            }
            this.tsbTimeStation.Image = getTimeStationGif();
            this.panelGraph.Invalidate();
        }

        private void tsmTimeDown_Click(object sender, EventArgs e)
        {
            if (chkTimeStation[2])
            {
                this.tsmTimeDown.Image = global::MvrpLite.Properties.Resources.timeStationDown0;
                chkTimeStation[2] = false;
                Draw.DrawConf.reconfDefaultSize(m_total_runTime,0, 3);
            }
            else
            {
                this.tsmTimeDown.Image = global::MvrpLite.Properties.Resources.timeStationDown1;
                chkTimeStation[2] = true;
                Draw.DrawConf.reconfDefaultSize(m_total_runTime,40, 3);
            }
            this.tsbTimeStation.Image = getTimeStationGif();
            this.panelGraph.Invalidate();
        }

        private void tsmStationLeft_Click(object sender, EventArgs e)
        {
            if (chkTimeStation[3])
            {
                this.tsmStationLeft.Image = global::MvrpLite.Properties.Resources.timeStationLeft0;
                chkTimeStation[3] = false;
                Draw.DrawConf.reconfDefaultSize(m_total_runTime,0, 4);
            }
            else
            {
                this.tsmStationLeft.Image = global::MvrpLite.Properties.Resources.timeStationLeft1;
                chkTimeStation[3] = true;
                Draw.DrawConf.reconfDefaultSize(m_total_runTime,90, 4);
            }
            this.tsbTimeStation.Image = getTimeStationGif();
            this.panelGraph.Invalidate();
        }
        #endregion"结束 时刻精度 与 列车显示"

        private void tsbSetSimu_Click(object sender, EventArgs e)
        {
            inputData();
            resetDataForDarw();

            Draw_Frame = new DrawFrame(m_all_nodes_name, m_all_nodes_y, m_set_s);
            Draw_Trains = new DrawTrains();
            drawConf();
            initDrawTSB();        ////对绘图工具栏的初始化

            this.panelGraph.Invalidate();
        }

    }
}
