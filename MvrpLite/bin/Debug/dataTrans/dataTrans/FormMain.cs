using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.IO;


namespace dataTrans
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }



        #region "xml->csv"
        /// <summary>
        /// get input file path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "open";
            ofd.Filter = "all files|*.xml"; /////
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txbInputFilePath.Text = ofd.FileName;
            }
        }
        /// <summary>
        /// trans xml->csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXml2CSV_Click(object sender, EventArgs e)
        {


            #region "xml -> memoral"

            XmlDocument doc = new XmlDocument();
            doc.Load(this.txbInputFilePath.Text);
            XmlNode xn = doc.SelectSingleNode("IRP_Roadef_Challenge_Instance");            
            XmlNodeList elemList = xn.ChildNodes;

            string unit="",horizon="";

            int[,] timeMatrices=new int[200,200]; 

            int[] driverIndex=new int[200];
            int[] driverMaxDrivingDuration=new int[200];
            int[] driverTrailer=new int[200];
            int[] driverMinInterSHIFTDURATION=new int[200];
            double[] driverTimeCost=new double[200];
            int[,] driverTimeWindowStart=new int[200,200];
            int[,] driverTimeWindowEnd=new int[200,200];


            int[] trailerIndex=new int[200];
            int[] trailerCapacity = new int[200];
            int[] trailerInitialQuantity = new int[200];
            double[] trailerDistanceCost = new double[200];

            int[] baseIndex = new int[200];

            int[] sourceIndex = new int[200];
            int[] sourceSetupTime = new int[200];

            int[] customerIndex = new int[200];
            int[] customerSetupTime= new int[200];
            int[,] customerAllowedTrailers = new int[200, 200];
            float[,] customerForecast = new float[200, 2000];
            float[] customerCapacity = new float[200];
            float[] customerInitialTankQuantity = new float[200];
            float[] customerSafetyLevel = new float[200];

            float[,] distMatrices = new float[200, 200]; 

            ////////////////////////////////////////////////
            foreach (XmlNode elem in elemList)
            {
                if (elem.Name == "unit") unit = elem.InnerText;
                else if (elem.Name == "horizon") horizon = elem.InnerText;
                else if (elem.Name == "timeMatrices")
                {
                    XmlNodeList elemList2 = elem.ChildNodes;

                    int nRowCount = 1;
                    foreach (XmlNode elem2 in elemList2)
                    {
                        if (elem2.Name == "ArrayOfInt")
                        {
                            XmlNodeList elemList3 = elem2.ChildNodes;
                            int nColCount = 1;
                            foreach (XmlNode elem3 in elemList3)
                            {
                                if (elem3.Name == "int")
                                {
                                    timeMatrices[nRowCount, nColCount] = int.Parse(elem3.InnerText);
                                    nColCount++;
                                }
                            }
                            timeMatrices[nRowCount, 0] = nColCount-1;                            
                        }
                        nRowCount++;
                    }///end of elem2
                    timeMatrices[0, 0] = nRowCount - 1;

                }////end of timeMatrices

                /////////////////
                else if (elem.Name == "drivers")
                {
                    int nRowCount = 1;
                    XmlNodeList elemList20 = elem.ChildNodes;
                    foreach (XmlNode elem20 in elemList20)
                    {
                        XmlNodeList elemList2 = elem20.ChildNodes;

                        foreach (XmlNode elem2 in elemList2)
                        {
                            if (elem2.Name == "index") driverIndex[nRowCount] = int.Parse(elem2.InnerText);
                            else if (elem2.Name == "maxDrivingDuration") driverMaxDrivingDuration[nRowCount] = int.Parse(elem2.InnerText);
                            else if (elem2.Name == "trailer") driverTrailer[nRowCount] = int.Parse(elem2.InnerText);
                            else if (elem2.Name == "minInterSHIFTDURATION") driverMinInterSHIFTDURATION[nRowCount] = int.Parse(elem2.InnerText);
                            else if (elem2.Name == "TimeCost") driverTimeCost[nRowCount] = double.Parse(elem2.InnerText);

                            else if (elem2.Name == "timewindows")
                            {
                                XmlNodeList elemList3 = elem2.ChildNodes;
                                int nColCount = 1;
                                foreach (XmlNode elem3 in elemList3)
                                {
                                    if (elem3.Name == "TimeWindow")
                                    {
                                        driverTimeWindowStart[nRowCount, nColCount] = int.Parse(elem3.ChildNodes[0].InnerText);
                                        driverTimeWindowEnd[nRowCount, nColCount] = int.Parse(elem3.ChildNodes[1].InnerText);
                                    }
                                    nColCount++;
                                }
                                driverTimeWindowStart[nRowCount, 0] = nColCount - 1;
                                driverTimeWindowEnd[nRowCount, 0] = nColCount - 1;
                            }
                        }
                        nRowCount++;
                        driverTimeWindowStart[0, 0] = nRowCount - 1;
                        driverTimeWindowEnd[0, 0] = nRowCount - 1;
                        driverIndex[0] = nRowCount - 1;
                        driverMaxDrivingDuration[0] = nRowCount - 1;
                    }
                }///////////end of driver


                else if (elem.Name == "trailers")
                {
                    int nRowCount = 1;
                    XmlNodeList elemList2 = elem.ChildNodes;
                    foreach (XmlNode elem2 in elemList2)
                    {
                        XmlNodeList elemList3 = elem2.ChildNodes;
                        foreach (XmlNode elem3 in elemList3)
                        {
                            if (elem3.Name == "index") trailerIndex[nRowCount] = int.Parse(elem3.InnerText);
                            else if (elem3.Name == "Capacity") trailerCapacity[nRowCount] = int.Parse(elem3.InnerText);
                            else if (elem3.Name == "InitialQuantity") trailerInitialQuantity[nRowCount] = int.Parse(elem3.InnerText);
                            else if (elem3.Name == "DistanceCost") trailerDistanceCost[nRowCount] = double.Parse(elem3.InnerText);
                        }
                        nRowCount++;
                    }
                    trailerIndex[0] = nRowCount - 1;
                    trailerCapacity[0] = nRowCount - 1;
                    trailerInitialQuantity[0] = nRowCount - 1;
                    trailerDistanceCost[0] = nRowCount - 1;
                }//////////end of trailer

                else if (elem.Name == "bases")
                {
                    int nRowCount = 1;
                    XmlNodeList elemList2 = elem.ChildNodes;
                    foreach (XmlNode elem2 in elemList2)
                    {
                        if (elem2.Name == "index") baseIndex[nRowCount] = int.Parse(elem2.InnerText);
                        nRowCount++;
                    }
                    baseIndex[0] = nRowCount - 1;
                }////////////end of bases


                else if (elem.Name == "sources")
                {
                    int nRowCount = 1;
                    XmlNodeList elemList2 = elem.ChildNodes;
                    foreach (XmlNode elem2 in elemList2)
                    {
                        XmlNodeList elemList3 = elem2.ChildNodes;
                        foreach (XmlNode elem3 in elemList3)
                        {
                            if (elem3.Name == "index") sourceIndex[nRowCount] = int.Parse(elem3.InnerText);
                            else if (elem3.Name == "setupTime") sourceSetupTime[nRowCount] = int.Parse(elem3.InnerText);
                          
                        }  
                        nRowCount++;
                    }
                    sourceIndex[0] = nRowCount - 1;
                    sourceSetupTime[0] = nRowCount - 1;
                }////////////end of sources

                else if (elem.Name == "customers")
                {
                    int nRowCount = 1;
                    XmlNodeList elemList20 = elem.ChildNodes;
                    foreach (XmlNode elem20 in elemList20)
                    {
                        XmlNodeList elemList2 = elem20.ChildNodes;

                        foreach (XmlNode elem2 in elemList2)
                        {
                            if (elem2.Name == "index") customerIndex[nRowCount] = int.Parse(elem2.InnerText);
                            else if (elem2.Name == "setupTime") customerSetupTime[nRowCount] = int.Parse(elem2.InnerText);
                            else if (elem2.Name == "allowedTrailers")
                            {
                                int nColCount=1;
                                XmlNodeList elemList3 = elem2.ChildNodes;
                                foreach (XmlNode elem3 in elemList3)
                                {
                                    customerAllowedTrailers[nRowCount, nColCount] = int.Parse(elem3.InnerText);
                                    nColCount++;
                                }
                                customerAllowedTrailers[nRowCount, 0] = nColCount - 1;
                            }

                            else if (elem2.Name == "Forecast")
                            {
                                int nColCount = 1;
                                XmlNodeList elemList3 = elem2.ChildNodes;
                                foreach (XmlNode elem3 in elemList3)
                                {
                                    customerForecast[nRowCount, nColCount] = float.Parse(elem3.InnerText);
                                    nColCount++;
                                }
                                customerForecast[nRowCount, 0] = nColCount - 1;
                            }

                            else if (elem2.Name == "Capacity") customerCapacity[nRowCount] = float.Parse(elem2.InnerText);
                            else if (elem2.Name == "InitialTankQuantity") customerInitialTankQuantity[nRowCount] = float.Parse(elem2.InnerText);
                            else if (elem2.Name == "SafetyLevel") customerSafetyLevel[nRowCount] = float.Parse(elem2.InnerText);

                        }/////end of elem2
                        nRowCount++;
                        customerIndex[0] = nRowCount - 1;
                        customerSetupTime[0] = nRowCount - 1;
                        customerAllowedTrailers[0, 0] = nRowCount - 1;
                        customerForecast[0, 0] = nRowCount - 1;
                        customerCapacity[0] = nRowCount - 1;
                        customerInitialTankQuantity[0] = nRowCount - 1;
                        customerSafetyLevel[0] = nRowCount - 1;
                    }
                   
                }///////////end of customers


                else if (elem.Name == "DistMatrices")
                {
                    XmlNodeList elemList2 = elem.ChildNodes;

                    int nRowCount = 1;
                    foreach (XmlNode elem2 in elemList2)
                    {
                        if (elem2.Name == "ArrayOfDouble")
                        {
                            XmlNodeList elemList3 = elem2.ChildNodes;
                            int nColCount = 1;
                            foreach (XmlNode elem3 in elemList3)
                            {
                                if (elem3.Name == "double")
                                {
                                    distMatrices[nRowCount, nColCount] = float.Parse(elem3.InnerText);
                                    nColCount++;
                                }
                            }
                            distMatrices[nRowCount, 0] = nColCount - 1;
                        }
                        nRowCount++;
                    }///end of elem2
                    distMatrices[0, 0] = nRowCount - 1;

                }////end of DistMatrices

            }////end of elem

            #endregion

            //////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////


            string[] strPath=this.txbInputFilePath.Text.Split('\\');
            string pathFile = strPath[0];
            for (int i = 1; i < strPath.Length-1;i++ )
                pathFile += @"\" + strPath[i];
            string lastFileName = strPath[strPath.Length - 1].Split('.')[0];
            pathFile += "\\" + lastFileName;

            ///////////////////input_node
            string fullPath = pathFile  + "\\input_node.csv";
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";


            //column name
            data += "name"; data += ",node_id"; data += ",node_type"; data += ",x"; data += ",y"; data += ",lamda_1"; data += ",lamda_2";
            data += ",setupTime"; data += ",allowedTrailers_1"; data += ",allowedTrailer_2"; data += ",capacity"; data += ",InitialTankQuantity";
            data += ",SafetyLevel";
            sw.WriteLine(data);
            //////////////1.input base
            data = "";
            data += "," + baseIndex[1].ToString();
            data += "," + "1";
            sw.WriteLine(data);

            /////2.input source
            data = "";
            data += "," + sourceIndex[1].ToString();////index
            data += "," + "2";
            data += ","; data += ","; data += ","; data += ",";
            data += ","+sourceSetupTime[1].ToString();
            sw.WriteLine(data);
            /////3. input customes
            for(int i=1;i<=customerIndex[0];i++)
            {
                data = "";               
                data += "," + customerIndex[i].ToString();////index
                data += "," + "3";
                data += ","; data += ","; data += ","; data += ",";
                data += "," + customerSetupTime[i].ToString();
                data += "," + customerAllowedTrailers[i, 1].ToString();
                data += "," + customerAllowedTrailers[i, 2].ToString();
                data += "," + customerCapacity[i].ToString();
                data += "," + customerInitialTankQuantity[i].ToString();
                data += "," + customerSafetyLevel[i].ToString();

                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();

            /////////////////
            fullPath = pathFile + "\\input_customers_forecast.csv";
            fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sw = new StreamWriter(fs, System.Text.Encoding.Default);
            data = "";

            //column name
            data += "horizon_id";
            for (int i = 1; i <= customerForecast[0, 0]; i++)
                data += ",customer_" + customerIndex[i].ToString();
            sw.WriteLine(data);
            /////input forecast
            for (int i = 1; i <= customerForecast[1,0]; i++)
            {
                data = "";
                data += i.ToString();
                for (int j = 1; j <= customerIndex[0]; j++)
                {
                    data += "," + customerForecast[j,i].ToString();
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();


            /////////////////////////////
            /////////////////
            fullPath = pathFile + "\\input_link.csv";
            fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sw = new StreamWriter(fs, System.Text.Encoding.Default);
            data = "";

            //column name
            data += "name";
            data += ",link_id";
            data += ",from_node_id"; data += ",to_node_id"; data += ",link_type";
            data += ",link_time"; data += ",link_dist";

            sw.WriteLine(data);
            /////input link
            
            int nCount = 0;
            for (int i = 1; i <= timeMatrices[0, 0];i++ )
            {
                for(int j=1;j<=timeMatrices[i,0];j++)
                {
                    nCount++;
                    data = "";
                    data += ","+nCount.ToString();
                    data += "," +  (i-1).ToString();
                    data += "," + (j - 1).ToString();
                    data += ",";
                    data += "," + timeMatrices[i, j].ToString();
                    data += "," + distMatrices[i, j].ToString();
                    sw.WriteLine(data);
                }
            }
            sw.Close();
            fs.Close();

            /////////////////////////////
            /////////////////
            fullPath = pathFile + "\\input_drivers.csv";
            fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sw = new StreamWriter(fs, System.Text.Encoding.Default);
            data = "";

            //column name
            data += "agent_id";
            data += ",maxDrivingDuration";
            data += ",trailer"; data += ",minInterSHIFTDURATION"; data += ",TimeCost";            
            sw.WriteLine(data);
            /////input drivers
            for (int i = 1; i <= driverIndex[0];i++ )
            {
                data = "";
                data += driverIndex[i].ToString();
                data += "," + driverMaxDrivingDuration[i].ToString();
                data += "," + driverTrailer[i].ToString();
                data += "," + driverMinInterSHIFTDURATION[i].ToString();
                data += "," + driverTimeCost[i].ToString();
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();


            /////////////////////////////
            /////////////////
            fullPath = pathFile + "\\input_drivers_timeWindow.csv";
            fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sw = new StreamWriter(fs, System.Text.Encoding.Default);
            data = "";

            //column name
            data += "unit_no";
            for (int i = 1; i <= driverIndex[0];i++ )
            { 
                data += ",dirver_"+driverIndex[i].ToString()+"_start";
                data += ",dirver_" + driverIndex[i].ToString() + "_end";
            }
            sw.WriteLine(data);
            /////input timewindows
            for (int i = 1; i <= driverTimeWindowStart[1,0]; i++)
            {
                data = "";
                data += i.ToString();
                for (int j = 1; j <= driverIndex[0]; j++)
                {
                    data += "," + driverTimeWindowStart[j, i].ToString();
                    data += "," + driverTimeWindowEnd[j, i].ToString();
                    
                }
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();


            /////////////////////////////
            /////////////////
            fullPath = pathFile + "\\input_trailers.csv";
            fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sw = new StreamWriter(fs, System.Text.Encoding.Default);
            data = "";

            //column name
            data += "trailer_id";
            data += ",Capacity";
            data += ",InitialQuantity"; data += ",DistanceCost"; 
            sw.WriteLine(data);
            /////input drivers
            for (int i = 1; i <= trailerIndex[0]; i++)
            {
                data = "";
                data += trailerIndex[i].ToString();
                data += "," + trailerCapacity[i].ToString();
                data += "," + trailerInitialQuantity[i].ToString();
                data += "," + trailerDistanceCost[i].ToString();
                sw.WriteLine(data);
            }
            sw.Close();
            fs.Close();

            
            MessageBox.Show("CSV文件保存成功！");

        }

        #endregion

        #region"txt->csv"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowser2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "open";
            ofd.Filter = "all files|*.*"; /////
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txbInputFilePath2.Text = ofd.FileName;
            }
        }
        /// <summary>
        /// trans txt->csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTxt2CSV_Click(object sender, EventArgs e)
        {
            string[] strPath = this.txbInputFilePath2.Text.Split('\\');
            string pathFile = strPath[0];
            for (int i = 1; i < strPath.Length - 1; i++)
                pathFile += @"\" + strPath[i];

            int nCount = 0;
            DirectoryInfo dir = new DirectoryInfo(pathFile);
            foreach (FileInfo dChild in dir.GetFiles("*."))
            {
                string fileName= dChild.FullName;
                //MessageBox.Show(fileNam);
                txt2Csv(fileName);
                nCount++;

                //if (nCount == 4) break;  ////debug
            }


            MessageBox.Show("CSV文件保存成功,共保存文件数："+nCount.ToString()+" 个！");

        }

        private void txt2Csv(string inputFileName)
        {

            StreamReader sr = new StreamReader(inputFileName, Encoding.Default);
            String line;
            int[] customersInfo = new int[5];
            string[,] customersOD = new string[200, 7];
            string[] vehicleOD = new string[7];////only has one vehicle

            int nCount = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (nCount == 0)
                {
                    string[] strLine = line.ToString().Split(' ');
                    for (int i = 0; i < 5; i++)
                        customersInfo[i] = int.Parse(strLine[i]);
                    nCount++;
                    continue;
                }
                if (nCount == 1)
                {
                    nCount++;
                    continue;////ignore the first depot
                }



                string[] strLines = line.ToString().Split(' ');
                string[] strLineInfo = new string[7];
                int nStrCount = 0;
                for (int i = 0; i < strLines.Length; i++)
                    if (strLines[i] != "") strLineInfo[nStrCount++] = strLines[i];

                for (int i = 0; i < strLineInfo.Length; i++)
                    customersOD[nCount - 1, i] = strLineInfo[i];

                nCount++;
            }
            customersOD[0, 0] = (nCount - 2).ToString();////all exist node

            for (int i = 0; i < 7;i++ )
                vehicleOD[i] = customersOD[int.Parse(customersOD[0, 0])+1, i];
            ///////////////////////////////////////////////////////////

            string[] strPath = inputFileName.Split('\\');
            string pathFile = strPath[0];
            for (int i = 1; i < strPath.Length - 1; i++)
                pathFile += @"\" + strPath[i];
            string lastFileName = strPath[strPath.Length - 1].Split('.')[0];
            pathFile += "\\" + lastFileName + "CSV";

            ///////////////////input_node
            string fullPath = pathFile + "\\input_node.csv";
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";


            //column name
            data += "name"; data += ",node_id"; data += ",QEM_reference_node_id"; data += ",control_type"; data += ",control_type_name"; data += ",cycle_length_in_second"; data += ",offset_in_second";
            data += ",x"; data += ",y"; data += ",geometry";
            sw.WriteLine(data);
            /////////////input node
            for (int i = 1; i <= int.Parse(customersOD[0, 0]); i++)
            {
                data = "";
                data += "," + customersOD[i, 0].ToString();
                data += ",0,0,unknown_control,0,0";
                data += "," + customersOD[i, 1].ToString();
                data += "," + customersOD[i, 2].ToString();
                data += "," + "\"" + @"<Point><coordinates>" + customersOD[i, 1].ToString() + "," + customersOD[i, 2].ToString() + @"</coordinates></Point>" + "\"";
                sw.WriteLine(data);
            }
            //////add depot
            //data = "";
            //data += "," + vehicleOD[0].ToString();
            //data += ",0,0,unknown_control,0,0";
            //data += "," + vehicleOD[1].ToString();
            //data += "," + vehicleOD[2].ToString();
            //data += "," + "\"" + @"<Point><coordinates>" + vehicleOD[1].ToString() + "," + vehicleOD[2].ToString() + @"</coordinates></Point>" + "\"";
            //sw.WriteLine(data);

          

            sw.Close();
            fs.Close();


            /////////////////////////////
            /////////////////
            fullPath = pathFile + "\\input_link.csv";
            fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sw = new StreamWriter(fs, System.Text.Encoding.Default);
            data = "";

            //column name
            data += "name"; data += ",link_id"; data += ",link_key"; data += ",speed_sensor_id"; data += ",count_sensor_id";
            data += ",from_node_id"; data += ",to_node_id";
            data += ",link_type_name"; data += ",direction"; data += ",length"; data += ",number_of_lanes"; data += ",speed_limit";
            data += ",speed_at_capacity"; data += ",lane_capacity_in_vhc_per_hour";
            data += ",link_type"; data += ",jam_density"; data += ",wave_speed"; data += ",demand_type_code"; data += ",mode_code"; data += ",network_design_flag"; data += ",grade";
            data += ",geometry"; data += ",original_geometry";
            data += ",BPR_alpha_term"; data += ",BPR_beta_term"; data += ",number_of_left_turn_lanes";
            data += ",length_of_left_turn_lanes"; data += ",number_of_right_turn_lanes"; data += ",length_of_right_turn_lanes"; data += ",from_approach"; data += ",to_approach"; data += ",reversed_link_id";
            data += ",orientation_code"; data += ",loop_code";
            sw.WriteLine(data);
            /////input links
            int nLinkCount = 0;
            for (int i = 1; i <= int.Parse(customersOD[0, 0]); i++)
            {
                for (int j = 1; j <= int.Parse(customersOD[0, 0]); j++)
                {
                    if (i == j) continue;
                    double length = 0f;
                    float x1 = float.Parse(customersOD[i, 1]);
                    float y1 = float.Parse(customersOD[i, 2]);
                    float x2 = float.Parse(customersOD[j, 1]);
                    float y2 = float.Parse(customersOD[j, 2]);
                    length = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

                    data = "";
                    data += "," + (nLinkCount++).ToString();
                    data += ",,,";
                    data += "," + customersOD[i, 0].ToString();//from
                    data += "," + customersOD[j, 0].ToString();//to
                    data += "," + "Freeway";
                    data += "," + "1";
                    data += "," + length.ToString(".000");
                    data += "," + "1,60,25,4590,1,180,12,,,0,0";
                    data += "," + "\"" + @"<LineString><coordinates>" + x1.ToString(".0000") + "," + y1.ToString(".0000") + "," + x2.ToString(".0000") + "," + y2.ToString(".0000") + @"</coordinates></LineString>" + "\"";
                    data += "," + "\"" + @"<LineString><coordinates>" + x1.ToString(".0000") + "," + y1.ToString(".0000") + "," + x2.ToString(".0000") + "," + y2.ToString(".0000") + @"</coordinates></LineString>" + "\"";
                    data += "," + "0.15,4,0,0,0,0,N,N,0";
                    // data += "," + "0,0";
                    sw.WriteLine(data);
                }
            }


            sw.Close();
            fs.Close();
            ////////////////////////////////////////
            ///////////input_agent
            fullPath = pathFile + "\\input_agent.csv";
            fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            sw = new StreamWriter(fs, System.Text.Encoding.Default);
            data = "";

            //column name
            data += "agent_id"; data += ",agent_type";
            data += ",from_node_id"; data += ",to_node_id";
            data += ",departure_time_start"; data += ",departure_time_window"; data += ",arrival_time_start"; data += ",arrival_time_window";
            data += ",capacity";
            data += ",base_profit"; data += ",VOIVTT_per_hour"; data += ",VOWT_per_hour,";

            sw.WriteLine(data);
            /////input agent

            int agentNum = (int.Parse(customersOD[0, 0])-1) / 2;

            for (int i = 1; i <= agentNum; i++)
            {
                int j = agentNum + i;

                data = customersOD[i, 0].ToString();
                data += ",0";
                data += "," + customersOD[i, 0].ToString();//from
                data += "," + customersOD[j, 0].ToString();//to
                data += "," + customersOD[i, 5].ToString();
                data += "," + (int.Parse(customersOD[i, 6]) - int.Parse(customersOD[i, 5])).ToString();

                //data += "," +"1";
                data += "," + customersOD[j, 5].ToString();
                data += "," + (int.Parse(customersOD[j, 6]) - int.Parse(customersOD[j, 5])).ToString();
                data += "," + customersOD[i, 4].ToString();//capacity  also is demand
                data += ",50";
                sw.WriteLine(data);

            }
            /////input vehicles
            int nVehicleOrder =Math.Max(agentNum*2+1,101);
            int nVehicleTotalNum = customersInfo[1];
            int nVehicleCapacity = customersInfo[3];
            int v_from_node = int.Parse(customersOD[int.Parse(customersOD[0, 0]), 0]);
            int v_to_node = int.Parse(customersOD[int.Parse(customersOD[0, 0]), 0]);
            //int v_x = int.Parse(customersOD[int.Parse(customersOD[0, 0]), 1]);
            //int v_y = int.Parse(customersOD[int.Parse(customersOD[0, 0]), 2]);
            //int v_depart_time = int.Parse(customersOD[int.Parse(customersOD[0, 0]), 5]);
            //int v_depart_time_window = int.Parse(customersOD[int.Parse(customersOD[0, 0]), 6]) - v_depart_time;
            //int v_arrive_time = v_depart_time;
            //int v_arrive_time_window = v_depart_time_window;

            for (int i = 1; i <= nVehicleTotalNum;i++ )
            {
                data = (nVehicleOrder++).ToString();
                data += ",1";
                data += "," + v_from_node;//from
                data += "," + v_to_node;//to
                data += "," + "1";
                data += "," + "1000";
                data += "," + "1";
                data += "," + "1000";
                data += "," + nVehicleCapacity;
                data += ",,60,0";
                sw.WriteLine(data);
            }

         

            sw.Close();
            fs.Close();


            //////////////////////////////////////////
            /////////////debug_for_price
            //fullPath = pathFile + "\\debug_for_price.csv";
            //fi = new FileInfo(fullPath);
            //if (!fi.Directory.Exists)
            //{
            //    fi.Directory.Create();
            //}
            //fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //sw = new StreamWriter(fs, System.Text.Encoding.Default);
            //data = "";

            ////column name
            //data += "vehicle_id";
            //for (int i = 1; i <= agentNum;i++ )
            //{
            //    data += ",passenger_" + customersOD[i, 0].ToString();
            //    data += ",,,";
            //}
            //sw.WriteLine(data);

            //data = ",";
            //for (int i = 1; i <= agentNum; i++)
            //{
            //    data += "V_0->P_0,VP_0->VP_d,P_d->V_0,sum,";                
            //}
            //sw.WriteLine(data);

            ///////for vehicles
            //nVechicleNum = agentNum * 2 + 1;
            //for (int i = 0; i <= 4; i++)
            //    for (int j = 0; j <= 4; j++)
            //    {
            //        data = (nVechicleNum++).ToString();
            //        float x_v_o = 5 + 10 * i;
            //        float y_v_o = 5 + 10 * j;
            //        for (int k = 1; k <= agentNum; k++)
            //        {
            //             float x_p_o = float.Parse(customersOD[k, 1]);
            //             float y_p_o = float.Parse(customersOD[k, 2]);
            //             float x_p_d = float.Parse(customersOD[k+agentNum, 1]);
            //             float y_p_d = float.Parse(customersOD[k+agentNum, 2]);
            //             double length_v0_p0 = Math.Sqrt((x_p_o - x_v_o) * (x_p_o - x_v_o) + (y_p_o - y_v_o) * (y_p_o - y_v_o));
            //             double length_p0_pd = Math.Sqrt((x_p_d - x_p_o) * (x_p_d - x_p_o) + (y_p_d - y_p_o) * (y_p_d - y_p_o));
            //             double length_pd_v0 = Math.Sqrt((x_v_o - x_p_d) * (x_v_o - x_p_d) + (y_v_o - y_p_d) * (y_v_o - y_p_d));

            //             data += "," + length_v0_p0.ToString(".000");
            //             data += "," + length_p0_pd.ToString(".000");
            //             data += "," + length_pd_v0.ToString(".000");
            //             data += "," + (length_v0_p0 + length_p0_pd + length_pd_v0).ToString(".000");

            //        }


            //        sw.WriteLine(data);
            //    }

            ///////for virtual vehicles
            //for (int i = 1; i <= agentNum; i++)
            //{
            //    data ="VV"+ customersOD[i, 0].ToString();
            //    float x_v_o = float.Parse(customersOD[i, 1]);
            //    float y_v_o = float.Parse(customersOD[i, 2]);

            //    for (int k = 1; k <= agentNum; k++)
            //    {
            //        if(k!=i)
            //        {
            //            data += ",,,,";
            //            continue;
            //        }

            //        float x_p_o = float.Parse(customersOD[k, 1]);
            //        float y_p_o = float.Parse(customersOD[k, 2]);
            //        float x_p_d = float.Parse(customersOD[k + agentNum, 1]);
            //        float y_p_d = float.Parse(customersOD[k + agentNum, 2]);
            //        double length_v0_p0 = Math.Sqrt((x_p_o - x_v_o) * (x_p_o - x_v_o) + (y_p_o - y_v_o) * (y_p_o - y_v_o));
            //        double length_p0_pd = Math.Sqrt((x_p_d - x_p_o) * (x_p_d - x_p_o) + (y_p_d - y_p_o) * (y_p_d - y_p_o));
            //        double length_pd_v0 = Math.Sqrt((x_v_o - x_p_d) * (x_v_o - x_p_d) + (y_v_o - y_p_d) * (y_v_o - y_p_d));

            //        data += "," + length_v0_p0.ToString(".000");
            //        data += "," + length_p0_pd.ToString(".000");
            //        data += "," + length_pd_v0.ToString(".000");
            //        data += "," + (length_v0_p0 + length_p0_pd + length_pd_v0).ToString(".000");

            //    }

            //    sw.WriteLine(data);

            //}


            //sw.Close();
            //fs.Close();


        }

        #endregion


        #region"txt->txt"
        private void btnBrowser3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "open";
            ofd.Filter = "all files|*.txt"; /////
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txbInputFilePath3.Text = ofd.FileName;
            }
        }
 
        private void btnTxt2Txt_Click(object sender, EventArgs e)
        {

            StreamReader sr = new StreamReader(this.txbInputFilePath3.Text, Encoding.Default);
            String line;
            int[] vehicleInfo = new int[2];
            string[,] customersOD = new string[200, 7];


            int nCount = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (nCount == 4)
                {
                    string[] strLinesA = line.ToString().Split(' ');
                    string[] strLineInfoA = new string[2];
                    int nStrCountA = 0;
                    for (int i = 0; i < strLinesA.Length; i++)
                        if (strLinesA[i] != "") strLineInfoA[nStrCountA++] = strLinesA[i];


                    for (int i = 0; i < 2; i++)
                        vehicleInfo[i] = int.Parse(strLineInfoA[i]);
                    nCount++;
                    continue;
                }
                if (nCount <9)
                {
                    nCount++;
                    continue;////ignore the prepore depot
                }



                string[] strLines = line.ToString().Split(' ');
                string[] strLineInfo = new string[7];
                int nStrCount = 0;
                for (int i = 0; i < strLines.Length; i++)
                    if (strLines[i] != "") strLineInfo[nStrCount++] = strLines[i];

                for (int i = 0; i < strLineInfo.Length; i++)
                    customersOD[nCount - 8, i] = strLineInfo[i];

                nCount++;
            }
            customersOD[0, 0] = (nCount - 9).ToString();/////ncount-9 means include the last depot,start from index 1

            ///////////////////////////////////////////////////////////

            if(this.txbVehicleNum.Text.ToString()!="")
                vehicleInfo[0] = int.Parse(this.txbVehicleNum.Text.ToString());
            if(this.txbCustomerNum.Text.ToString()!="")
                customersOD[0, 0] = this.txbCustomerNum.Text;////////// computation scale 

            //////////////
            string[] strPath = txbInputFilePath3.Text.Split('\\');
            string pathFile = strPath[0];
            for (int i = 1; i < strPath.Length - 1; i++)
                pathFile += @"\" + strPath[i];
            string lastFileName = strPath[strPath.Length - 1].Split('.')[0];
            pathFile += "\\" + lastFileName + "TXT";

            ///////////////////input_gams_data
            string fullPath = pathFile + "\\input_gams_data.txt";
            FileInfo fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";

            data = @"set m/1*" + vehicleInfo[0].ToString()+@"/;";
            sw.WriteLine(data);

            data = @"set n/0*" + (int.Parse(customersOD[0,0])-1).ToString() + @"/;";
            sw.WriteLine(data);

            data = @"set np/1*" + (int.Parse(customersOD[0, 0]) - 1).ToString() + @"/;";
            sw.WriteLine(data);

            data = @"scalar N_m/" + vehicleInfo[0].ToString() + @"/;";
            sw.WriteLine(data);

            data = @"scalar N_n/" + (int.Parse(customersOD[0, 0]) - 1).ToString() + @"/;";
            sw.WriteLine(data);

            data = @"Q(k)=" + vehicleInfo[1].ToString() + @";";   ////capacity
            sw.WriteLine(data);

            data = @"parameter origin(n)/"; sw.WriteLine(data);
            data = " 0    1 "; sw.WriteLine(data);
            data = @"/;"; sw.WriteLine(data);

            data = @"parameter destination(n)/";sw.WriteLine(data);
            data = " 0    1 "; sw.WriteLine(data);
            data= @"/;"; sw.WriteLine(data);

            data = @"parameter d(i)/"; sw.WriteLine(data);
            for (int i = 1; i <= int.Parse(customersOD[0, 0]);i++ )
            {
                data = customersOD[i, 0].ToString();
                data += "    ";
                data += customersOD[i, 3].ToString();
                sw.WriteLine(data);
            }
            data = @"/;"; sw.WriteLine(data);


            data = @"parameter tr(i,j)/"; sw.WriteLine(data);
            for (int i = 1; i <= int.Parse(customersOD[0, 0]); i++)
            {
                for (int j = 1; j <= int.Parse(customersOD[0, 0]); j++)
                {
                    if (i == j) continue;
                    float x1 = float.Parse(customersOD[i, 1]);
                    float y1 = float.Parse(customersOD[i, 2]);
                    float x2 = float.Parse(customersOD[j, 1]);
                    float y2 = float.Parse(customersOD[j, 2]);
                    double length=Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1));

                    data = customersOD[i, 0].ToString()+".";
                    data += "  ";
                    data += customersOD[j, 0].ToString();
                    data += "  ";
                    data += length.ToString(".000");
                    sw.WriteLine(data);
                }
            }
            data = @"/;"; sw.WriteLine(data);


            data = @"c(i,j)=tr(i,j);"; sw.WriteLine(data);

            data = @"parameter s(i)/"; sw.WriteLine(data);
            for (int i = 1; i <= int.Parse(customersOD[0, 0]); i++)
            {
                data = customersOD[i, 0].ToString();
                data += "    ";
                data += customersOD[i, 6].ToString();
                sw.WriteLine(data);
            }
            data = @"/;"; sw.WriteLine(data);

            data = @"parameter e(i)/"; sw.WriteLine(data);
            for (int i = 1; i <= int.Parse(customersOD[0, 0]); i++)
            {
                data = customersOD[i, 0].ToString();
                data += "    ";
                data += customersOD[i, 4].ToString();
                sw.WriteLine(data);
            }
            data = @"/;"; sw.WriteLine(data);


            data = @"parameter u(i)/"; sw.WriteLine(data);
            for (int i = 1; i <= int.Parse(customersOD[0, 0]); i++)
            {
                data = customersOD[i, 0].ToString();
                data += "    ";
                data += customersOD[i, 5].ToString();
                sw.WriteLine(data);
            }
            data = @"/;"; sw.WriteLine(data);


            data = @"scalar UMax/99999/;";
            sw.WriteLine(data);

            data = @"miu(i,k)=0.1;"; 
            sw.WriteLine(data);

            data = @"matrix_i_e_j(i,i)=1;";
            sw.WriteLine(data);

            data = @"matrix_ip_e_jp(ip,ip)=1;";
            sw.WriteLine(data);

            sw.Close();
            fs.Close();
            /////////////////////////////////////////////////////////////////
            ///////////////////input_gams_data
            fullPath = pathFile + "\\input_case_node_xy.txt";
            fi = new FileInfo(fullPath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            sw = new StreamWriter(fs, System.Text.Encoding.Default);
            data = "";
            for (int i = 1; i <= int.Parse(customersOD[0, 0]); i++)
            {
                data = customersOD[i, 0].ToString()+",";
                float x1 = float.Parse(customersOD[i, 1]);
                float y1 = float.Parse(customersOD[i, 2]);
                data += x1.ToString(".000") + ",";
                data += y1.ToString(".000") ;
                sw.WriteLine(data);                
            }



            sw.Close();
            fs.Close();

            MessageBox.Show("success generate files.");

        }
        #endregion

       
    }
}
