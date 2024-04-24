﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using WebUI.Models;
using Corelib.Models;
using Corelib;
using System.Text.RegularExpressions;

namespace WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var model = new LoginViewModel()
            {
                UserName = "admin",
                Password = "P@ssw0rd"
            };
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user;
                bool errorMessageAdded = false;
                using (var db = new IdentityDataContext())
                {
                    user = db.Users.FirstOrDefault(t => t.UserName == model.UserName);
                }

                if (user != null)
                {
                    if (user.UseActiveDirectory)
                    {
                        string errorMessage;
                        if (!LdapAuthentication.AuthenticateUser(model.UserName, model.Password, out errorMessage))
                        {
                            ModelState.AddModelError("", errorMessage);
                            user = null;
                            errorMessageAdded = true;
                        }
                    }
                    else
                    {
                        user = await UserManager.FindAsync(model.UserName, model.Password);
                    }
                }
                
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);

                    if (!user.AcceptedMemorandumOfAgreement)
                    {
                        if (!String.IsNullOrEmpty(returnUrl))
                        {
                            Regex regex = new Regex("^/(?<Controller>[^/]*)(/(?<Action>[^/]*)(/(?<id>[^?]*)(/(?<QueryString>.*))?)?)?$");
                            Match match = regex.Match(returnUrl);
                            if (returnUrl.Contains("?"))
                            {
                                returnUrl += "&ShowMemorandumOfAgreement=true";
                            }
                            else
                            {
                                returnUrl += "?ShowMemorandumOfAgreement=true";
                            }
                        }
                        else
                        {
                            returnUrl = "/?ShowMemorandumOfAgreement=true";
                        }
                    }

                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    if (!errorMessageAdded)
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        [ChildActionOnly]
        public ActionResult UserInformation()
        {
            return PartialView("_UserInformation", this.UserManager.FindByName(User.Identity.Name));
        }

        [ChildActionOnly]
        public ActionResult MemberMenu()
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.UserName == HttpContext.User.Identity.Name);
                if (member == null) return null;
                ViewBag.AccountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == member.AccountCode) ?? new AccountSetting();

                return PartialView("_MemberMenu", member);
            }

        }

        public void Accept()
        {
            using (var db = new IdentityDataContext())
            {
                db.User = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
                var user = db.Users.FirstOrDefault(t => t.UserName == User.Identity.Name);
                if (user != null)
                {
                    user.AcceptedMemorandumOfAgreement = true;
                    db.SaveChanges();
                }
            }
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}