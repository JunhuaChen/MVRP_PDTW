using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MvrpLite.data;
using System.Diagnostics;
using ZedGraph;

//using Microsoft.SolverFoundation.Common;
//using Microsoft.SolverFoundation.Services;
namespace MvrpLite.adminFuc
{
    public partial class panelRealData : UserControl
    {
        private string m_path;
        private DataTable m_dtNode;
        private DataTable m_dtAgent;
        private DataTable m_dtConfiguration;

        private DataTable m_dtRoute_Sequence;
        /// /////////////////////
        private double[] g_node_x;
        private double[] g_node_y;
        Dictionary<string, int> g_exteral_to_internal_node = new Dictionary<string, int>();
        Dictionary<int, string> g_internal_to_exteral_node = new Dictionary<int, string>();

        #region"初始化"
        public panelRealData()
        {
            InitializeComponent();
            this.tabControlData.SelectedIndex = 0;
            m_path =  Directory.GetCurrentDirectory() + @"\";
            inputData();
            call_mvrpInit_distance_vp();
        }
        public void setTabControlSelectedIndex(int selectIndex)
        {
            this.tabControlData.SelectedIndex = selectIndex;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelRealData_Load(object sender, EventArgs e)
        {
            this.m_dtRoute_Sequence = new DataTable();
            this.m_dtRoute_Sequence.Columns.Add("v_active", System.Type.GetType("System.String"));
            this.m_dtRoute_Sequence.Columns.Add("v_ori", System.Type.GetType("System.String"));
            this.m_dtRoute_Sequence.Columns.Add("group", System.Type.GetType("System.String"));
            this.m_dtRoute_Sequence.Columns.Add("p", System.Type.GetType("System.String"));
            this.m_dtRoute_Sequence.Columns.Add("time", System.Type.GetType("System.String"));
            this.m_dtRoute_Sequence.Columns.Add("node", System.Type.GetType("System.String"));
            this.m_dtRoute_Sequence.Columns.Add("cost", System.Type.GetType("System.String"));
            this.m_dtRoute_Sequence.Columns.Add("accumulative_cost", System.Type.GetType("System.String"));

        }

        private void inputData()
        {
            string fileName = m_path + "input_agent.csv";//文件路径
            m_dtAgent = new DataTable();
            m_dtAgent = get_data_from_CSV(fileName);
            this.dgvAgent.DataSource = m_dtAgent.DefaultView;


            fileName = m_path + "input_node.csv";//文件路径
            m_dtNode = get_data_from_CSV(fileName);
            this.dgvNode.DataSource = m_dtNode.DefaultView;


            fileName = m_path + "input_configuration.csv";//文件路径
            m_dtConfiguration = new DataTable();
            m_dtConfiguration = get_data_from_CSV(fileName);
            this.dgvConfiguration.DataSource = m_dtConfiguration.DefaultView;

            this.txbNodeNumber.Text = this.m_dtNode.Rows.Count.ToString();

            int pCount = 0, vCount = 0;
            foreach(DataRow dr in m_dtAgent.Rows)
            {
                if (dr["agent_type"].ToString() == "0") pCount++;
                else if (dr["agent_type"].ToString() == "1") vCount++;
            }
            this.txbPNum.Text = pCount.ToString();
            this.txbVNumber.Text = vCount.ToString();
            //////////////////////////////////////////////////////////

            this.g_node_x=new double[m_dtNode.Rows.Count];
            this.g_node_y=new double[m_dtNode.Rows.Count];
            int nCount=0;
            g_exteral_to_internal_node.Clear();
            g_internal_to_exteral_node.Clear();
            foreach(DataRow dr in m_dtNode.Rows)
            {
               
                string node_id = dr["node_id"].ToString();
                double x = double.Parse(dr["x"].ToString());
                double y = double.Parse(dr["y"].ToString());
                g_exteral_to_internal_node.Add(node_id,nCount);
                g_internal_to_exteral_node.Add(nCount,node_id);
                g_node_x[nCount] = x;
                g_node_y[nCount] = y; 
                nCount++;
            }

          
        }
   
        private void call_mvrpInit_distance_vp()
        {
           
            Process myProcess = new Process();
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = m_path + "MvrpInit.exe";//文件路径 "MvrpInit.exe";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                myProcess.Start();
                myProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public DataTable get_data_from_CSV(string pCsvPath)
        {
            String line;
            String[] split = null;String[] split_new=null;
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
                ///////for input.node   colName:geometry
                if (pCsvPath == m_path+"input_node.csv" && split.Length==11)
                {
                    split_new = new string[split.Length - 1];
                    for (int i = 0; i <= 8; i++) split_new[i] = split[i];
                    split_new[9] = split[9] + "," + split[10];
                }
                else split_new = split;
                //split_new = split;
                //////
                foreach (String colname in split_new)
                {
                    row[j] = colname;
                    j++;
                }
                dtInfo.Rows.Add(row);
            }
            sr.Close();

            return dtInfo;
        }
        public DataTable get_data_from_CSV(string pCsvPath,string colNameA,string colNameB,int[,] colValue)
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
                foreach (String colvalue in split)
                {
                    row[j] = colvalue;
                    j++;
                }
                int g = int.Parse(row[colNameA].ToString());
                int s_index=int.Parse(row[colNameB].ToString());
                if(colValue[g,s_index]==1)
                    dtInfo.Rows.Add(row);
            }
            sr.Close();

            return dtInfo;
        }


        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtNewConfiguration = new DataTable();
            dtNewConfiguration = GetDgvToTable(this.dgvConfiguration);

            ///////////////////dgvConfiguration
            string fullPath = m_path + "input_configuration.csv";
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";
            //column name
            data += "max_number_of_threads_to_be_used";  data += ",max_group_number"; data += ",max_v_in_group"; data += ",max_p_in_group";
            data += ",vehicle_capacity"; data += ",min_finish_task_each_vehicle"; data += ",max_finish_task_each_vehicle"; data += ",memory_for_states"; data += ",gams_path";
            sw.WriteLine(data);


            foreach (DataRow dr in dtNewConfiguration.Rows)
            {
                data = "";
                for (int i = 0; i < dtNewConfiguration.Columns.Count - 1; i++)
                    data += dr[i].ToString() + ",";
                data += dr[dtNewConfiguration.Columns.Count - 1].ToString();
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();

            inputData();
            MessageBox.Show("success update the configuration data!");
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            bool b_run_model_1, b_run_model_2, b_run_model_3;
            b_run_model_1 = this.chk1.Checked; b_run_model_2 = this.chk2.Checked; b_run_model_3 = this.chk3.Checked;
       
            DataTable dtNewConfiguration = new DataTable();
            dtNewConfiguration = GetDgvToTable(this.dgvConfiguration);
            m_dtConfiguration = dtNewConfiguration;

            
                #region "step1: get data from trans_A_set_similarity_p_p.csv"

                string fileName = m_path + "trans_A_set_similarity_p_p.csv";//文件路径
                DataTable dtDpp = new DataTable();
                dtDpp = get_data_from_CSV(fileName);
                int m_num_v = int.Parse(dtDpp.Rows[0]["p1"].ToString());
                int m_num_p = int.Parse(dtDpp.Rows[0]["p2"].ToString());
                int m_num_g = m_num_p;
                int m_max_v_in_group = int.Parse(m_dtConfiguration.Rows[0]["max_v_in_group"].ToString());/////=4
                int m_max_p_in_group = int.Parse(m_dtConfiguration.Rows[0]["max_p_in_group"].ToString());/////=7
                int m_max_group_number = int.Parse(m_dtConfiguration.Rows[0]["max_group_number"].ToString());/////=2
                //int MAX_COST = 9999;

                float[,] cost_pg_value = new float[m_num_p + 1, m_num_g + 1];
                foreach (DataRow dr in dtDpp.Rows)
                {
                    int p1 = int.Parse(dr["p1"].ToString());
                    int p2 = int.Parse(dr["p2"].ToString());
                    int value = (int)float.Parse(dr["value"].ToString());
                    if (value == -1) continue;
                    cost_pg_value[p1, p2] = value;

                }

                #endregion

            if (b_run_model_1)
            {

                #region "step2.1: generate file for gams_group_p_input"

                string fullPath = m_path + @"gams_group_p_input.txt";

                FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                /////////////////create general codes
                string data = "";
                data = @"set g /1*" + m_num_g.ToString() + @"/;";
                sw.WriteLine(data);

                data = @"set p /1*" + m_num_p.ToString() + @"/;";
                sw.WriteLine(data);

                data = @"scalar max_p /" + m_max_p_in_group.ToString() + @"/;";
                sw.WriteLine(data);

                data = @"scalar max_g /" + m_max_group_number.ToString() + @"/;";
                sw.WriteLine(data);
                ////////////////////////////////////
                data = @"parameter c(p,g) /";
                sw.WriteLine(data);

                for (int p = 1; p <= m_num_p; p++)
                    for (int g = 1; g <= m_num_g; g++)
                    {
                        data = p.ToString() + @"." + g.ToString() + @" " + cost_pg_value[p, g].ToString();
                        sw.WriteLine(data);
                    }

                data = @"/;";
                sw.WriteLine(data);

                sw.Close();
                fs.Close();

                #endregion

                #region "step2.2: Run gams_exe_group_p and get results"
                ////run gams
                RunGamsModel("gams_exe_group_p");
                ////read result
                int[,] X_pg = new int[m_num_p + 1, m_num_g + 1];
                for (int p = 1; p <= m_num_p; p++)
                    for (int g = 1; g <= m_num_g; g++)
                        X_pg[p, g] = 0;

                fileName = m_path + @"gams_group_p_output.txt";//文件路径
                StreamReader din = File.OpenText(fileName);
                string oneStr = string.Empty;


                while ((oneStr = din.ReadLine()) != null)
                {
                    string[] strLines = oneStr.ToString().Split(' ');
                    string[] strLinesValue = new string[3];
                    int nValue = 0;
                    for (int i = 0; i < strLines.Length; i++)
                    {
                        if (strLines[i] != "")
                        {
                            strLinesValue[nValue] = strLines[i]; nValue++;
                        }
                    }

                    int p_in = int.Parse(strLinesValue[0]);
                    int g_in = int.Parse(strLinesValue[1]);
                    //int x_pg_in = int.Parse(strLinesValue[2]);
                    X_pg[p_in, g_in] = 1;
                }
                din.Close();


                int m_num_g_new = 0;
                int[] map_gnew_g = new int[m_num_g + 1]; map_gnew_g[0] = 0;
                for (int g = 1; g <= m_num_g; g++)
                    for (int p = 1; p <= m_num_p; p++)
                    {
                        if (X_pg[p, g] == 1)
                        {
                            m_num_g_new++;
                            map_gnew_g[++map_gnew_g[0]] = g;
                            break;
                        }
                    }
                #endregion

                #region "step3.1: computer cost_pv_value[v, g] for grouping v"

                int[,] group_g_p = new int[m_num_g_new + 1, m_num_p + 1];
                for (int g = 1; g <= map_gnew_g[0]; g++)
                {
                    for (int p = 1; p <= m_num_p; p++)
                    {
                        int g_ori = map_gnew_g[g];
                        group_g_p[g, p] = (int)X_pg[p, g_ori];
                    }
                }

                ////////////read xyt
                fileName = m_path + "trans_A_set_xyt.csv";//文件路径
                DataTable dtXYT = new DataTable();
                dtXYT = get_data_from_CSV(fileName);

                float[] p_o_x = new float[m_num_p + 1];
                float[] p_o_y = new float[m_num_p + 1];
                float[] p_o_t = new float[m_num_p + 1];
                float[] p_d_x = new float[m_num_p + 1];
                float[] p_d_y = new float[m_num_p + 1];
                float[] p_d_t = new float[m_num_p + 1];

                float[] v_o_x = new float[m_num_v + 1];
                float[] v_o_y = new float[m_num_v + 1];
                float[] v_o_t = new float[m_num_v + 1];
                float[] v_d_x = new float[m_num_v + 1];
                float[] v_d_y = new float[m_num_v + 1];
                float[] v_d_t = new float[m_num_v + 1];
                foreach (DataRow dr in dtXYT.Rows)
                {
                    string type = dr["type"].ToString();
                    int p = int.Parse(dr["p/v"].ToString());
                    int v = int.Parse(dr["p/v"].ToString());
                    if (type == "p")
                    {
                        p_o_x[p] = float.Parse(dr["o_x"].ToString());
                        p_o_y[p] = float.Parse(dr["o_y"].ToString());
                        p_o_t[p] = float.Parse(dr["o_t"].ToString());
                        p_d_x[p] = float.Parse(dr["d_x"].ToString());
                        p_d_y[p] = float.Parse(dr["d_y"].ToString());
                        p_d_t[p] = float.Parse(dr["d_t"].ToString());
                    }
                    else if (type == "v")
                    {
                        v_o_x[v] = float.Parse(dr["o_x"].ToString());
                        v_o_y[v] = float.Parse(dr["o_y"].ToString());
                        v_o_t[v] = float.Parse(dr["o_t"].ToString());
                        v_d_x[v] = float.Parse(dr["d_x"].ToString());
                        v_d_y[v] = float.Parse(dr["d_y"].ToString());
                        v_d_t[v] = float.Parse(dr["d_t"].ToString());
                    }
                }
                ///////genearl virtual xyt
                float[] g_o_x = new float[m_num_g_new + 1];
                float[] g_o_y = new float[m_num_g_new + 1];
                float[] g_o_t = new float[m_num_g_new + 1];
                float[] g_d_x = new float[m_num_g_new + 1];
                float[] g_d_y = new float[m_num_g_new + 1];
                float[] g_d_t = new float[m_num_g_new + 1];

                for (int g = 1; g <= m_num_g_new; g++)
                {
                    int p_count = 0;
                    g_o_x[g] = g_o_y[g] = g_o_t[g] = g_d_x[g] = g_d_y[g] = g_d_t[g] = 0f;
                    for (int p = 1; p <= m_num_p; p++)
                    {
                        if (group_g_p[g, p] == 1)
                        {
                            p_count++;
                            g_o_x[g] += p_o_x[p]; g_o_y[g] += p_o_y[p]; g_o_t[g] += p_o_t[p];
                            g_d_x[g] += p_d_x[p]; g_d_y[g] += p_d_y[p]; g_d_t[g] += p_d_t[p];
                        }
                    }
                    g_o_x[g] = g_o_x[g] / p_count; g_o_y[g] = g_o_y[g] / p_count; g_o_t[g] = g_o_t[g] / p_count;
                    g_d_x[g] = g_d_x[g] / p_count; g_d_y[g] = g_d_y[g] / p_count; g_d_t[g] = g_d_t[g] / p_count;
                }
                /////////////////////////
                float[,] cost_vg_value = new float[m_num_v + 1, m_num_g_new + 1];

                for (int v = 1; v <= m_num_v; v++)
                {
                    for (int g = 1; g <= m_num_g_new; g++)
                    {
                        double similarity_o = Math.Sqrt((v_o_x[v] - g_o_x[g]) * (v_o_x[v] - g_o_x[g]) + (v_o_y[v] - g_o_y[g]) * (v_o_y[v] - g_o_y[g]) + (v_o_t[v] - g_o_t[g]) * (v_o_t[v] - g_o_t[g]));
                        double similarity_d = Math.Sqrt((v_d_x[v] - g_d_x[g]) * (v_d_x[v] - g_d_x[g]) + (v_d_y[v] - g_d_y[g]) * (v_d_y[v] - g_d_y[g]) + (v_d_t[v] - g_d_t[g]) * (v_d_t[v] - g_d_t[g]));

                        cost_vg_value[v, g] = (float)Math.Max(similarity_o, similarity_d);
                    }
                }
                #endregion

                #region "step3.2:generate file gams_group_v_input.txt"

                ///////////////////// 
                fullPath = m_path + @"gams_group_v_input.txt";

                fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                /////////////////create general codes
                data = "";
                data = @"set g /1*" + m_num_g_new.ToString() + @"/;";
                sw.WriteLine(data);

                data = @"set v /1*" + m_num_v.ToString() + @"/;";
                sw.WriteLine(data);

                data = @"scalar max_v /" + m_max_v_in_group.ToString() + @"/;";
                sw.WriteLine(data);

                data = @"scalar group_number /" + m_max_group_number.ToString() + @"/;";
                sw.WriteLine(data);
                ////////////////////////////////////
                data = @"parameter p_number(g) /";
                sw.WriteLine(data);

                for (int g = 1; g <= m_num_g_new; g++)
                {
                    int p_number = 0;
                    for (int p = 1; p <= m_num_p; p++)
                    {
                        if (group_g_p[g, p] == 1) p_number++;
                    }
                    data = g.ToString() + @" " + p_number.ToString();
                    sw.WriteLine(data);

                }

                data = @"/;";
                sw.WriteLine(data);

                ////////////
                data = @"parameter c(v,g) /";
                sw.WriteLine(data);

                for (int v = 1; v <= m_num_v; v++)
                    for (int g = 1; g <= m_num_g_new; g++)
                    {
                        data = v.ToString() + @"." + g.ToString() + @" " + cost_vg_value[v, g].ToString();
                        sw.WriteLine(data);
                    }

                data = @"/;";
                sw.WriteLine(data);

                sw.Close();
                fs.Close();


                #endregion

                #region "step3.3: Run gams_exe_group_v and get results"
                ////run gams
                RunGamsModel("gams_exe_group_v");
                ////read result
                int[,] X_vg = new int[m_num_v + 1, m_num_g_new + 1];
                for (int v = 1; v <= m_num_v; v++)
                    for (int g = 1; g <= m_num_g_new; g++)
                        X_vg[v, g] = 0;

                fileName = m_path + "gams_group_v_output.txt";//文件路径
                din = File.OpenText(fileName);
                oneStr = string.Empty;


                while ((oneStr = din.ReadLine()) != null)
                {
                    string[] strLines = oneStr.ToString().Split(' ');
                    string[] strLinesValue = new string[3];
                    int nValue = 0;
                    for (int i = 0; i < strLines.Length; i++)
                    {
                        if (strLines[i] != "")
                        {
                            strLinesValue[nValue] = strLines[i]; nValue++;
                        }
                    }

                    int v_in = int.Parse(strLinesValue[0]);
                    int g_in = int.Parse(strLinesValue[1]);
                    //int x_pg_in = int.Parse(strLinesValue[2]);
                    X_vg[v_in, g_in] = 1;
                }
                din.Close();



                int[,] group_g_v = new int[m_num_g_new + 1, m_num_v + 1];
                for (int g = 1; g <= m_num_g_new; g++)
                {
                    for (int v = 1; v <= m_num_v; v++)
                    {
                        group_g_v[g, v] = (int)X_vg[v, g];
                    }
                }


                #endregion

                #region "step3.4: generate trans file   trans_partition_g_v_p.csv  "

                string str_test = "";
                for (int g = 1; g <= m_num_g_new; g++)
                {
                    str_test += "\n group:" + g.ToString() + "\n";
                    for (int v = 1; v <= m_num_v; v++)
                    {
                        if (group_g_v[g, v] == 1)
                            str_test += " v" + v.ToString();
                    }
                    str_test += "\n";

                    for (int p = 1; p <= m_num_p; p++)
                    {
                        if (group_g_p[g, p] == 1)
                            str_test += " p" + p.ToString();
                    }
                    str_test += "\n";
                }
                MessageBox.Show(str_test);


                fullPath = m_path + "trans_B_partition_g_v_p.csv";
                FileInfo fi = new FileInfo(fullPath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                data = "";
                //column name
                data += "g"; data += ",v"; data += ",p";
                sw.WriteLine(data);

                for (int g = 1; g <= m_num_g_new; g++)
                {
                    for (int v = 1; v <= m_num_v; v++)
                    {
                        if (group_g_v[g, v] == 1)
                        {
                            data = g.ToString() + "," + v.ToString() + ",-1";
                            sw.WriteLine(data);
                        }
                    }
                    for (int p = 1; p <= m_num_p; p++)
                    {
                        if (group_g_p[g, p] == 1)
                        {
                            data = g.ToString() + ",-1," + p.ToString();
                            sw.WriteLine(data);
                        }
                    }
                }
                sw.Close();
                fs.Close();

                #endregion

            }

            if (b_run_model_2)
            {

                #region "step4: call mvrpPlus.exe in c++"

                Process myProcess = new Process();
                try
                {
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.FileName = m_path + "MvrpPlus.exe";//文件路径 "MvrpPlus.exe";
                    //myProcess.StartInfo.CreateNoWindow = true;
                    //myProcess.StartInfo.RedirectStandardInput = true;//重定向输入  
                    //myProcess.StartInfo.RedirectStandardOutput = true;//重定向输出  
                    //myProcess.StartInfo.RedirectStandardError = true;//重定向输出错误    
                    myProcess.Start();
                    myProcess.WaitForExit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
           

                #endregion

            }

            if (b_run_model_3)
            {

                #region "step5.1: read trans_C_vehicle_connection_value.csv"

                fileName = m_path + "trans_C_vehicle_connection_value.csv";//文件路径
                DataTable dtParameter = new DataTable();
                dtParameter = get_data_from_CSV(fileName);
                int[] active_u_list = new int[m_num_v + 1];
                for (int i = 0; i < active_u_list.Length; i++) active_u_list[i] = 0;
                int[,] cost_uv = new int[m_num_v + 1, m_num_v + 1];

                foreach (DataRow dr in dtParameter.Rows)
                {
                    int u = int.Parse(dr["u"].ToString());
                    int v = int.Parse(dr["v"].ToString());
                    bool isRepeatU = false, isRepeatV = false;
                    for (int i = 1; i <= active_u_list[0]; i++)
                    {
                        if (active_u_list[i] == u) isRepeatU = true;
                        if (active_u_list[i] == v) isRepeatV = true;
                    }
                    if (!isRepeatU) active_u_list[++active_u_list[0]] = u;
                    if (!isRepeatV) active_u_list[++active_u_list[0]] = v;

                    int cost = int.Parse(dr["travelTime"].ToString());
                    cost_uv[u, v] = cost;
                }



                #endregion

                #region "step5.2: generate file gams_vehicle_connection_input.txt"
                string fullPath = m_path + @"gams_vehicle_connection_input.txt";

                FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                /////////////////create general codes

                string str_u = "";
                for (int i = 1; i < active_u_list[0]; i++)
                    str_u += active_u_list[i].ToString() + ",";
                str_u += active_u_list[active_u_list[0]].ToString();

                string data = "";
                data = @"set u /" + str_u + @"/;";
                sw.WriteLine(data);

                data = @"scalar M /1000/;";
                sw.WriteLine(data);

                data = @"scalar Infinite /99999/;";
                sw.WriteLine(data);
                ////////////////////////////////////
                data = @"parameter c(u,v) /";
                sw.WriteLine(data);

                for (int i = 1; i <= active_u_list[0]; i++)
                    for (int j = 1; j <= active_u_list[0]; j++)
                    {
                        int u = active_u_list[i]; int v = active_u_list[j];
                        int real_value = 0;
                        if (cost_uv[u, v] != 0) real_value = cost_uv[u, v];
                        else real_value = 99999;
                        if (u == v) real_value = 1000;

                        data = u.ToString() + @"." + v.ToString() + @" " + real_value.ToString();
                        sw.WriteLine(data);
                    }

                data = @"/;";
                sw.WriteLine(data);

                sw.Close();
                fs.Close();

                #endregion


                #region "step5.3: Run gams_exe_vehicle_connection and get results"
                ////run gams
                RunGamsModel("gams_exe_vehicle_connection");
                ////read result
                int[,] X_uv = new int[m_num_v + 1, m_num_v + 1];
                for (int u = 1; u <= m_num_v; u++)
                    for (int v = 1; v <= m_num_v; v++)
                        X_uv[u, v] = 0;

                fileName = m_path + "gams_vehicle_connection_output.txt";//文件路径
                StreamReader din = File.OpenText(fileName);
                string oneStr = string.Empty;


                while ((oneStr = din.ReadLine()) != null)
                {
                    string[] strLines = oneStr.ToString().Split(' ');
                    string[] strLinesValue = new string[3];
                    int nValue = 0;
                    for (int i = 0; i < strLines.Length; i++)
                    {
                        if (strLines[i] != "")
                        {
                            strLinesValue[nValue] = strLines[i]; nValue++;
                        }
                    }

                    int u_in = int.Parse(strLinesValue[0]);
                    int v_in = int.Parse(strLinesValue[1]);
                    X_uv[u_in, v_in] = 1;
                }
                din.Close();

                //////
                int[,] u_sequence = new int[m_num_v + 1, m_num_v + 1];
                for (int u = 0; u <= m_num_v; u++)
                    for (int v = 0; v <= m_num_v; v++)
                        u_sequence[u, v] = 0;

                for (int u = 1; u <= m_num_v; u++)
                {
                    if (X_uv[u, u] == 1)
                    {
                        u_sequence[0, 0]++;///number of sequence 
                        u_sequence[u_sequence[0, 0], 0]++;
                        u_sequence[u_sequence[0, 0], 1] = u;
                    }
                }


                for (int sequence_index = 1; sequence_index <= u_sequence[0, 0]; sequence_index++)
                {
                    int u_sequence_start = u_sequence[sequence_index, 1];
                    int u_next = u_sequence_start;
                    while (u_next != -1)
                    {
                        u_next = -1;
                        for (int v = 1; v <= m_num_v; v++)
                        {
                            if (X_uv[u_sequence_start, v] == 1 && u_sequence_start != v) { u_next = v; break; }
                        }
                        if (u_next != -1)
                        {
                            u_sequence[sequence_index, ++u_sequence[sequence_index, 0]] = u_next;
                            u_sequence_start = u_next;
                        }
                    }
                }


                #endregion

           

                #region "6.1 generate connection routes from trans_C_file_all_routes.csv && trans_C_vehicle_connection_route.csv"
                ///////////////////routes
                fileName = m_path + "trans_C_file_all_routes.csv";//文件路径
                DataTable dt_route_v_self = get_data_from_CSV(fileName);
           
                fileName = m_path + "trans_C_vehicle_connection_route.csv";//文件路径
                DataTable dt_route_v_connection = get_data_from_CSV(fileName);

                this.m_dtRoute_Sequence.Clear();

                for (int i = 1; i <= u_sequence[0, 0]; i++)
                {
                    for(int j=1;j<=u_sequence[i, 0];j++)
                    {
                        int u = u_sequence[i, j];
                        string show_route_type="";
                        if (u_sequence[i, 0] == 1) show_route_type = "all";
                        else
                        {
                            if (j == 1) show_route_type = "start";
                            else
                            {
                                if (j == u_sequence[i, 0]) show_route_type = "end";
                                else show_route_type = "middle";
                            }
                        }
                        ///////////
                        print_one_vehicle_route(i,u, show_route_type, dt_route_v_self);
                        ////check connection
                        int u_next = -1;
                        if ( j + 1<=u_sequence[i, 0] && u_sequence[i, j + 1] != 0) u_next = u_sequence[i, j + 1];
                        if(u_next!=-1)
                        {
                            print_connection_vehicle_route(i,u, u_next, dt_route_v_connection);
                        }
                    }
                }

                this.dgvRoute.DataSource = this.m_dtRoute_Sequence;

                #endregion

                #region "6.2 output_solution_route.csv  based on m_dtRoute_Sequence"

                ///////output_solution_route.csv  based on m_dtRoute_Sequence
                fullPath = m_path + "output_solution_route.csv";
                FileInfo fi = new FileInfo(fullPath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                data = "";
                //column name
                data += "v_active"; data += ",v_ori"; data += ",group"; data += ",p";
                data += ",time"; data += ",node"; data += ",cost"; data += ",accumulative_cost";
                sw.WriteLine(data);



                foreach (DataRow dr in m_dtRoute_Sequence.Rows)
                {
                    data = "";
                    for (int i = 0; i < m_dtRoute_Sequence.Columns.Count - 1; i++)
                        data += dr[i].ToString() + ",";
                    data += dr[m_dtRoute_Sequence.Columns.Count - 1].ToString();
                    sw.WriteLine(data);
                }
                sw.Close();
                fs.Close();

                #endregion 

                #region"7.1 dgvDecisionX.DataSource = dtX   && 7.2 show figure"
                //////////////////////
                int[,] p_sequence = new int[m_num_v + 1, m_num_p + 1];
                foreach (DataRow dr in m_dtRoute_Sequence.Rows)
                {
                    int u = int.Parse(dr["v_active"].ToString());
                    string str_p = dr["p"].ToString();
                    if (str_p.Length > 2) continue;
                    if (str_p == "0") continue;
                    p_sequence[u, ++p_sequence[u, 0]] = int.Parse(str_p);
                }


                /////////////////
                DataTable dtX = new DataTable();
                DataRow row = null;
                dtX.Columns.Add("v_active", System.Type.GetType("System.String"));
                dtX.Columns.Add("v_sequence", System.Type.GetType("System.String"));
                dtX.Columns.Add("p_sequence", System.Type.GetType("System.String"));
                row = dtX.NewRow();
                row[0] = "all"; row[1] = "all"; row[2] = "1*" + m_num_p.ToString();
                dtX.Rows.Add(row);
                for (int i = 1; i <= u_sequence[0, 0]; i++)
                {
                    row = dtX.NewRow();
                    row[0] = i.ToString();
                    string str_u_sequence = "";
                    for (int j = 1; j <= u_sequence[i, 0]; j++)
                    {
                        int u = u_sequence[i, j];
                        str_u_sequence += u.ToString() + "-";

                    }
                    row[1] = str_u_sequence.ToString();

                    string str_p_sequence = "";
                    for (int j = 1; j <= p_sequence[i, 0]; j++)
                    {
                        int p = p_sequence[i, j];
                        str_p_sequence += p.ToString() + "-";
                    }
                    row[2] = str_p_sequence.ToString();

                    dtX.Rows.Add(row);
                }  
               
                this.dgvDecisionX.DataSource = dtX;


                

                 /////7.2 show figure"
                //ZedGraph.ZedGraphControl 
                 
                draw_figure("all");
                this.tabControlData.SelectedIndex = 1;

                #endregion

                #region "8 output__route.csv  based on m_dtRoute_Sequence"

                ///////output_solution_route.csv  based on m_dtRoute_Sequence
                fullPath = m_path + "output_cumulative_flow_data.csv";
                fi = new FileInfo(fullPath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                data = "";
                //column name
                data += "t"; data += ",A(t):Demand"; data += ",A(t):Supply"; data += ",D(t):Supply";
                data += ",Cumulative cost"; 
                sw.WriteLine(data);


                int[] event_demand=new int[1000];/////到达事件系列
                int[] event_pickup = new int[1000];////pick up 事件系列
                int[] event_deliver = new int[1000];////deliver 事件系列

                foreach (DataRow dr in m_dtAgent.Rows)
                {
                    if (dr["agent_type"].ToString() == "1") continue;
                    int departure_time_start = int.Parse(dr["departure_time_start"].ToString());
                    event_demand[departure_time_start] = event_demand[departure_time_start] + 1;
                }


                int[] active_pickup_time = new int[500];
                int[] active_deliver_time = new int[500];
                foreach (DataRow dr in m_dtRoute_Sequence.Rows)
                {
                    if (dr["group"].ToString() == "") continue;
                    if (dr["p"].ToString() == "0") continue;
                    int event_p = int.Parse(dr["p"].ToString());

                    if(active_pickup_time[event_p]==0)
                        active_pickup_time[event_p] = int.Parse(dr["time"].ToString());
                    else
                        active_deliver_time[event_p] = int.Parse(dr["time"].ToString());
                }
                for(int i=1;i<500;i++)
                {
                    if (active_pickup_time[i] != 0)
                        event_pickup[active_pickup_time[i]] += 1;
                    if (active_deliver_time[i] != 0)
                        event_deliver[active_deliver_time[i]] += 1;
                }

                /////////////////////

                int n_demnad = 0, n_pickup = 0, n_deliver = 0;
                for(int t=0;t<1000;t++)
                {
                    if (event_demand[t] == 0 && event_pickup[t] == 0 && event_deliver[t] == 0) continue;
                    n_demnad += event_demand[t];
                    n_pickup += event_pickup[t];
                    n_deliver += event_deliver[t];
                    data = "";
                    data += t.ToString();
                    data += ","+ n_demnad.ToString(); data += ","+ n_pickup.ToString(); data += ","+ n_deliver.ToString();
                    data += ","+"0";
                    sw.WriteLine(data);


                }



                sw.Close();
                fs.Close();

                #endregion 



            }



        }


        private void dgvDecisionX_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string active_u = this.dgvDecisionX.CurrentRow.Cells[0].Value.ToString();
            draw_figure(active_u);


        }

        private void print_one_vehicle_route(int vehicle_active,int vehicle_number,string show_route_type,DataTable dt_route_in)
        {

            float accumulative_cost_start = 0.0f;
            if (this.m_dtRoute_Sequence.Rows.Count > 0) accumulative_cost_start = float.Parse(m_dtRoute_Sequence.Rows[m_dtRoute_Sequence.Rows.Count - 1]["accumulative_cost"].ToString());
            float base_cost = 0.0f;
            int nCount = 0;  
            foreach(DataRow dr in dt_route_in.Rows)
            {
                int v=int.Parse(dr["v"].ToString());
                string route_type = dr["route_type"].ToString();
                if (v != vehicle_number) continue;

                if(show_route_type=="start")///in this case ,show a+b+c0
                {
                    if (route_type != "a" && route_type != "b" && route_type != "c0") continue;
                }
                if (show_route_type == "middle")///in this case ,show b+c0
                {
                    if (route_type != "b" && route_type != "c0") continue;
                }
                if (show_route_type == "end")///in this case ,show b+c0+c
                {
                    if (route_type != "b" && route_type != "c0" && route_type != "c" ) continue;
                }

                nCount++;
                if (nCount == 1)
                {
                    base_cost = float.Parse(dr["cost"].ToString());
                    if (show_route_type == "middle" || show_route_type == "end") base_cost -= 1;
                }
                float accumulative_cost =accumulative_cost_start+ (float.Parse(dr["cost"].ToString())-base_cost);
                
                DataRow drNew = this.m_dtRoute_Sequence.NewRow();
                drNew["v_active"] = vehicle_active.ToString();
                drNew["v_ori"] = dr["v"]; drNew["group"] = dr["g"]; drNew["p"] = dr["p"];
                drNew["time"] = dr["time"]; drNew["node"] = dr["node"]; drNew["cost"] = (float.Parse(dr["cost"].ToString()) - base_cost).ToString(".00");
                drNew["accumulative_cost"] = accumulative_cost.ToString(".00");

                this.m_dtRoute_Sequence.Rows.Add(drNew);

            }
        }

        private void print_connection_vehicle_route(int vehicle_active,int vehicle_number_1,int vehicle_number_2, DataTable dt_route_in)
        {           
            float accumulative_cost_start = float.Parse(m_dtRoute_Sequence.Rows[m_dtRoute_Sequence.Rows.Count - 1]["accumulative_cost"].ToString());
            accumulative_cost_start += 1;
           
            foreach (DataRow dr in dt_route_in.Rows)
            {
                int u = int.Parse(dr["u"].ToString());
                int v = int.Parse(dr["v"].ToString());

                if (u == vehicle_number_1 && v == vehicle_number_2) 
                {

                   
                    float accumulative_cost = accumulative_cost_start + float.Parse(dr["cost"].ToString());                   

                    DataRow drNew = this.m_dtRoute_Sequence.NewRow();
                    drNew["v_active"] = vehicle_active.ToString(); drNew["group"] = "";
                    drNew["v_ori"] = dr["u"].ToString() + "->" + dr["v"].ToString();
                    drNew["p"] = dr["p1"].ToString() + "->" + dr["p2"].ToString();
                    drNew["time"] = dr["time"]; drNew["node"] = dr["node"]; drNew["cost"] = float.Parse(dr["cost"].ToString()).ToString(".00");
                    drNew["accumulative_cost"] = accumulative_cost.ToString(".00");

                    this.m_dtRoute_Sequence.Rows.Add(drNew);
                }

            }
        }


        private void draw_figure(string u_active)
        {
            //ZedGraph.ZedGraphControl 
            z1.GraphPane.CurveList.Clear();
            z1.IsShowPointValues = true;
            z1.GraphPane.Title.Text = "";
            z1.GraphPane.XAxis.Title.Text = "";
            z1.GraphPane.YAxis.Title.Text = "";

            z1.GraphPane.XAxis.Scale.Min = -5; z1.GraphPane.YAxis.Scale.Min = -5;
            z1.GraphPane.XAxis.IsVisible = false; z1.GraphPane.YAxis.IsVisible = false;

            z1.GraphPane.GraphObjList.Clear();

            LineItem myCurve = z1.GraphPane.AddCurve("Pos", g_node_x, g_node_y, Color.Red, SymbolType.Circle);
            myCurve.Line.IsVisible = false;

            for (int i = 0; i < myCurve.Points.Count; i++)
            {
                TextObj pointLabel = new TextObj(g_internal_to_exteral_node[i], myCurve.Points[i].X, myCurve.Points[i].Y + 2);
                pointLabel.FontSpec.Border.IsVisible = false;
                pointLabel.FontSpec.Size = 8;
                z1.GraphPane.GraphObjList.Add(pointLabel);
            }
            myCurve.Label.IsVisible = true;

            /////////////////////
            int[,] vehicle_sequence_node = new int[m_dtRoute_Sequence.Rows.Count, 200];
            foreach (DataRow dr in m_dtRoute_Sequence.Rows)
            {
                int v_in = int.Parse(dr["v_active"].ToString());
                int node_in = int.Parse(dr["node"].ToString());
                vehicle_sequence_node[v_in, ++vehicle_sequence_node[v_in, 0]] = node_in;
                vehicle_sequence_node[0, 0] = v_in;
            }


            for (int v = 1; v <= vehicle_sequence_node[0, 0]; v++)
            {
                if (u_active != "all" && int.Parse(u_active) != v) continue;
                double[] sequence_x = new double[vehicle_sequence_node[v, 0]];
                double[] sequence_y = new double[vehicle_sequence_node[v, 0]];
                int nCount = 0;
                int prev_node = -1;
                for (int k = 1; k <= vehicle_sequence_node[v, 0]; k++)
                {
                    int node_exteral = vehicle_sequence_node[v, k];
                    if (!g_exteral_to_internal_node.ContainsKey(node_exteral.ToString())) continue;
                    if (prev_node == node_exteral) continue;
                    prev_node = node_exteral;
                    int node_inner = g_exteral_to_internal_node[node_exteral.ToString()];
                    double x = g_node_x[node_inner];
                    double y = g_node_y[node_inner];
                    sequence_x[nCount] = x;
                    sequence_y[nCount] = y;
                    nCount++;
                }
                Array.Resize(ref sequence_x, nCount); Array.Resize(ref sequence_y, nCount);
                if (u_active == "all")
                {
                    LineItem vehicleCurve = z1.GraphPane.AddCurve("v" + v.ToString(), sequence_x, sequence_y, Color.FromArgb(v * 60 % 255, v * 120 % 255, v * 40 % 255), SymbolType.XCross);
                }
                else
                {
                    LineItem vehicleCurve = z1.GraphPane.AddCurve("v" + v.ToString(), sequence_x, sequence_y, Color.FromArgb(v * 60 % 255, v * 120 % 255, v * 40 % 255), SymbolType.XCross);
                    vehicleCurve.Line.IsVisible = false;
                    for (int i = 1; i < sequence_x.Length; i++)
                    {
                        ArrowObj myArrow = new ArrowObj(Color.DarkBlue, 8f, sequence_x[i - 1], sequence_y[i - 1], sequence_x[i], sequence_y[i]);
                        z1.GraphPane.GraphObjList.Add(myArrow);
                    }
                }
            }

            

            ///////////////////////////////
            z1.AxisChange();
            z1.Invalidate();
        }


        #region "common function or bak" 

        /// <summary>
        /// 方法实现把dgv里的数据完整的复制到一张内存表
        /// </summary>
        /// <param name="dgv">dgv控件作为参数</param>
        /// <returns>返回临时内存表</returns>
        public static DataTable GetDgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            for (int count = 0; count < dgv.Rows.Count - 1; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                {
                    dr[countsub] = dgv.Rows[count].Cells[countsub].Value.ToString();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        void RunGamsModel(string gams_file_name)
        {
            //string str_path = @"E:\3 PrgStudy\MrpPlus_dominate\MvrpLite\bin\Debug\";

            string str_path = Directory.GetCurrentDirectory();
            str_path += @"\";
            string str_gams_path = this.m_dtConfiguration.Rows[0]["gams_path"].ToString();

            Process p = new Process();
            p.StartInfo.FileName = str_gams_path + "gams.exe";/// @"C:\GAMS\win64\24.4\gams.exe";
            p.StartInfo.WorkingDirectory = str_path + @"\gmas";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.Arguments = "\"" + str_path + @"gmas\" + "gams_model.gpr" + "\" LO=0 --runid=" + "0";
            p.Start();
            
            p.StartInfo.Arguments = "\"" + str_path + @"gmas\" + gams_file_name + ".gms" +"\" LO=0 --runid=" + "0";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            p.Start();
            p.WaitForExit();
           // MessageBox.Show("success solve problem by gams");
        }
        #endregion

       



    }
}
