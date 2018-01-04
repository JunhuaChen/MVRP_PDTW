using System;
using System.Collections.Generic;
using System.Text;

namespace MvrpLite.data
{
    public class StrConn
    {        
        private string m_StrAccess;
        private string m_StrSql;
        private string m_StrSqlOut;
        public StrConn(string dbName)
        {                       
           
            //m_StrAccess = "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + DataPath.ThisPath + "Debug//data//MvrpLite.mdb;";           

            m_StrAccess = "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + @"data\MvrpLite.mdb;"; 
            m_StrSql = @"User Instance=False; Persist Security Info=False;UID=MvrpLite;Password=MvrpLitebesthua;Server="+DataPath.ThisIp+@";Initial Catalog=MvrpLite";
            m_StrSqlOut = @"User Instance=False; Persist Security Info=False;UID=dmcl;Password=dmclbesthua;Server=" + DataPath.LocomotiveIp + @";Initial Catalog=dmcl2001";
            //m_StrSqlOut = @"server=" + DataPath.UserID + @"\SQLEXPRESS;trusted_connection=true;database=dmcl2001;uid=" + DataPath.UserID;

            if (DataPath.LocalOrNet == "local")
            {
                m_StrSql = @"server=" + DataPath.UserID + @";trusted_connection=true;database=MvrpLite;uid=" + DataPath.UserID;
                m_StrSqlOut = @"server=" + DataPath.UserID + @"\SQLEXPRESS;trusted_connection=true;database=dmcl2001;uid=" + DataPath.UserID;
            }
                //if(DataPath.AssessOrSqlServer=="assess")
            //    m_StrSql = "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + DataPath.ThisPath + "noUse//MvrpLiteData.mdb;"; 
        }
        public string StrAccess
        {
            get
            {
                return m_StrAccess;
            }
        }
        public string StrSql
        {
            get
            {
                return m_StrSql;
            }
        }
        public string StrSqlOut
        {
            get
            {
                return m_StrSqlOut;
            }
        }       
    }

    /// <summary>
    /// Ô­Ê¼Â·¾¶
    /// </summary>
    public static class DataPath
    {
        private static string customerPath;
        private static string currentPath;
        private static string thisIp;
        private static string locomotiveIp;
        private static string localOrNet;
        private static string assessOrSqlServer;
        private static string userID;
        private static string isAdminOrNot;
        public static string CustomerPath
        {
            get
            {
                return customerPath;
            }
            set
            {
                customerPath = value;
            }
        }
        public static string CurrentPath
        {
            get
            {
                return currentPath;
            }
            set
            {
                currentPath = value;
            }
        }
        public static string ThisIp
        {
            get
            {
                return thisIp;
            }
            set
            {
                thisIp = value;
            }
        }
        public static string LocomotiveIp
        {
            get
            {
                return locomotiveIp;
            }
            set
            {
                locomotiveIp = value;
            }
        }
        public static string LocalOrNet
        {
            get
            {
                return localOrNet;
            }
            set
            {
                localOrNet = value;
            }
        }
        public static string UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }
        }
        public static string AssessOrSqlServer
        {
            get
            {
                return assessOrSqlServer;
            }
            set
            {
                assessOrSqlServer = value;
            }
        }
        public static string IsAdminOrNot
        {
            get
            {
                return isAdminOrNot;
            }
            set
            {
                isAdminOrNot = value;
            }
        }
    }
}
