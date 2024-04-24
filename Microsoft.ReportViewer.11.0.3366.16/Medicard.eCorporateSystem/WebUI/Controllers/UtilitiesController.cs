using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class UtilitiesController : Controller
    {
        #region -- Action Results --

        public int? ComputeAge(DateTime? dateOfBirth)
        {
            if (dateOfBirth.HasValue)
            {
                return Helper.ComputeAge(dateOfBirth.Value);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}