﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using Corelib.Models;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Mvc.Mailer;
using System.ComponentModel;

namespace Corelib.Classes
{
    public static class Extension
    {
        #region -- Mailer Extensions --

        public static void SendEmailAsync(this MvcMailMessage msg, HttpContext context)
        {
            Task.Factory.StartNew(() =>
            {
                HttpContext.Current = context;
                msg.SendAsync();
            });
        }

        #endregion
        
    }
}