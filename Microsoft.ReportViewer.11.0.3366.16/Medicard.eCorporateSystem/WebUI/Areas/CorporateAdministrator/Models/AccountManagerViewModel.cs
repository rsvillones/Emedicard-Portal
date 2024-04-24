﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corelib.Models;

namespace WebUI.Areas.CorporateAdministrator.Models
{
    public class AccountManagerViewModel
    {
        #region -- Properties --

        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<LegacyAccount> Accounts { get; set; }

        #endregion
    }
}