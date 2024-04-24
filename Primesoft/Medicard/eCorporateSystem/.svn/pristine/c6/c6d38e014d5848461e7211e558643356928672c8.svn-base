using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    [Authorize(Roles = "SysAd, CanViewBenefitsAndExclusions")]
    public class BenefitsAndExclusionsController : BaseAccountController
    {
        #region -- Action Results --

        public ActionResult Index(string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            return View();
        }

        public void ProcessEmail()
        {
            var actionMemo = db.ActionMemos.FirstOrDefault(t => t.Id == 7);
            Helper.CorpAdminActionMemo(System.Web.HttpContext.Current, actionMemo, "02202003-000260");
        }
        
        
        #endregion
    }
}