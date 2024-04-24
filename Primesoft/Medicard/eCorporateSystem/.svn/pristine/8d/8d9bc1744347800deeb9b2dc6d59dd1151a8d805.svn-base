using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using Corelib;


namespace WebUI
{
    public class LdapAuthentication
    {
        #region -- Variable Declarations --

        private string _path;
        private string _filterAttribute;

        #endregion

        #region -- Constructor --

        public LdapAuthentication(string path)
        {
            _path = path;
        }

        #endregion

        #region -- Functions --

        public bool IsAuthenticated(string domain, string username, string password, out string errorMessage)
        {
            var domainAccount = String.Format(@"{0}\{1}", domain, username);
            var directoryEntry = new DirectoryEntry(_path, domainAccount, password);
            errorMessage = "";

            try
            {
                object obj = directoryEntry.NativeObject;
                var directorySearcher = new DirectorySearcher(directoryEntry)
                {
                    Filter = String.Format(@"(SAMAccountName={0})", username)
                };

                directorySearcher.PropertiesToLoad.Add("cn");

                var searchResult = directorySearcher.FindOne();

                if (searchResult == null)
                {
                    errorMessage = "Invalid username or password.";
                    return false;
                }

                _path = searchResult.Path;
                _filterAttribute = (string)searchResult.Properties["cn"][0];

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public static bool AuthenticateUser(string username, string password, out string errorMessage)
        {
            errorMessage = "";
            bool authenticated = Config.AutoLogin;

            if (!authenticated)
            {
                var authentication = new LdapAuthentication(Config.DomainPath);
                authenticated = authentication.IsAuthenticated(Config.Domain, username, password, out errorMessage);
            }

            return authenticated;
        }

        #endregion
    }
}