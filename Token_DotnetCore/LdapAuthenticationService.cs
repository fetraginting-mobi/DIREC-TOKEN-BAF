using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Token_Ifinsys.Interfaces;
using Token_Ifinsys.Models;
using System.DirectoryServices;

namespace Token_Ifinsys
{
    public class LdapAuthenticationService : IAuthenticationService
    {
        private const string DisplayNameAttribute = "DisplayName";
        private const string SAMAccountNameAttribute = "SAMAccountName";

        private readonly LdapConfig config;

        public LdapAuthenticationService(IOptions<LdapConfig> config)
        {
            this.config = config.Value;
        }

        public User Login(string userName, string password)
        {
            try
            {
                //using (DirectoryEntry entry = new DirectoryEntry(config.Path, config.UserDomainName + "\\" + userName, password))
                using (DirectoryEntry entry = new DirectoryEntry(config.Path, userName, password))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = String.Format("({0}={1})", SAMAccountNameAttribute, userName);
                        searcher.PropertiesToLoad.Add(DisplayNameAttribute);
                        searcher.PropertiesToLoad.Add(SAMAccountNameAttribute);
                        var result = searcher.FindOne();
                        if (result != null)
                        {
                            var displayName = result.Properties[DisplayNameAttribute];
                            var samAccountName = result.Properties[SAMAccountNameAttribute];

                            return new User
                            {
                                DisplayName = displayName == null || displayName.Count <= 0 ? null : displayName[0].ToString(),
                                UserName = samAccountName == null || samAccountName.Count <= 0 ? null : samAccountName[0].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.Message.ToString();
                // if we get an error, it means we have a login failure.
                // Log specific exception
            }
            return null;
        }


        //public static bool CheckLDAP(string user, string pass)
        //{
        //    string LDAPServer = config.Path;
        //    DirectoryEntry entry = new DirectoryEntry(LDAPServer, user, pass);
        //    DirectorySearcher ds = new DirectorySearcher(entry);

        //    try
        //    {
        //        ds.FindOne();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        CreateLog("Error Check LDAP at " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + ", info: " + ex.ToString(), "ldap_log_");
        //        return false;
        //    }
        //}

        //public static void CreateLog(string log, string logfilename)
        //{
        //    FileStream fs = null;
        //    StreamWriter sw = null;

        //    // for avoid new file in the same time
        //    Guid g = Guid.NewGuid();
        //    string RandomString = g.ToString();
        //    RandomString = RandomString.Replace("=", "");
        //    RandomString = RandomString.Replace("+", "");
        //    RandomString = RandomString.Replace("-", "");
        //    RandomString = RandomString.Replace("_", "");

        //    string logname = logfilename + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + RandomString + ".txt";
        //    string filepath = @"C:\inetpub\wwwroot\efam\temp\txt\" + logname;

        //    try
        //    {

        //        fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);

        //        sw = new StreamWriter(fs);
        //        sw.WriteLine(log);

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        if (sw != null)
        //        {
        //            sw.Close();
        //            sw.Dispose();
        //        }

        //        if (fs != null)
        //        {
        //            fs.Close();
        //            fs.Dispose();
        //        }
        //    }
        //}
    }
}
