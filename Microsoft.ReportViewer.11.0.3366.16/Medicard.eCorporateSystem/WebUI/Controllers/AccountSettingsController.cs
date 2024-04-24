﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using Corelib;
using Corelib.Models;
using PagedList;
using PagedList.Mvc;
using WebUI.Models;
using Corelib.Enums;

namespace WebUI.Controllers
{
    [Authorize(Roles = "SysAd, CanViewAccountSettings, CanEditAccountSettings")]
    public class AccountSettingsController : Controller
    {
        #region -- Variable Declarations --

        private IdentityDataContext db = new IdentityDataContext();
        private LegacyDataContext legacyDb = new LegacyDataContext();

        #endregion

        #region -- Constructor --

        public AccountSettingsController()
        {
            db.User = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
        }

        #endregion

        #region -- Action Results --

        [Authorize(Roles = "SysAd, CanViewAccountSettings")]
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            var legacyAccounts = Helper.GetLegacyAccounts(db, legacyDb);

            var listAccountSettings = db.AccountSettings.Where(t => !t.Deleted).ToList();

            foreach (var legacyAccount in legacyAccounts)
            {
                if (!listAccountSettings.Any(t => t.AccountCode == legacyAccount.Code))
                {
                    var accountSetting = new AccountSetting()
                    {
                        AccountCode = legacyAccount.Code,
                        AccountName = legacyAccount.Name
                    };
                    db.AccountSettings.Add(accountSetting);
                    listAccountSettings.Add(accountSetting);
                }
            }

            db.SaveChanges();
            var accountSettings = AccountSettings(listAccountSettings);

            IsReadOnlyAttribute();

            if (!string.IsNullOrEmpty(currentFilter))
            {
                accountSettings = accountSettings.Where(t => t.AccountCode.Contains(currentFilter));
            }

            Helper.SetSortParameters<AccountSetting>(this, ref accountSettings, sortOrder, currentFilter, new SortParameter() { PropertyName = "AccountCode" }, new List<SortParameter>() { });

            return View(accountSettings.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }

        [Authorize(Roles = "SysAd, CanEditAccountSettings")]
        public ActionResult EditUrgSetting(Guid guid, string accountCode, string propertyValue,string accountName)
        {
            AccountSetting accountSetting;
            accountSetting = db.AccountSettings.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            var urgSetting = (UrgSetting)Enum.Parse(typeof(UrgSetting), propertyValue);
            if (accountSetting != null)
            {
                accountSetting.UrgSetting = urgSetting;
                db.Entry(accountSetting).State = EntityState.Modified;
            }
            db.SaveChanges();

            IsReadOnlyAttribute();

            return PartialView("_AccountSetting", accountSetting);
        }

        [Authorize(Roles = "SysAd, CanEditAccountSettings")]
        public ActionResult EditDomainEmail(Guid guid, string accountCode, string propertyValue, string accountName)
        {
            AccountSetting accountSetting;
            accountSetting = db.AccountSettings.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (accountSetting != null)
            {
                accountSetting.DomainEmail = propertyValue;
                db.Entry(accountSetting).State = EntityState.Modified;
            }
            db.SaveChanges();

            IsReadOnlyAttribute();

            return PartialView("_AccountSetting", accountSetting);

        }

        [Authorize(Roles = "SysAd, CanEditAccountSettings")]
        public ActionResult Edit(Guid guid, bool propertyValue, string propertyName, string accountCode, string accountName)
        {
            AccountSetting accountSetting;
            accountSetting = db.AccountSettings.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (accountSetting != null)
            {
                SetAccountSetting(ref accountSetting, propertyValue, propertyName);
                db.Entry(accountSetting).State = EntityState.Modified;
            }
            db.SaveChanges();

            IsReadOnlyAttribute();

            return PartialView("_AccountSetting", accountSetting);
        }

        #endregion

        #region -- Functions --

        private IQueryable<AccountSetting> AccountSettings(List<AccountSetting> accountSettings)
        {
            return accountSettings.AsQueryable();
        }

        private void SetAccountSetting(ref AccountSetting accountSetting, bool propertyValue, string propertyName)
        {
            switch (propertyName)
            {
                case "UseEmailAsLogin":
                    accountSetting.UseEmailAsLogin = propertyValue;
                    break;
                case "UseRandomGeneratedPassword":
                    accountSetting.UseRandomGeneratedPassword = propertyValue;
                    break;
                case "BypassHRManagerApproval":
                    accountSetting.BypassHRManagerApproval = propertyValue;
                    break;
                case "BypassMedicalHistory":
                    accountSetting.BypassMedicalHistory = propertyValue;
                    break;
                default:
                    break;
            }
        }

        public ActionResult IsReadOnlyAttribute()
        {
            #region -- CanEditAccountSettingsUseEmailAsLogin --

            if (User.IsInRole("SysAd") || User.IsInRole("CanEditAccountSettingsUseEmailAsLogin"))
            {
                ViewBag.HtmlAttributeEmailAsLogin = new { @onClick = "useEmailAsLogin($(this))" };
            }
            else{
                ViewBag.HtmlAttributeEmailAsLogin = new { @disabled = "disabled" };
            }

            #endregion

            #region -- CanEditAccountSettingsUseRandomGeneratedPassword --

            if (User.IsInRole("SysAd") || User.IsInRole("CanEditAccountSettingsUseRandomGeneratedPassword"))
            {
                ViewBag.HtmlAttributeRandomGeneratedPassword = new { @onChange = "useRandomGeneratedPassword($(this))" };
            }
            else
            {
                ViewBag.HtmlAttributeRandomGeneratedPassword = new { @disabled = "disabled" };
            }

            #endregion

            #region -- CanEditAccountSettingsBypassCorporateAdminApproval --

            if (User.IsInRole("SysAd") || User.IsInRole("CanEditAccountSettingsBypassCorporateAdminApproval"))
            {
                ViewBag.HtmlAttributeByPassCorAdminApproval = new { @onChange = "bypassHRManagerApproval($(this))" };
            }
            else
            {
                ViewBag.HtmlAttributeByPassCorAdminApproval = new { @disabled = "disabled" };
            }

            #endregion

            #region -- CanEditAccountSettingsBypassMedicalHistory --
            
            if (User.IsInRole("SysAd") || User.IsInRole("CanEditAccountSettingsBypassMedicalHistory"))
            {
                ViewBag.HtmlAttributeByPassMedicalHistory = new { @onChange = "bypassMedicalHistory($(this))" };
            }
            else
            {
                ViewBag.HtmlAttributeByPassMedicalHistory = new { @disabled = "disabled" };
            }

            #endregion

            #region -- CanEditAccountSettingsUrgSetting --

            if (User.IsInRole("SysAd") || User.IsInRole("CanEditAccountSettingsUrgSetting"))
            {
                ViewBag.HtmlAttributeUrgSetting = new { @onChange = "urgSetting($(this))", @class = "form-control" };
            }
            else
            {
                ViewBag.HtmlAttributeUrgSetting = new { @disabled = "disabled", @class = "form-control" };
            }

            #endregion

            #region -- CanEditAccountSettingsDomainEmail --

            if (User.IsInRole("SysAd") || User.IsInRole("CanEditAccountSettingsDomainEmail"))
            {
                ViewBag.HtmlAttributeDomainEmail = new { @class = "form-control" };
            }
            else
            {
                ViewBag.HtmlAttributeDomainEmail = new { @readonly = "readonly", @class = "form-control white-readonly" };
            }

            #endregion

            return null;
        }

        #endregion

        #region -- Overrides --

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                legacyDb.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}