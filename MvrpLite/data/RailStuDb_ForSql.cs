using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace MvrpLite.data
{
    public class RailStuDb_ForSql : IRailStuDb
    {
        #region "fields of baseClass"
        protected StrConn strConn = new StrConn("");
        protected string strCmd;

        #endregion
        
        #region IRailStuDb ≥…‘±
        /// <summary>
        /// Execute Sql Commands
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>int</returns>
        public int ExecuteSqlCmd(string strSql)
        {
            return RailStuDb_Sql.ExecuteSqlCmd(strConn.StrSql, strSql);
           
        }
        /// <summary>
        /// Excute SQL Commands to reader. Just for query use.
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>int</returns>
        public int ExecuteSqlCmdEx(string strSql)
        {
            return RailStuDb_Sql.ExecuteSqlCmdEx(strConn.StrSql, strSql);
        }
        /// <summary>
        /// get dataSet
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>dataset</returns>
        public DataSet ExecuteSqlForDs(string strSql)
        {
            return RailStuDb_Sql.ExecuteSqlForDS(strConn.StrSql, strSql);
        }

        /// <summary>
        /// Just get a single value
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public int ExecuteSqlForValue(string strSql)
        {
            return RailStuDb_Sql.ExecuteSqlForValue(strConn.StrSql, strSql);
        }

        /// <summary>
        /// Return for an object
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>object</returns>
        public Object ExecuteSqlForValueEx(string strSql)
        {
            return RailStuDb_Sql.ExecuteSqlForValueEx(strConn.StrSql, strSql);
        }

        /// <summary>
        /// Execute more than one sql commands
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public int ExecuteSqls(string[] strSQLs)
        {
            return RailStuDb_Sql.ExecuteSqls(strConn.StrSql, strSQLs);
            //return -1;
        }

 #endregion
                  
    }
}
