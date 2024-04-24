using Corelib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Member.Models
{
    public class AdditionalDependentMedicalHistoryViewModel
    {
        #region -- Properties --

        public ICollection<AdditionalDependentMedicalHistory> AdditionalDependentMedicalHistories { get; set; }

        #endregion
    }
}