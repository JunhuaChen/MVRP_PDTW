using System;
using System.Collections.Generic;
using System.Text;

using System.Data;

namespace MvrpLite.data
{
    public class RailStuDb_ForAccess : IRailStuDb
    {
        #region "fields of baseClass"
        protected StrConn strConn = new StrConn("MvrpLiteData");
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
            return RailStuDb_Access.ExecuteSqlCmd(strConn.StrAccess, strSql);

        }
        /// <summary>
        /// Excute SQL Commands to reader. Just for query use.
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>int</returns>
        public int ExecuteSqlCmdEx(string strSql)
        {
            return RailStuDb_Access.ExecuteSqlCmdEx(strConn.StrAccess, strSql);
        }
        /// <summary>
        /// get dataSet
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>dataset</returns>
        public DataSet ExecuteSqlForDs(string strSql)
        {
            return RailStuDb_Access.ExecuteSqlForDS(strConn.StrAccess, strSql);
        }

        /// <summary>
        /// Just get a single value
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public int ExecuteSqlForValue(string strSql)
        {
            return RailStuDb_Access.ExecuteSqlForValue(strConn.StrAccess, strSql);
        }

        /// <summary>
        /// Return for an object
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>object</returns>
        public Object ExecuteSqlForValueEx(string strSql)
        {
            return RailStuDb_Access.ExecuteSqlForValueEx(strConn.StrAccess, strSql);
        }

        /// <summary>
        /// Execute more than one sql commands
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public int ExecuteSqls(string[] strSQLs)
        {
            return RailStuDb_Access.ExecuteSqls(strConn.StrAccess, strSQLs);

        }

        #endregion

    }
}

