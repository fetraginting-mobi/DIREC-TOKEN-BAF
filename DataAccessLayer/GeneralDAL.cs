using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;


namespace DataAccessLayer
{
    public class GeneralDAL
    {
        private string _ConnectionString;

        public GeneralDAL(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        #region Insert
        public void Insert(string TableName, Hashtable parameters, ref int id)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP("xsp_" + TableName + "_insert", parameters, ref id))
            {
                throw new Exception("Fail to execute xsp_" + TableName + "_insert", new Exception(dbw.DBErrorMessage));
            }
        }

        public void Insert(string TableName, Hashtable parameters, ref string id)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP("xsp_" + TableName + "_insert", parameters, ref id))
            {
                throw new Exception("Fail to execute xsp_" + TableName + "_insert", new Exception(dbw.DBErrorMessage));
            }
        }

        public void Insert(string TableName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP("xsp_" + TableName + "_insert", parameters))
            {
                throw new Exception("Fail to execute xsp_" + TableName + "_insert", new Exception(dbw.DBErrorMessage));
            }
        }

        public void Insert(string TableName, string SPName, Hashtable parameters, ref int id)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP(SPName, parameters, ref id))
            {
                throw new Exception("Fail to execute " + SPName, new Exception(dbw.DBErrorMessage));
            }
        }

        public void Insert(string TableName, string SPName, Hashtable parameters, ref string id)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP(SPName, parameters, ref id))
            {
                throw new Exception("Fail to execute " + SPName, new Exception(dbw.DBErrorMessage));
            }
        }

        public void Insert(string TableName, string SPName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP(SPName, parameters))
            {
                throw new Exception("Fail to execute " + SPName, new Exception(dbw.DBErrorMessage));
            }
        }

        public void Insert(string tableName, Hashtable htParameters, ref long iNextID)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP("xsp_" + tableName + "_insert", htParameters))
            {
                throw new Exception("Fail to execute xsp_" + tableName + "_insert", new Exception(dbw.DBErrorMessage));
            }
            //throw new NotImplementedException();
        }
        #endregion

        #region Update
        public void Update(string TableName, string SPName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP(SPName, parameters))
            {
                throw new Exception("Fail to execute " + SPName, new Exception(dbw.DBErrorMessage));
            }
        }

        public void Update(string TableName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP("xsp_" + TableName + "_update", parameters))
            {

                throw new Exception("Fail to execute xsp_" + TableName + "_update", new Exception(dbw.DBErrorMessage));
            }
        }
        #endregion

        #region Delete
        public void Delete(string TableName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP("xsp_" + TableName + "_delete", parameters))
            {
                throw new Exception("Fail to execute xsp_" + TableName + "_delete", new Exception(dbw.DBErrorMessage));
            }
        }
        #endregion

        #region GetRows
        public DataTable GetRows(string TableName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            DataSet ds = new DataSet();
            dbw.ConnectionString = _ConnectionString;

            //if
            if (parameters["p_keywords"] != null)
                parameters["p_keywords"] = parameters["p_keywords"].ToString().Trim();

            if (!dbw.ExecuteSP("xsp_" + TableName + "_getrows", parameters, ds))
            {
                throw new Exception("Fail to xsp_" + TableName + "_getrows", new Exception(dbw.DBErrorMessage));
            }
            else
            {
                if (ds.Tables.Count <= 0)
                    throw new Exception("Fail to xsp_" + TableName + "_getrows. No row found.");
                else
                    return ds.Tables[0];
            }
        }

        public DataTable GetRows(string TableName, string SPName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            DataSet ds = new DataSet();
            dbw.ConnectionString = _ConnectionString;

            if (parameters["p_keywords"] != null)
                parameters["p_keywords"] = parameters["p_keywords"].ToString().Trim();

            if (!dbw.ExecuteSP(SPName, parameters, ds))
            {
                throw new Exception("Fail to " + SPName, new Exception(dbw.DBErrorMessage));
            }
            else
            {
                if (ds.Tables.Count <= 0)
                    throw new Exception("Fail to " + SPName + ". No row found.");
                else
                    return ds.Tables[0];
            }
        }
        #endregion

        #region GetRow
        public DataTable GetRow(string TableName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            DataSet ds = new DataSet();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP("xsp_" + TableName + "_getrow", parameters, ds))
            {
                throw new Exception("Fail to execute xsp_" + TableName + "_getrow", new Exception(dbw.DBErrorMessage));
            }
            else
            {
                if (ds.Tables.Count <= 0)
                    throw new Exception("Fail to xsp_" + TableName + "_getrow. No row found.");
                else
                    return ds.Tables[0];
            }
        }

        public DataTable GetRow(string TableName, string SPName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            DataSet ds = new DataSet();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP(SPName, parameters, ds))
            {
                throw new Exception("Fail to execute " + SPName, new Exception(dbw.DBErrorMessage));
            }
            else
            {
                if (ds.Tables.Count <= 0)
                    throw new Exception("Fail to " + SPName + ". No row found.");
                else
                    return ds.Tables[0];
            }
        }
        #endregion

        public void ExecRawSP(string SPName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP(SPName, parameters))
            {
                throw new Exception("Fail to execute " + SPName, new Exception(dbw.DBErrorMessage));
            }
        }

        public void ExecRawSP(string SPName, Hashtable parameters, ref string ReturnValue)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            dbw.ConnectionString = _ConnectionString;
            if (!dbw.ExecuteSP(SPName, parameters, ref ReturnValue))
            {
                throw new Exception("Fail to execute " + SPName, new Exception(dbw.DBErrorMessage));
            }
        }

        public DataTable ExecuteExcelReport(string spReportName, string spResultName, Hashtable parameters)
        {
            DBWrapper dbw = DBWrapper.GetSqlClientWrapper();
            DataSet ds = new DataSet();

            dbw.ConnectionString = _ConnectionString;

            if (spReportName != null && spResultName != null)
            {
                if (dbw.ExecuteSP(spReportName, parameters))
                {

                    if (!dbw.ExecuteSP(spResultName, parameters, ds))
                    {
                        throw new Exception(spResultName, new Exception(dbw.DBErrorMessage));
                    }
                    else
                    {
                        if (ds.Tables.Count <= 0)
                            throw new Exception("Fail to " + spResultName + ". No row found.");
                        else
                            return ds.Tables[0];
                    }
                }
                else
                {
                    throw new Exception(spReportName);
                }
            }
            else if (spReportName == null)
            {
                if (!dbw.ExecuteSP(spResultName, parameters, ds))
                {
                    throw new Exception(spResultName, new Exception(dbw.DBErrorMessage));
                }
                else
                {
                    if (ds.Tables.Count <= 0)
                        throw new Exception("Fail to " + spResultName + ". No row found.");
                    else
                        return ds.Tables[0];
                }
            }
            else if (spResultName == null)
            {
                if (!dbw.ExecuteSP(spReportName, parameters))
                {
                    throw new Exception(spReportName, new Exception(dbw.DBErrorMessage));
                }
                else
                    return null;
            }
            else return null;
        }
    }
}
