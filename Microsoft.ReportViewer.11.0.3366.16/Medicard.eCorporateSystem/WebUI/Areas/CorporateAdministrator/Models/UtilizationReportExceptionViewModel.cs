﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corelib.Models;

namespace WebUI.Areas.CorporateAdministrator.Models
{
    public class UtilizationReportExceptionViewModel
    {
        #region -- Properties --

        public ICollection<UtilizationReportException> UtilizationReportExceptions { get; set; }

        #endregion
    }
}