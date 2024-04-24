using Corelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    [Authorize(Roles = "SysAd, CanViewAccountInformation")]
    public class AccountInformationController : BaseAccountController
    {
        #region -- Action Results --

        [Authorize(Roles = "SysAd, CanViewAccountInformation")]
        public ActionResult Index(string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            return View();
        }

        

        
        #endregion
    }
}