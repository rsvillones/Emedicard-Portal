﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Corelib.Models;
using WebUI.Areas.Member.Models;
using Corelib.Enums;

namespace WebUI.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class MedicalHistoryController : BaseMemberController
    {
        #region -- Action Results --

        public ActionResult Index(string messageType, string message)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;

            Helper.IsMedicalHistoryComplete(db, this.Member);

            var model = new MedicalHistoryViewModel()
            {
                MedicalHistories = db.MedicalHistories.Include(t => t.Question).Where(t => t.MemberId == this.Member.Id).OrderBy(t => t.Question.DisplayOrder).ThenBy(t => t.Question.Description).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult> Index(MedicalHistoryViewModel model, string submit)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;

            if (ModelState.IsValid)
            {
                if (model.MedicalHistories != null)
                {
                    foreach (var medicalHistory in model.MedicalHistories)
                    {
                        db.Entry(medicalHistory).State = EntityState.Modified;
                    }
                }

                var currentMember = db.Members.FirstOrDefault(t => t.Id == this.Member.Id);
                currentMember.Status = MembershipStatus.Saved;

                await db.SaveChangesAsync();

                var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == this.Member.AccountCode) ?? new AccountSetting();

                if (submit == "Save")
                {
                    return RedirectToAction("Index", new { @messageType = "Success", @message = "Successfully saved information." });
                }
                else if (submit == "Continue")
                {
                    if (this.Member.Dependent > 0)
                    {
                        return RedirectToAction("Index", "Dependents");
                    }
                    else
                    {
                        return RedirectToAction("Index", "SubmitForApproval");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "SubmitForApproval");
                }
            }

            foreach (var medicalHistory in model.MedicalHistories)
            {
                medicalHistory.Question = db.Questions.Find(medicalHistory.QuestionId);
            }

            return View(model);
       }

        #endregion
    }
}