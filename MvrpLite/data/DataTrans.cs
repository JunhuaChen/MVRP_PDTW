using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MvrpLite.data
{
    public abstract class DataTrans
    {
        protected static IRailStuDb dbTrans;
        protected static string strSql;

        #region "对数据库的统一操作"
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataTable execSql(string strSql, string dataFrom)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer")
                if (dataFrom == "server") dbTrans = new RailStuDb_ForSql();
                else if (dataFrom == "serverOut") dbTrans = new RailStuDb_ForSqlOut();
                else dbTrans = new RailStuDb_ForAccess();

            return dbTrans.ExecuteSqlForDs(strSql).Tables[0];
        }
        public static DataTable execSql(string strSql)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer")
                dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();

            return dbTrans.ExecuteSqlForDs(strSql).Tables[0];
        }
        public static void execSqlNoReturn(string strSql)
        {
            //if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();

            dbTrans.ExecuteSqlCmd(strSql);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="isOrder"></param>
        /// <param name="orderField"></param>
        /// <returns></returns>
        public static DataTable getAllData(string tableName, bool isOrder, string orderField, string dataFrom)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer")
            {
                if (dataFrom == "server") dbTrans = new RailStuDb_ForSql();
                else if (dataFrom == "serverOut") dbTrans = new RailStuDb_ForSqlOut();
            }
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "Select * from " + tableName;
            if (isOrder == true) strSql += " order by " + orderField;
            return dbTrans.ExecuteSqlForDs(strSql).Tables[0];
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableName"></param>
        public static void deleteData(string tableName)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "delete from " + tableName;
            dbTrans.ExecuteSqlCmd(strSql);
        }
        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="idAItem"></param>
        /// <param name="idA"></param>
        /// <param name="idBitem"></param>
        /// <param name="idB"></param>
        /// <returns></returns>
        public static bool isExistData(string tableName, string idAItem, string idA, string idBitem, string idB)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "select * from "+tableName+" where "+idAItem+"='"+idA+"'";
            if (idBitem != "") strSql += " and " + idBitem + " = '" + idB + "'";
            DataTable dt = dbTrans.ExecuteSqlForDs(strSql).Tables[0];
            if (dt.Rows.Count == 0) return false;
            else return true;
        }
        #endregion

        #region"标准时间 导入导出更新"
        public static bool insertOneRunStandTime(string fromStation, string toStation, string upOrDown)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO runStandTime (fromStation,toStation,upOrDown)VALUES("
                    + "'" + fromStation + "',"
                    + "'" + toStation + "',"                  
                    + "'" + upOrDown + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void updateRunStandTime(string fromStation, string toStation, string upOrDown, string ttTime, string ttUp,string ttDown,
            string tdTime,string tdUp,string tdDown,string ftTime,string ftUp,string ftDown,string fdTime,string fdUp,string fdDown,

            string ttTime_P, string ttUp_P, string ttDown_P,
            string tdTime_P, string tdUp_P, string tdDown_P, string ftTime_P, string ftUp_P, string ftDown_P, string fdTime_P, string fdUp_P, string fdDown_P)
           
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "UPDATE runStandTime SET ttTime='" + ttTime + "',ttUp='" + ttUp + "',ttDown='"+ttDown+"',"
                +"tdTime='"+tdTime+"',tdUp='"+tdUp+"',tdDown='"+tdDown+"',"
                + "ftTime='" + ftTime + "',ftUp='" + ftUp + "',ftDown='" + ftDown + "',"
                + "fdTime='" + fdTime + "',fdUp='" + fdUp + "',fdDown='" + fdDown + "',"

                + "ttTime_P='" + ttTime_P + "',ttUp_P='" + ttUp_P + "',ttDown_P='" + ttDown_P + "',"
                + "tdTime_P='" + tdTime_P + "',tdUp_P='" + tdUp_P + "',tdDown_P='" + tdDown_P + "',"
                + "ftTime_P='" + ftTime_P + "',ftUp_P='" + ftUp_P + "',ftDown_P='" + ftDown_P + "',"
                + "fdTime_P='" + fdTime_P + "',fdUp_P='" + fdUp_P + "',fdDown_P='" + fdDown_P + "'"
            + " WHERE fromStation=" + "'" + fromStation + "' and toStation='" + toStation + "' and upOrDown='" + upOrDown + "'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }


        #endregion

      

        #region"信号 数据预处理"
        /// <summary>
        /// 加入一条新数据（stateAll）
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="station"></param>
        /// <param name="trainID"></param>
        /// <param name="routeFrom"></param>
        /// <param name="routeTo"></param>
        /// <param name="takeTrack"></param>
        /// <param name="inPrepareRoute"></param>
        /// <param name="inTakeRoute"></param>
        /// <param name="inReleaseTrack"></param>
        /// <param name="inTrainStop"></param>
        /// <param name="outPrepareRoute"></param>
        /// <param name="outTakeRoute"></param>
        /// <param name="outReleaseLine"></param>
        /// <param name="outReleaseTrack"></param>
        /// <param name="trainType"></param>
        /// <param name="isThrough"></param>
        /// <returns></returns>
        public static bool insertOneStateAll(string recordDate, string stationName, string serialNum, string ADNum, string equName, string thisState,
            string stateTime, string stateTimeForOrder)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO stateAll (stationName,recordDate,serialNum,ADNum,equName,thisState,stateTime,stateTimeForOrder)VALUES("
                    + "'" + stationName + "',"
                    + "'" + recordDate + "',"
                    + "'" + serialNum + "',"
                    + "'" + ADNum + "',"
                    + "'" + equName + "',"
                    + "'" + thisState + "',"
                    + "'" + stateTime + "',"
                    + "'" + stateTimeForOrder + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool isExistStateAll(string recordDate, string stationName, string serialNum)
        {
            dbTrans = new RailStuDb_ForSql();
            strSql = "select stationName from [stateAll] where recordDate = '" + recordDate + "' and stationName='" + stationName + "' and serialNum='" + serialNum + "'";
            try
            {
                DataTable dt = dbTrans.ExecuteSqlForDs(strSql).Tables[0];
                if (dt.Rows.Count >= 1) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }



        public static bool insertOneStationSwitch(string stationName, string name, string rightLocation, string oppositeLocation, string redBand, string whiteBand)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO stationSwitch (stationName,name,rightLocation,oppositeLocation,redBand,whiteBand)VALUES("
                    + "'" + stationName + "',"
                    + "'" + name + "',"
                    + "'" + rightLocation + "',"
                    + "'" + oppositeLocation + "',"
                    + "'" + redBand + "',"

                    + "'" + whiteBand + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool insertOneStationSignal(string stationName, string name, string red, string green, string yellow, string white, string blue)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO stationSignal (stationName,name,red,green,yellow,white,blue)VALUES("
                    + "'" + stationName + "',"
                    + "'" + name + "',"
                    + "'" + red + "',"
                    + "'" + green + "',"
                    + "'" + yellow + "',"
                    + "'" + white + "',"
                    + "'" + blue + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool insertOneStationSection(string stationName, string name, string redBand, string whiteBand)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO stationSection (stationName,name,redBand,whiteBand)VALUES("
                    + "'" + stationName + "',"
                    + "'" + name + "',"
                    + "'" + redBand + "',"

                    + "'" + whiteBand + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool insertOneStationButton(string stationName, string name, string green, string yellow, string red, string blue, string white)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO stationButton (stationName,name,green,yellow,red,blue,white)VALUES("
                    + "'" + stationName + "',"
                    + "'" + name + "',"
                    + "'" + green + "',"
                    + "'" + yellow + "',"
                    + "'" + red + "',"
                    + "'" + blue + "',"
                    + "'" + white + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool insertOneSelectSection(string stationName, string name, string redBand, string whiteBand)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO selectSection (stationName,name,redBand,whiteBand)VALUES("
                    + "'" + stationName + "',"
                    + "'" + name + "',"
                    + "'" + redBand + "',"

                    + "'" + whiteBand + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion



        #region"数据准备"
        /// <summary>
        /// 加入一条新数据（SmallStationLine）
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="station"></param>
        /// <param name="trainID"></param>
        /// <param name="routeFrom"></param>
        /// <param name="routeTo"></param>
        /// <param name="takeTrack"></param>
        /// <param name="inPrepareRoute"></param>
        /// <param name="inTakeRoute"></param>
        /// <param name="inReleaseTrack"></param>
        /// <param name="inTrainStop"></param>
        /// <param name="outPrepareRoute"></param>
        /// <param name="outTakeRoute"></param>
        /// <param name="outReleaseLine"></param>
        /// <param name="outReleaseTrack"></param>
        /// <param name="trainType"></param>
        /// <param name="isThrough"></param>
        /// <returns></returns>
        public static bool insertOneSmallStationLine(string recordDate, string station, string trainID, string routeFrom, string routeTo, string takeTrack,
            string inPrepareRoute, string inTakeRoute, string inReleaseTrack, string inTrainStop,
            string outPrepareRoute, string outTakeRoute, string outReleaseLine, string outReleaseTrack, string trainType, string isThrough)
        {
            if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO smallStationLine (recordDate,station,trainID,routeFrom,routeTo,takeTrack,inPrepareRoute,inTakeRoute, inReleaseTrack, inTrainStop,"+
                    "outPrepareRoute, outTakeRoute, outReleaseLine, outReleaseTrack, trainType, isThrough)VALUES("
                    + "'" + recordDate + "',"
                    + "'" + station + "',"
                    + "'" + trainID + "',"
                    + "'" + routeFrom + "',"
                    + "'" + routeTo + "',"
                    + "'" + takeTrack + "',"
                    + "'" + inPrepareRoute + "',"
                    + "'" + inTakeRoute + "',"
                    + "'" + inReleaseTrack + "',"
                    + "'" + inTrainStop + "',"
                    + "'" + outPrepareRoute + "',"
                    + "'" + outTakeRoute + "',"
                    + "'" + outReleaseLine + "',"
                    + "'" + outReleaseTrack + "',"
                    + "'" + trainType + "',"                  
                    + "'" + isThrough + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 加入一条新数据（bigStationLine）
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="stationName"></param>
        /// <param name="reskName"></param>
        /// <param name="takeTrack"></param>
        /// <param name="prepareRoute"></param>
        /// <param name="takeRoute"></param>
        /// <param name="trainStop"></param>
        /// <param name="releaseLine"></param>
        /// <param name="trainType"></param>
        /// <returns></returns>
        public static bool insertOneBigStationLine(string recordDate, string stationName, string reskName, string takeTrack, string prepareRoute, string takeRoute,
            string trainStop, string releaseLine, string reskType, string trainType)
        {
            if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO bigStationLine (recordDate,stationName,reskName,takeTrack,prepareRoute,takeRoute,trainStop,releaseLine, reskType, trainType)VALUES("
                    + "'" + recordDate + "',"
                    + "'" + stationName + "',"
                    + "'" + reskName + "',"
                    + "'" + takeTrack + "',"
                    + "'" + prepareRoute + "',"
                    + "'" + takeRoute + "',"
                    + "'" + trainStop + "',"
                    + "'" + releaseLine + "',"
                    + "'" + reskType + "',"
                    + "'" + trainType + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 加入一条新数据（bigStationThroat）
        /// </summary>
        /// <param name="recordDate"></param>
        /// <param name="stationName"></param>
        /// <param name="reskName"></param>
        /// <param name="routeFrom"></param>
        /// <param name="routeTo"></param>
        /// <param name="prepareRoute"></param>
        /// <param name="takeRoute"></param>
        /// <param name="releaseLine"></param>
        /// <param name="fromToTracks"></param>
        /// <returns></returns>
        public static bool insertOneBigStationThroat(string recordDate, string stationName, string reskName, string routeFrom, string routeTo, string prepareRoute,
           string takeRoute, string releaseLine, string fromToTracks)
        {
            if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO bigStationThroat (recordDate,stationName,reskName,routeFrom,routeTo,prepareRoute,takeRoute,releaseLine, fromToTracks)VALUES("
                    + "'" + recordDate + "',"
                    + "'" + stationName + "',"
                    + "'" + reskName + "',"
                    + "'" + routeFrom + "',"
                    + "'" + routeTo + "',"
                    + "'" + prepareRoute + "',"
                    + "'" + takeRoute + "',"
                    + "'" + releaseLine + "',"
                    + "'" + fromToTracks + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }


       /// <summary>
        /// 加入一条新数据（SectionMoveList）
       /// </summary>
       /// <param name="recoredDate"></param>
       /// <param name="trainID"></param>
       /// <param name="fileID"></param>
       /// <param name="serialNum"></param>
       /// <param name="locomotiveName"></param>
       /// <param name="lastStation"></param>
       /// <param name="thisStation"></param>
       /// <param name="upOrDown"></param>
       /// <param name="stopTime"></param>
       /// <param name="startTime"></param>
       /// <param name="dwellTime"></param>
       /// <param name="moveTime"></param>
       /// <param name="moveTimeType"></param>
       /// <param name="inStationType"></param>
       /// <param name="emptyOrFull"></param>
       /// <param name="trainType"></param>
       /// <param name="trainLength"></param>
       /// <returns></returns>
        public static bool insertOneSectionMoveList(string recoredDate,string trainID, string fileID, string serialNum, string locomotiveName, string lastStation, string thisStation,
         string upOrDown, string stopTime, string startTime, string dwellTime, string moveTime, string moveTimeType, string inStationType, string emptyOrFull,
            string trainType,string trainLength,string trainWeight,string trainWeightAll)
        {
            if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO sectionMoveList (recoredDate, trainID,fileID,serialNum,locomotiveName,lastStation,thisStation,upOrDown,stopTime, startTime," +
           " dwellTime, moveTime, moveTimeType, inStationType, emptyOrFull,trainType,trainLength,trainWeight,trainWeightAll)VALUES("
                    + "'" + recoredDate + "',"
                    + "'" + trainID + "',"
                    + "'" + fileID + "',"
                    + "'" + serialNum + "',"
                    + "'" + locomotiveName + "',"
                    + "'" + lastStation + "',"
                    + "'" + thisStation + "',"
                    + "'" + upOrDown + "',";

            if (stopTime == "--") strSql += "NULL,";
            else strSql += ("'" + stopTime + "',");

            if (startTime == "--") strSql += "NULL,";
            else strSql += ("'" + startTime + "',");

            strSql+=
                     "'" + dwellTime + "',"
                    + "'" + moveTime + "',"
                    + "'" + moveTimeType + "',"
                    + "'" + inStationType + "',"
                    + "'" + emptyOrFull + "',"
                    + "'" + trainType + "',"
                    + "'" + trainLength + "',"
                    + "'" + trainWeight + "',"
                    + "'" + trainWeightAll + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool insertOneStationMoveInfo(string recordDate, string trainID, string fileID, string locomotiveName, string trainType, string upOrDown, string emptyOrFull,
        string OStation, string DStation, string startTime, string endTime, string totalMoveTime, string totalStopTime, string stationNum, string stopStationNum)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO trainMoveInfo (recoredDate, trainID,fileID,locomotiveName,trainType,upOrDown,emptyOrFull,OStation, DStation," +
           " startTime, endTime, totalMoveTime, totalStopTime, stationNum,stopStationNum)VALUES("
                    + "'" + recordDate + "',"
                    + "'" + trainID + "',"
                    + "'" + fileID + "',"
                    + "'" + locomotiveName + "',"
                    + "'" + trainType + "',"
                    + "'" + upOrDown + "',"
                    + "'" + emptyOrFull + "',"
                    + "'" + OStation + "',"
                    + "'" + DStation + "',"
                    + "'" + startTime + "',"
                    + "'" + endTime + "',"
                    + "'" + totalMoveTime + "',"
                    + "'" + totalStopTime + "',"
                    + "'" + stationNum + "',"
                    + "'" + stopStationNum + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool insertOneStationIntervalInfo(string recordDate, string trainID, string fileID, string locomotiveName, string fromStation, string nowStation, string upOrDown,
      string actionTime, string actionType,string runOrDwellTime, string emptyOrFull, string trainType, string trainLength)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO trainIntervalList (recoredDate, trainID,fileID,locomotiveName,fromStation,nowStation,upOrDown,actionTime, actionType,runOrDwellTime," +
           " emptyOrFull, trainType, trainLength)VALUES("
                    + "'" + recordDate + "',"
                    + "'" + trainID + "',"
                    + "'" + fileID + "',"
                    + "'" + locomotiveName + "',"
                    + "'" + fromStation + "',"
                    + "'" + nowStation + "',"
                    + "'" + upOrDown + "',"
                    + "'" + actionTime + "',"
                    + "'" + actionType + "',"
                    + "'" + runOrDwellTime + "',"
                    + "'" + emptyOrFull + "',"
                    + "'" + trainType + "',"               
                    + "'" + trainLength + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void updateSectionMoveListTrainWeight(string fileID, string trainWeight)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "UPDATE sectionMoveList SET trainWeight='" + trainWeight + "'"
            + " WHERE fileID='" + fileID + "'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }
        public static void updateSectionMoveListTrainWeightAll(string fileID, string trainWeightAll)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "UPDATE sectionMoveList SET trainWeightAll='" + trainWeightAll + "'"
            + " WHERE fileID='" + fileID + "'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }

        /// <summary>
        /// 更新下一停站
        /// </summary>
        /// <param name="fromStation"></param>
        /// <param name="nowStation"></param>
        /// <param name="upOrDown"></param>
        public static void updateIntervalNextStation(string fileID, string fromStation, string nowStation,string nextStation)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "UPDATE trainIntervalList SET nextStation='" + nextStation + "'"
            + " WHERE fileID='"+fileID+"' and fromStation=" + "'" + fromStation + "' and nowStation='"+nowStation+"'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }
        public static bool insertOneStationIntervalTime(string recordDate, string intervalType,string intervalTime, string firstUpOrDown,string secondUpDown, 
            string fromStation, string nowStation, string firstTrainType, string secondTrainType)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO outStationInterval (recoredDate, intervalType,intervalTime,firstUpOrDown,secondUpOrDown,fromStation,nowStation," +
           " firstTrainType, secondTrainType)VALUES("
                    + "'" + recordDate + "',"
                    + "'" + intervalType + "',"
                    + "'" + intervalTime + "',"
                    + "'" + firstUpOrDown + "',"
                    + "'" + secondUpDown + "',"
                    + "'" + fromStation + "',"
                    + "'" + nowStation + "',"
                    + "'" + firstTrainType + "',"
                    + "'" + secondTrainType + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool insertOneOutIntervalTime(string recordDate, string startTime, string endTime, string maxI, string minI, string n,
            string fromStation, string toStation, string AUpAUp, string AUpADown, string AUpBUp, string AUpBDown, string AUpCUp, string AUpCDown,
            string ADownAUp, string ADownADown, string ADownBUp, string ADownBDown, string ADownCUp, string ADownCDown,
            string BUpAUp, string BUpADown, string BUpBUp, string BUpBDown, string BUpCUp, string BUpCDown,
            string BDownAUp, string BDownADown, string BDownBUp, string BDownBDown, string BDownCUp, string BDownCDown,
            string CUpAUp, string CUpADown, string CUpBUp, string CUpBDown, string CUpCUp, string CUpCDown,
            string CDownAUp, string CDownADown, string CDownBUp, string CDownBDown, string CDownCUp, string CDownCDown)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO outIntervalTime (recoredDate, startTime,endTime,maxI,minI,n,fromStation,toStation," +
           "AUpAUp, AUpADown, AUpBUp, AUpBDown, AUpCUp, AUpCDown, ADownAUp, ADownADown, ADownBUp, ADownBDown,ADownCUp,ADownCDown,"+
           " BUpAUp, BUpADown, BUpBUp, BUpBDown,BUpCUp,BUpCDown, BDownAUp, BDownADown, BDownBUp, BDownBDown, BDownCUp, BDownCDown,"+
           " CUpAUp, CUpADown, CUpBUp, CUpBDown, CUpCUp, CUpCDown,CDownAUp, CDownADown, CDownBUp, CDownBDown, CDownCUp, CDownCDown)VALUES("
                    + "'" + recordDate + "'," + "'" + startTime + "'," + "'" + endTime + "',"
                    + "'" + maxI + "'," + "'" + minI + "'," + "'" + n + "'," + "'" + fromStation + "'," + "'" + toStation + "',"
                    + "'" + AUpAUp + "'," + "'" + AUpADown + "'," + "'" + AUpBUp + "'," + "'" + AUpBDown + "'," + "'" + AUpCUp + "'," + "'" + AUpCDown + "',"
                    + "'" + ADownAUp + "'," + "'" + ADownADown + "'," + "'" + ADownBUp + "'," + "'" + ADownBDown + "'," + "'" + ADownCUp + "'," + "'" + ADownCDown + "',"
                    + "'" + BUpAUp + "'," + "'" + BUpADown + "'," + "'" + BUpBUp + "'," + "'" + BUpBDown + "'," + "'" + BUpCUp + "'," + "'" + BUpCDown + "',"
                    + "'" + BDownAUp + "'," + "'" + BDownADown + "'," + "'" + BDownBUp + "'," + "'" + BDownBDown + "'," + "'" + BDownCUp + "'," + "'" + BDownCDown + "',"
                    + "'" + CUpAUp + "'," + "'" + CUpADown + "'," + "'" + CUpBUp + "'," + "'" + CUpBDown + "'," + "'" + CUpCUp + "'," + "'" + CUpCDown + "',"
                    + "'" + CDownAUp + "'," + "'" + CDownADown + "'," + "'" + CDownBUp + "'," + "'" + CDownBDown + "'," + "'" + CDownCUp + "'," + "'" + CDownCDown 
                    + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region"数据准备2"

        public static bool insertOneSysTrainInfo(string fileID, string startTime, string trainID, string locomotiveID, string locomotiveType, string startStation, string endStation,
       string moveStartTime, string moveEndTime, string endTime, string driverAID, string driverAName, string driverBID, string driverBName, string limitV,
            string totalQ, string loadQ, string fullNum, string emptyNum)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO sys_trainInfo (fileID, startTime,trainID,locomotiveID,locomotiveType,startStation,endStation,moveStartTime, moveEndTime," +
           " endTime, driverAID, driverAName, driverBID, driverBName,limitV,totalQ,loadQ,fullNum,emptyNum)VALUES("
                    + "'" + fileID + "',"
                    + "'" + startTime + "',"
                    + "'" + trainID + "',"
                    + "'" + locomotiveID + "',"
                    + "'" + locomotiveType + "',"
                    + "'" + startStation + "',"
                    + "'" + endStation + "',"
                    + "'" + moveStartTime + "',"
                    + "'" + moveEndTime + "',"                  
                    + "'" + endTime + "',"
                    + "'" + driverAID + "',"
                    + "'" + driverAName + "',"
                    + "'" + driverBID + "',"
                    + "'" + driverBName + "',"
                    + "'" + limitV + "',"
                    + "'" + totalQ + "',"
                    + "'" + loadQ + "',"
                    + "'" + fullNum + "',"
                    + "'" + emptyNum + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool insertOneTrainRoute(string inTime, string moveStartTime, string moveEndTime, string outTime, 
            string locomotiveType, string locomotiveNum, string trainID,string fileID, string startStation, string endStation,
            string driverAID, string driverAName, string driverBID, string driverBName, string limitV,
            string totalQ, string loadQ, string fullNum, string emptyNum)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO trainRoute (inTime, moveStartTime,moveEndTime,outTime,locomotiveType, locomotiveNum,trainID,fileID, startStation,endStation," +
           " driverAID, driverAName, driverBID, driverBName,limitV,totalQ,loadQ,fullNum,emptyNum)VALUES("
                    + "'" + inTime + "',"
                    + "'" + moveStartTime + "',"
                    + "'" + moveEndTime + "',"
                    + "'" + outTime + "',"                    
                    + "'" + locomotiveType + "',"
                    + "'" + locomotiveNum + "',"
                    + "'" + trainID + "',"
                    + "'" + fileID + "',"
                    + "'" + startStation + "',"
                    + "'" + endStation + "',"                   
                    + "'" + driverAID + "',"
                    + "'" + driverAName + "',"
                    + "'" + driverBID + "',"
                    + "'" + driverBName + "',"
                    + "'" + limitV + "',"
                    + "'" + totalQ + "',"
                    + "'" + loadQ + "',"
                    + "'" + fullNum + "',"
                    + "'" + emptyNum + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool insertEnumTrainID(string orderID, string trainID)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO enum_trainID (orderID, trainID)VALUES("
                    + "'" + orderID + "',"
                    + "'" + trainID + "')";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }
       
        public static bool insertEnumLocomotiveNum(string orderID, string locomotiveNum,string locomotiveType)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO enum_locomotiveNum (orderID, locomotiveNum,locomotiveType)VALUES("
                    + "'" + orderID + "',"
                    + "'" + locomotiveNum + "',"
                    + "'" + locomotiveType + "')";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool insertEnumDriverAName(string orderID, string driverAName)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO enum_driverAName (orderID, driverAName)VALUES("
                    + "'" + orderID + "',"
                    + "'" + driverAName + "')";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool insertEnumTrainRoute(string orderID, string trainRoute)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO enum_trainRoute (orderID, trainRoute)VALUES("
                    + "'" + orderID + "',"
                    + "'" + trainRoute + "')";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion


        #region "1批处理 计算利用率"
        public static bool insertOneOutSection(string stationName, string sectionName, string tracksID, string totalTime, string readyTime, string redTime, string takeNum, string bigRedTime, string bigTakeNum, string nowDate)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO outStationSection (stationName,sectionName,tracksID,totalTime, readyTime,redTime,takeNum,bigRedTime,bigTakeNum,dateRecord)VALUES("
                    + "'" + stationName + "',"
                    + "'" + sectionName + "',"
                    + "'" + tracksID + "',"
                    + "'" + totalTime + "',"
                    + "'" + readyTime + "',"
                    + "'" + redTime + "',"
                    + "'" + takeNum + "',"
                    + "'" + bigRedTime + "',"
                    + "'" + bigTakeNum + "',"
                    + "'" + nowDate + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 增加 异常数据
        /// </summary>
        /// <param name="stationName"></param>
        /// <param name="sectionName"></param>
        /// <param name="nowDate"></param>
        /// <param name="unNormalType"></param>
        /// <param name="lastTime"></param>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        public static bool insertUnNormal(string stationName, string sectionName, string nowDate, string unNormalType, string lastTime, string nowTime, string timeSpan, string unNormalNum)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO outUnNormal (stationName,sectionName,nowDate,unNormalType,lastTime,nowTime,timeSpan, unNormalNum)VALUES("
                    + "'" + stationName + "',"
                    + "'" + sectionName + "',"
                    + "'" + nowDate + "',"
                    + "'" + unNormalType + "',"
                    + "'" + lastTime + "',"
                    + "'" + nowTime + "',"
                    + "'" + timeSpan + "',"
                    + "'" + unNormalNum + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新 白光带，红光带的重复时间
        /// </summary>
        /// <param name="stationName"></param>
        /// <param name="sectionName"></param>
        /// <param name="dateRecord"></param>
        /// <param name="readyTimeRepeat"></param>
        public static void updateRepeatTime(string stationName, string sectionName, string dateRecord, string readyTimeRepeat, string redTimeRepeat)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "UPDATE outStationSection SET readyTimeRepeat='" + readyTimeRepeat + "',redTimeRepeat='" + redTimeRepeat + "'"
            + " WHERE stationName=" + "'" + stationName + "' and sectionName='" + sectionName + "' and dateRecord='" + dateRecord + "'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }

        public static void updateRepeatTimeAll(string stationName, string sectionName, string allReadyTimeRepeat, string allRedTimeRepeat)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "UPDATE outStationSection SET readyTimeRepeat='" + allReadyTimeRepeat + "',redTimeRepeat='" + allRedTimeRepeat + "'"
            + " WHERE stationName=" + "'" + stationName + "' and sectionName='" + sectionName + "' and dateRecord like '%allDay%'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }


        public static bool insertOneOutSectionNew(string stationName, string sectionName, string tracksID, string totalTime, string readyTime, string redTime, string nowDate)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO outStationSectionNew (stationName,sectionName,tracksID,totalTime, readyTime,redTime,dateRecord)VALUES("
                    + "'" + stationName + "',"
                    + "'" + sectionName + "',"
                    + "'" + tracksID + "',"
                    + "'" + totalTime + "',"
                    + "'" + readyTime + "',"
                    + "'" + redTime + "',"
                    + "'" + nowDate + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region"道岔分组 群"
        /// <summary>
        /// ID  是否存在
        /// </summary>
        /// <param name="assessID"></param>
        /// <param name="assessObjectID"></param>
        /// <returns></returns>
        public static bool isClassIDExit(string stationName, string classID)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "select * from stationTrackGroup where stationName='" + stationName + "' and  classID='" + classID + "'";
            try
            {
                if (dbTrans.ExecuteSqlForDs(strSql).Tables[0].Rows.Count == 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 加入新的  道岔群
        /// </summary>
        /// <param name="classID"></param>
        /// <param name="relationUsers"></param>
        public static void addThroatGroup(string stationName, string classID, ArrayList groupTracks, ArrayList tracksID)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            string[] strSqls = new string[groupTracks.Count];
            for (int i = 0; i < groupTracks.Count; i++)
            {
                strSqls[i] = "insert into stationTrackGroup(stationName, classID,tracksID,tracks)values("
                    + "'" + stationName + "',"
                    + "'" + classID + "',"
                    + "'" + (string)tracksID[i] + "',"
                    + "'" + (string)groupTracks[i] + "')";
            }
            try
            {
                dbTrans.ExecuteSqls(strSqls);
            }
            catch
            {
                throw new Exception("添加失败！请重试！");
            }
        }

        /// <summary>
        /// 获得一个 组的 所有 track
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public static DataTable getOneGroupTracks(string stationName, string classID)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "select tracks,tracksID from stationTrackGroup where stationName='" + stationName + "' and classID='" + classID + "'";
            return dbTrans.ExecuteSqlForDs(strSql).Tables[0];
        }
        /// <summary>
        /// 删除一特定 组
        /// </summary>
        /// <param name="classID"></param>
        /// <returns></returns>
        public static bool deleteOneClassID(string stationName, string classID)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "delete from stationTrackGroup where stationName='" + stationName + "' and  classID='" + classID + "'";
            dbTrans.ExecuteSqlCmd(strSql);
            return true;
        }
        /// <summary>
        /// 更新 默认的 道岔分组
        /// </summary>
        /// <param name="stationName"></param>
        /// <param name="defaultGroupID"></param>
        public static void updateDefaultStationTrackGroup(string stationName, string defaultClassID)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "UPDATE stationList SET defaultClassID='" + defaultClassID + "'"
            + " WHERE stationName=" + "'" + stationName + "'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }

        /// <summary>
        /// 更新 使用的道岔分组 到 statinSwitch表
        /// </summary>
        /// <param name="stationName"></param>
        /// <param name="trackName"></param>
        /// <param name="classID"></param>
        /// <param name="tracksID"></param>
        public static void updateStationSwitchTrack(string stationName, string trackName, string classID, string tracksID)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            strSql = "UPDATE stationSwitch SET ClassID='" + classID + "',tracksID='" + tracksID + "'"
            + " WHERE stationName=" + "'" + stationName + "' and name='" + trackName + "'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }
        #endregion


        #region "批处理 车站咽喉 导出数据 并保存"

        public static bool addOutStationLine_throughTrain(string stationName, string trainType, string throughTimes, string totalTakeTime, string averageTakeTime)
        {
            if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO outStationLine_throughTrain (stationName, trainType,throughTimes,totalTakeTime,averageTakeTime)VALUES("
                    + "'" + stationName + "',"
                    + "'" + trainType + "',"
                    + "'" + throughTimes + "',"
                    + "'" + totalTakeTime + "',"
                    + "'" + averageTakeTime + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool addOutStationLine_stopTrain(string stationName, string trainType, string stopTimes,string trainStopTime, string totalTakeTime, string averageTakeTime)
        {
            if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO outStationLine_stopTrain (stationName, trainType,stopTimes,trainStopTime, totalTakeTime,averageTakeTime)VALUES("
                    + "'" + stationName + "',"
                    + "'" + trainType + "',"
                    + "'" + stopTimes + "',"
                    + "'" + trainStopTime + "',"
                    + "'" + totalTakeTime + "',"
                    + "'" + averageTakeTime + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool addOutStationLine_track(string stationName, string trackName, string takeNum, string averageTakeNum, string totalTakeTime, string averageTakeTime,
            string takeRate, string modifyTakeRate,string n)
        {
            if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO outStationLine_track (stationName, trackName,takeNum,averageTakeNum, totalTakeTime,averageTakeTime," +
                "takeRate,modifyTakeRate,n)VALUES("
                    + "'" + stationName + "',"
                    + "'" + trackName + "',"
                    + "'" + takeNum + "',"
                    + "'" + averageTakeNum + "',"
                    + "'" + totalTakeTime + "',"
                    + "'" + averageTakeTime + "',"
                    + "'" + takeRate + "',"
                    + "'" + modifyTakeRate + "',"                   
                    + "'" + n + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool addOutStationThroat_track(string stationName, string groupTrack, string takeNum, string averageTakeNum, string totalTakeTime, string averageTakeTime,
           string takeRate, string modifyTakeRate, string n)
        {
            if(DataPath.AssessOrSqlServer=="sqlServer")dbTrans = new RailStuDb_ForSql();
            strSql = "INSERT INTO outStationThroat (stationName, groupTrack,takeNum,averageTakeNum, totalTakeTime,averageTakeTime," +
                "takeRate,modifyTakeRate,n)VALUES("
                    + "'" + stationName + "',"
                    + "'" + groupTrack + "',"
                    + "'" + takeNum + "',"
                    + "'" + averageTakeNum + "',"
                    + "'" + totalTakeTime + "',"
                    + "'" + averageTakeTime + "',"
                    + "'" + takeRate + "',"
                    + "'" + modifyTakeRate + "',"
                    + "'" + n + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region"批处理 区间数据 导出数据"

        public static bool addOutSectionMove(string stationCode, string sectionName, string upOrDown, string fromStation, string toStation, string locomotiveName, string trainType,string mainOrSide,string standardType,
           string dataNum, string average, string mainLineNum, string mainLineAverage,string yellowNum,string yellowAverage,string sideLineNum,string sideLineAverage,string wNum,string wAverage,string pNum,string pAverage)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO outSectionMove (stationCode, sectionName, upOrDown,fromStation,toStation, locomotiveName,trainType,mainOrSide,standardType," +
                "dataNum,average,mainLineNum,mainLineAverage,yellowNum,yellowAverage,sideLineNum,sideLineAverage,wNum,wAverage,pNum,pAverage)VALUES("
                    + "'" + stationCode + "',"
                    + "'" + sectionName + "',"
                    + "'" + upOrDown + "',"
                    + "'" + fromStation + "',"
                    + "'" + toStation + "',"
                    + "'" + locomotiveName + "',"
                    + "'" + trainType + "',"
                    + "'" + mainOrSide + "',"
                    + "'" + standardType + "',"
                    + "'" + dataNum + "',"
                    + "'" + average + "',"
                    + "'" + mainLineNum + "',"
                    + "'" + mainLineAverage + "',"
                    + "'" + yellowNum + "',"
                    + "'" + yellowAverage + "',"
                    + "'" + sideLineNum + "',"
                    + "'" + sideLineAverage + "',"
                    + "'" + wNum + "',"
                    + "'" + wAverage + "',"
                    + "'" + pNum + "',"               
                    + "'" + pAverage + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion



        public static void updateBXDATA_serialNum(double fileID, string stationName, int serialNum)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "UPDATE BX_DATA SET serialNum='" + serialNum + "'"
            + " WHERE fileID='" + fileID + "' and stationName=" + "'" + stationName + "'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }

        public static bool insertOneSectionName(string upOrDown, string sectionName)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO sectionList (upOrDown, sectionName)VALUES("
                    
                    + "'" + upOrDown + "',"
                    + "'" + sectionName + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool insertOneStationName(string lineName, string stationName)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "INSERT INTO stationList (lineName, stationName,stationSort,useOrNot)VALUES("

                    + "'" + lineName + "',"
                    + "'" + stationName + "',"
                    + "'" + "" + "',"
                    + "'" + "yes" + "')";

            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void updateTrainIntervalList_actionTime(string  fileID,string actionTime, string fromStation, string nowStation,string nextStation)
        {
            if (DataPath.AssessOrSqlServer == "sqlServer") dbTrans = new RailStuDb_ForSql();
            else dbTrans = new RailStuDb_ForAccess();
            strSql = "UPDATE trainIntervalList SET actionTime='" + actionTime + "'"
            + " WHERE fileID='" + fileID + "' and fromStation=" + "'" + fromStation + "'" + " and nowStation='"+nowStation+"' and nextStation='"+nextStation+"'";
            try
            {
                dbTrans.ExecuteSqlCmd(strSql);
            }
            catch
            {
                throw new Exception("更新失败！请重试！");
            }
        }

    }
}
