﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Corelib
{
    public static class Config
    {
        #region -- Properties --

        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Medicard.eCorporateSystem"].ConnectionString; }
        }

        public static string CurrentUser
        {
            get
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null)
                {
                    return System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    return "anonymous";
                }
            }
        }

        public static int RecordCountPerPage
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["RecordCountPerPage"]);
            }
        }

        public static string GetCurrentUser(HttpContextBase contextBase)
        {
            var context = contextBase != null ? contextBase.ApplicationInstance.Context : null;
            if (context != null && context.User != null)
            {
                return System.Web.HttpContext.Current.User.Identity.Name;
            }
            else
            {
                return "anonymous";
            }
        }

        public static string NotificationFromEmail
        {
            get { return ConfigurationManager.AppSettings["NotificationFromEmail"]; }
        }

        public static string Domain
        {
            get { return ConfigurationManager.AppSettings["Domain"]; }
        }

        public static string DomainPath
        {
            get { return ConfigurationManager.AppSettings["DomainPath"]; }
        }

        public static bool AutoLogin
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["AutoLogin"]); }
        }

        public static string EmailDomain
        {
            get { return ConfigurationManager.AppSettings["EmailDomain"]; }
        }

        public static void MapProperties(object source, object destination, bool mapId = true, params string[] propertiesToExclude)
        {
            foreach (var pi in destination.GetType().GetProperties())
            {
                if (!pi.CanWrite || source.GetType().GetProperty(pi.Name) == null || (!mapId && pi.Name == "Id") || (propertiesToExclude != null && propertiesToExclude.Contains(pi.Name)))
                    continue;

                destination.GetType().GetProperty(pi.Name).SetValue(destination, source.GetType().GetProperty(pi.Name).GetValue(source));
            }
        }

        #endregion
    }
}
