﻿using Corelib.Enums;
using Corelib.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace WebUI.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class ProfileController : BaseMemberController
    {
        #region -- Action Results --

        [Authorize(Roles = "Member")]
        public ActionResult Index(string messageType, string message)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;

            var principalPlans = Helper.GetLegacyRoomRates(this.Member.AccountCode, true);
            var appliedPlans = principalPlans.Where(t => t.Id == this.Member.AppliedPlan);
            var allowedPlans = new List<int>();
            if (!String.IsNullOrEmpty(this.Member.AllowedPlans))
            {
                foreach (var allowedPlan in this.Member.AllowedPlans.Split(','))
                {
                    allowedPlans.Add(int.Parse(allowedPlan));
                }
            }

            var optionalPlans = principalPlans.Where(t => t.Id != this.Member.AppliedPlan && allowedPlans.Contains(t.Id));

            ViewBag.AppliedPlanList = new SelectList(appliedPlans, "Id", "LongDescription", this.Member.AppliedPlan);
            ViewBag.OptionalPlanList = new SelectList(optionalPlans, "Id", "LongDescription");
            ViewBag.GenderList = new SelectList(new List<string>() { "Male", "Female" }, this.Member.Gender);
            ViewBag.CivilStatusList = new SelectList(new List<string>() { "Single", "Married", "Divorced", "Widowed" }, this.Member.CivilStatus);
            ViewBag.OptionalPlanCount = optionalPlans.Count(t => t.Id != this.Member.AppliedPlan);

            return View(this.Member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult> Index([Bind(Include = "Id, Guid, Timestamp, UserId, DependentAppliedPlan, UserName, EndorsementBatchId, AccountCode, AllowedPlans, AllowedDependentPlans, EmployeeNumber, LastName, FirstName, MiddleName, Suffix, DateOfBirth, Area, CostCenter, EffectivityDate, ValidityDate, DateHired, Gender, CivilStatus, EmailAddress, OtherEmailAddress, Dependent, AppliedPlan, OptionalPlan, Street, City, AreaCode, Province, Telephone, Mobile, HeightFeet, HeightInches, Weight, Position")] Corelib.Models.Member member, string submit)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;

            if (ModelState.IsValid)
            {
                var currentMember = db.Members.Include(t => t.EndorsementBatch).FirstOrDefault(t => t.Id == member.Id);
                var currentEndorsementBatch = currentMember.EndorsementBatch;
                if (currentMember == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                db.Entry(currentMember).CurrentValues.SetValues(member);
                currentMember.EndorsementBatch = currentEndorsementBatch;
                currentMember.Status = MembershipStatus.Saved;
                db.Entry(currentMember).State = EntityState.Modified;
                await db.SaveChangesAsync();

                var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == currentMember.AccountCode) ?? new AccountSetting();

                if (submit == "Save")
                {
                    return RedirectToAction("Index", new { @messageType = "Success", @message = "Successfully saved information." });
                }
                else if (submit == "Continue")
                {
                    if (!accountSetting.BypassMedicalHistory)
                    {
                        return RedirectToAction("Index", "MedicalHistory");
                    }
                    else if (currentMember.Dependent > 0)
                    {
                        return RedirectToAction("Index", "Dependents");
                    }
                    else
                    {
                        return RedirectToAction("Index", "SubmitForApproval");
                    }
                }
            }

            var principalPlans = Helper.GetLegacyRoomRates(this.Member.AccountCode, true);
            var appliedPlans = principalPlans.Where(t => t.Id == this.Member.AppliedPlan);
            var allowedPlans = new List<int>();
            if (!String.IsNullOrEmpty(this.Member.AllowedPlans))
            {
                foreach (var allowedPlan in this.Member.AllowedPlans.Split(','))
                {
                    allowedPlans.Add(int.Parse(allowedPlan));
                }
            }

            var optionalPlans = principalPlans.Where(t => t.Id != this.Member.AppliedPlan && allowedPlans.Contains(t.Id));

            ViewBag.AppliedPlanList = new SelectList(appliedPlans, "Id", "LongDescription", this.Member.AppliedPlan);
            ViewBag.OptionalPlanList = new SelectList(optionalPlans, "Id", "LongDescription", this.Member.OptionalPlan);
            ViewBag.GenderList = new SelectList(new List<string>() { "Male", "Female" }, this.Member.Gender);
            ViewBag.CivilStatusList = new SelectList(new List<string>() { "Single", "Married", "Divorced", "Widowed" }, this.Member.CivilStatus);
            ViewBag.OptionalPlanCount = principalPlans.Count(t => t.Id != this.Member.AppliedPlan);

            return View(member);
        }

        #endregion
    }
}