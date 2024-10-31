using DataAccessLayer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Data;
using Token_Ifinsys.Interfaces;
using System.DirectoryServices;
using System.IO;

namespace Token_Ifinsys
{
    public class UserManagementService : IUserManagementService
    {
        private readonly string _connectionString;
        private readonly string _connectionStringLDAP;
        private readonly string _LDAPActivation;
        //String TableName = "MASTER_ASSET_CATEGORY";
        //private String ConnectionString = "Server=192.168.18.6; Database=IFINSYS; User ID=live; Password=abdi; Connection Timeout=0; Persist Security Info=true";

        public UserManagementService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connectionStringLDAP = configuration.GetConnectionString("LDAPConnection");
            _LDAPActivation = configuration.GetConnectionString("LDAPActivation");
        }

        public bool IsValidUser(string userName, string password, out string messageEx, out DataTable dataList) //string imei
        {
            GeneralDAL _dal = null;
            Hashtable _ht = null;
            DataTable _dt = null;

            messageEx = string.Empty;

            _dal = new GeneralDAL(_connectionString);
            _ht = new Hashtable();

            try
            {
              
                _ht["p_ucode"] = userName;
                _ht["p_password"] = password;
                //_ht["p_imei"] = imei;

                if (_LDAPActivation == "1") // for LDAP
                {
                    if (CheckLDAP(userName, password) == true)
                    {
                        _dt = _dal.GetRows("", "xsp_master_user_main_validate", _ht);

                        messageEx = "Berhasil Login!";
                        dataList = _dt;
                        return true;
                    }
                    else
                    {
                        //throw new Exception("Invalid Username or Password");

                        messageEx = "Invalid Username or Password LDAP";
                        CreateLog(messageEx, "");
                        dataList = _dt;
                        return false;

                    }

                }
                else
                {
                    _dt = _dal.GetRows("", "xsp_master_user_main_validate", _ht);

                    messageEx = "Berhasil Login!";
                    dataList = _dt;
                    return true;
                }


                //_dt = _dal.GetRows("", "xsp_master_user_main_validate", _ht);

                //messageEx = "Berhasil Login!";
                //dataList = _dt;
                //return true;


            }
            catch (Exception ex)
            {   
                messageEx = ex.InnerException.Message.ToString();
                //_dal.ExecRawSP("xsp_master_user_main_validate_update", _ht);
                dataList = _dt;
                return false;
            }


        }

        public bool CheckLDAP(string userName, string password)
        {
            string LDAPServer = _connectionStringLDAP;
            DirectoryEntry entry = new DirectoryEntry(LDAPServer, userName, password);
            DirectorySearcher ds = new DirectorySearcher(entry);

            try
            {
                ds.FindOne();
                return true;
            }
            catch (Exception ex)
            {
                CreateLog("Error Check LDAP at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + ", info: " + ex.ToString(), "ldap_log_");
                return false;
            }
        }


        public static void CreateLog(string log, string logfilename)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            // for avoid new file in the same time
            Guid g = Guid.NewGuid();
            string RandomString = g.ToString();
            RandomString = RandomString.Replace("=", "");
            RandomString = RandomString.Replace("+", "");
            RandomString = RandomString.Replace("-", "");
            RandomString = RandomString.Replace("_", "");

            string logname = logfilename + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + RandomString + ".txt";
            string filepath = @"C:\inetpub\wwwroot\token_baf\temp\log\" + logname;

            try
            {

                fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);

                sw = new StreamWriter(fs);
                sw.WriteLine(log);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }

                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
    }
}
