﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corelib.Models;

namespace WebUI.Areas.Member.Models
{
    public class DependentViewModel
    {
        #region -- Properties --

        public ICollection<Dependent> Dependents { get; set; }

        #endregion
    }
}