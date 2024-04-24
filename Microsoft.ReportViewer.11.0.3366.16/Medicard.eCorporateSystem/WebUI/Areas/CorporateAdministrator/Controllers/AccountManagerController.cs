﻿using Corelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using WebUI.Areas.CorporateAdministrator.Models;

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    [Authorize(Roles = "SysAd, CanViewAccountManager")]
    public class AccountManagerController : Controller
    {
        #region -- Variable Declarations --

        private IdentityDataContext db = new IdentityDataContext();

        #endregion

        #region -- Constructor --

        public AccountManagerController()
        {
            db.User = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
        }

        #endregion

        #region -- Action Results --

        [Authorize(Roles = "SysAd, CanViewAccountManager")]
        public async Task<ActionResult> Index()
        {
            var user = await db.Users.FirstOrDefaultAsync(t => t.UserName == User.Identity.Name);
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var model = new AccountManagerViewModel()
            {
                ApplicationUser = user,
                Accounts = Helper.GetLegacyAccounts(db, null)
            };

            return View(model);
        }
        
        #endregion

        #region -- Overrides --

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

    }
}