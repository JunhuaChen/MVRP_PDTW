using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
namespace MvrpLite.data
{
    public interface IRailStuDb
    {
        /// <summary>
        /// Execute Sql Commands
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>int</returns>
        int ExecuteSqlCmd(string strSql);

        /// <summary>
        /// Excute SQL Commands to reader. Just for query use.
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>int</returns>
        int ExecuteSqlCmdEx(string strSql);

        /// <summary>
        /// get dataSet
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>dataset</returns>
        System.Data.DataSet ExecuteSqlForDs(string strSql);

        /// <summary>
        /// Just get a single value
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        int ExecuteSqlForValue(string strSQL);

        /// <summary>
        /// Return for an object
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>object</returns>
        object ExecuteSqlForValueEx(string strSQL);

        /// <summary>
        /// Execute more than one sql commands
        /// </summary>
        /// <returns>int</returns>
        int ExecuteSqls(string[] strSQLs);
    }
       
    /// <summary>
    /// RailStuDb_Sql 的摘要说明。
    /// </summary>
    public abstract class RailStuDb_Sql
    {


        #region "Functions of BaseClass"

        /// <summary>
        /// Execute SQL Commands
        /// </summary>
        /// <param name = "strSQL"> string </param>
        /// <return> int </return>
        public static int ExecuteSqlCmd(string strConn, string strSQL)
        {

            SqlConnection currentConn = new SqlConnection(strConn);
            SqlCommand currentCmd = new SqlCommand(strSQL, currentConn);
            try
            {
                currentConn.Open();
                currentCmd.ExecuteNonQuery();
                return 0;
            }

                // In case of some exception

            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }

                // We have to close the connection at last correctly.

            finally
            {
                currentCmd.Dispose();
                currentConn.Close();
            }
        }

        /// <summary>
        /// Excute SQL Commands to reader. Just for query use.
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public static int ExecuteSqlCmdEx(string strConn, string strSQL)
        {
            SqlConnection currentConn = new SqlConnection(strConn);
            SqlCommand currentCmd = new SqlCommand(strSQL, currentConn);
            try
            {
                currentConn.Open();

                // Just for query, so we use SqlDataReader here.

                SqlDataReader currentReader = currentCmd.ExecuteReader();
                if (currentReader.Read())
                {
                    return 0;
                }
                else
                {
                    throw new Exception("Value Unavailable!");
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                currentCmd.Dispose();
                currentConn.Close();

            }
        }

        /// <summary>
        /// Get DataSet
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteSqlForDS(string strConn, string strSQL)
        {
            SqlConnection currentConn = new SqlConnection(strConn);
            try
            {
                currentConn.Open();
                SqlDataAdapter currentDA = new SqlDataAdapter(strSQL, currentConn);
                DataSet ds = new DataSet("ds");
                currentDA.Fill(ds);
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                currentConn.Close();
            }
        }

        /// <summary>
        /// Just get a single value
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public static int ExecuteSqlForValue(string strConn, string strSQL)
        {
            SqlConnection currentConn = new SqlConnection(strConn);
            SqlCommand currentCmd = new SqlCommand(strSQL, currentConn);

            try
            {
                currentConn.Open();
                // Only return the first line of the results
                object ret = currentCmd.ExecuteScalar();
                if (Object.Equals(ret, null))
                {
                    throw new Exception("Value unavailable!");
                }
                else
                {
                    return (int)ret;
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                currentCmd.Dispose();
                currentConn.Close();
            }
        }

        /// <summary>
        /// Return for an object
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>object</returns>
        public static Object ExecuteSqlForValueEx(string strConn, string strSQL)
        {
            SqlConnection currentConn = new SqlConnection(strConn);
            SqlCommand currentCmd = new SqlCommand(strSQL, currentConn);
            try
            {
                currentConn.Open();
                object ret = currentCmd.ExecuteScalar();
                if (Object.Equals(ret, null))
                {
                    throw new Exception("Value unavailable!");
                }
                else
                {
                    return ret;
                }
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                currentCmd.Dispose();
                currentConn.Close();
            }
        }

        /// <summary>
        /// Execute more than one sql commands
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public static int ExecuteSqls(string strConn, string[] strSQLs)
        {
            SqlConnection currentConn = new SqlConnection(strConn);
            SqlCommand currentCmd = new SqlCommand();
            int i = strSQLs.Length;

            try
            {
                currentConn.Open();
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }


            SqlTransaction currentTran = currentConn.BeginTransaction();

            try
            {
                currentCmd.Connection = currentConn;
                currentCmd.Transaction = currentTran;
                foreach (string str in strSQLs)
                {
                    currentCmd.CommandText = str;
                    currentCmd.ExecuteNonQuery();
                }
                currentTran.Commit();
                return 0;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                currentTran.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                currentCmd.Dispose();
                currentConn.Close();
            }
        }
        #endregion

    }


    /// <summary>
    /// RailStuDb_Access 的摘要说明。
    /// </summary>
    public abstract class RailStuDb_Access
    {


        #region "Functions of BaseClass"

        /// <summary>
        /// Execute SQL Commands
        /// </summary>
        /// <param name = "strSQL"> string </param>
        /// <return> int </return>
        public static int ExecuteSqlCmd(string strConn, string strSQL)
        {

            OleDbConnection currentConn = new OleDbConnection(strConn);
            OleDbCommand currentCmd = new OleDbCommand(strSQL, currentConn);
            try
            {
                currentConn.Open();
                currentCmd.ExecuteNonQuery();
                return 0;
            }

                // In case of some exception

            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }

                // We have to close the connection at last correctly.

            finally
            {
                currentCmd.Dispose();
                currentConn.Close();
            }
        }

        /// <summary>
        /// Excute SQL Commands to reader. Just for query use.
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public static int ExecuteSqlCmdEx(string strConn, string strSQL)
        {
            OleDbConnection currentConn = new OleDbConnection(strConn);
            OleDbCommand currentCmd = new OleDbCommand(strSQL, currentConn);
            try
            {
                currentConn.Open();

                // Just for query, so we use SqlDataReader here.

                OleDbDataReader currentReader = currentCmd.ExecuteReader();
                if (currentReader.Read())
                {
                    return 0;
                }
                else
                {
                    throw new Exception("Value Unavailable!");
                }
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                currentCmd.Dispose();
                currentConn.Close();

            }
        }

        /// <summary>
        /// Get DataSet
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteSqlForDS(string strConn, string strSQL)
        {
            OleDbConnection currentConn = new OleDbConnection(strConn);
            try
            {
                currentConn.Open();
                OleDbDataAdapter currentDA = new OleDbDataAdapter(strSQL, currentConn);
                DataSet ds = new DataSet("ds");
                currentDA.Fill(ds);
                return ds;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                currentConn.Close();
            }
        }

        /// <summary>
        /// Just get a single value
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public static int ExecuteSqlForValue(string strConn, string strSQL)
        {
            OleDbConnection currentConn = new OleDbConnection(strConn);
            OleDbCommand currentCmd = new OleDbCommand(strSQL, currentConn);

            try
            {
                currentConn.Open();
                // Only return the first line of the results
                object ret = currentCmd.ExecuteScalar();
                if (Object.Equals(ret, null))
                {
                    throw new Exception("Value unavailable!");
                }
                else
                {
                    return (int)ret;
                }
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                currentCmd.Dispose();
                currentConn.Close();
            }
        }

        /// <summary>
        /// Return for an object
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>object</returns>
        public static Object ExecuteSqlForValueEx(string strConn, string strSQL)
        {
            OleDbConnection currentConn = new OleDbConnection(strConn);
            OleDbCommand currentCmd = new OleDbCommand(strSQL, currentConn);
            try
            {
                currentConn.Open();
                object ret = currentCmd.ExecuteScalar();
                if (Object.Equals(ret, null))
                {
                    throw new Exception("Value unavailable!");
                }
                else
                {
                    return ret;
                }
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                currentCmd.Dispose();
                currentConn.Close();
            }
        }

        /// <summary>
        /// Execute more than one sql commands
        /// </summary>
        /// <param name="strSQL">string</param>
        /// <returns>int</returns>
        public static int ExecuteSqls(string strConn, string[] strSQLs)
        {
            OleDbConnection currentConn = new OleDbConnection(strConn);
            OleDbCommand currentCmd = new OleDbCommand();
            int i = strSQLs.Length;

            try
            {
                currentConn.Open();
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                throw new Exception(e.Message);
            }


            OleDbTransaction currentTran = currentConn.BeginTransaction();

            try
            {
                currentCmd.Connection = currentConn;
                currentCmd.Transaction = currentTran;
                foreach (string str in strSQLs)
                {
                    currentCmd.CommandText = str;
                    currentCmd.ExecuteNonQuery();
                }
                currentTran.Commit();
                return 0;
            }
            catch (System.Data.OleDb.OleDbException e)
            {
                currentTran.Rollback();
                throw new Exception(e.Message);
            }
            finally
            {
                currentCmd.Dispose();
                currentConn.Close();
            }
        }
        #endregion

    }
}
