﻿using Corelib;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebUI.Areas.CorporateAdministrator.Models;
using WebUI.Models;

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    [Authorize(Roles = "SysAd, CanViewProfileManagement")]
    public class ProfileManagementController : Controller
    {
        #region -- Variable Declarations --

        private IdentityDataContext db = new IdentityDataContext();

        #endregion

        #region -- Constructor --

        public ProfileManagementController()
        {
            db.User = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
        }

        #endregion

        #region -- Action Results --

        [Authorize(Roles = "SysAd, CanViewProfileManagement")]
        public async Task<ActionResult> Index()
        {
            var user = await db.Users.FirstOrDefaultAsync(t => t.UserName == User.Identity.Name);
            if (user == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = new ProfileManagementViewModel();
            Helper.MapProperties(user, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SysAd, CanViewProfileManagement")]
        public async Task<ActionResult> Index([Bind(Include = "Id, Name, Email, PhoneNumber, UserId, UserName, OldPassword, Password, ConfirmPassword, Designation, Mobile, Fax, Address, CrById, AgentCode")] ProfileManagementViewModel applicationUser)
        {
            if (ModelState.IsValid)
            {
                var entity = db.Users.FirstOrDefault(t => t.UserName == User.Identity.Name);
                if (entity != null)
                {
                    var currentPasswordHash = entity.PasswordHash;
                    var currentSecurityStamp = entity.SecurityStamp;

                    db.Entry(entity).CurrentValues.SetValues(applicationUser);

                    if (string.IsNullOrEmpty(applicationUser.Password))
                    {
                        entity.PasswordHash = currentPasswordHash;
                        entity.SecurityStamp = currentSecurityStamp;
                    }

                    db.Entry(entity).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    if (!string.IsNullOrEmpty(applicationUser.Password))
                    {
                        await Helper.UpdateUserPasswordAsync(entity.Id, applicationUser.UserName, applicationUser.Password);
                    }
                    await Helper.LoginUser(entity.Id);
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            return View(applicationUser);
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