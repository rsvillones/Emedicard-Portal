﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Corelib;
using Corelib.Models;
using OfficeOpenXml;
using System.IO;
using System.Data.Entity;
using Corelib.Classes;
using System.Threading.Tasks;
using System.Net;
using Corelib.Enums;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using WebUI.Areas.Member.Models;

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    [Authorize(Roles = "SysAd, CanViewApplicationManagement")]
    public class AdditionalDependentsController : BaseAccountController
    {
        #region  -- Action Results --

        public ActionResult Index(string accountCode, int? page, string messageType, string message,string dependentName, string memberName,DateTime? applicationDate)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;
            
            var model = db.AdditionalDependents
                .Include(t => t.EndorsementBatch)
                .Include(t=>t.Relationship)
                .Where(t => t.AccountCode == this.LegacyAccount.Code && !t.Deleted &&
                    t.Status != RequestStatus.Saved)
                .OrderByDescending(t => t.ApplicationDate).ToList();

            if (!string.IsNullOrEmpty(dependentName))
            {
                model = model.Where(t => t.FirstName.Contains(dependentName) || t.MiddleName.Contains(dependentName) || t.LastName.Contains(dependentName)).ToList();
            }
            if (!string.IsNullOrEmpty(memberName))
            {
                var memberIds = db.Members
                       .Where(t => !t.Deleted &&
                           (t.FirstName.Contains(memberName) ||
                           t.MiddleName.Contains(memberName) ||
                           t.LastName.Contains(memberName)))
                       .Select(t => t.Id).ToList();
                model = model.Where(t => memberIds.Contains(t.MemberId)).ToList();
            } 
            var applicationDateString = "";
            if (applicationDate != null)
            {
                model = model.Where(t => t.ApplicationDate.Date == applicationDate.Value.Date).ToList();
                applicationDateString = applicationDate.Value.ToString("MM/dd/yyyy");
            }
            ViewBag.SearchValue = String.Format("{0} {1} {2}", memberName, dependentName, applicationDateString).Trim();

            ViewBag.DependentName = dependentName;
            ViewBag.MemberName = memberName;
            ViewBag.ApplicationDate = applicationDateString;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;
            ViewBag.Members = db.Members.Where(t => !t.Deleted).ToList();

            return View(model.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }

        public ActionResult Create(string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            var model = new AdditionalDependent()
            {
                AccountCode = accountCode,
                Status = RequestStatus.Submitted
            };

            ProcessOtherInfo(model);
            base.AdditionalDependentReadOnlyAttribute(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdditionalDependent model, string submit)
        {
            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            if (ModelState.IsValid)
            {
                var endorsementBatch = new EndorsementBatch()
                {
                    Deadline = DateTime.Now,
                    ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber(),
                    Date = DateTime.Now,
                    EndorsementType = "Additional Dependent",
                    BatchType = "Additional Dependent",
                    EndorsementCount = 1,
                    AccountCode = model.AccountCode,
                    Status = Corelib.Enums.EndorsementBatchStatus.New
                };
                db.EndorsementBatches.Add(endorsementBatch);

                model.EndorsementBatch = endorsementBatch;
                if (submit == "Submit Request")
                {
                    model.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
                    Helper.CorpAdminAdditionalDependent(System.Web.HttpContext.Current, model);
                }                
                db.AdditionalDependents.Add(model);                
                db.SaveChanges();

                return RedirectToAction("Index", new { accountCode = model.AccountCode });
            }

            ProcessOtherInfo(model);
            base.AdditionalDependentReadOnlyAttribute(model);

            return View(model);
        }

        public ActionResult Edit(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.AdditionalDependents.FirstOrDefault(t => t.Guid == guid && !t.Deleted);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            ProcessOtherInfo(model);

            base.AdditionalDependentReadOnlyAttribute(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdditionalDependent model, string submit)
        {
            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            if (ModelState.IsValid)
            {
                var additionDependent = db.AdditionalDependents.FirstOrDefault(t => t.Id == model.Id && !t.Deleted);
                db.Entry(additionDependent).CurrentValues.SetValues(model);
                if (submit == "Submit Request")
                {
                    additionDependent.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
                    Helper.CorpAdminAdditionalDependent(System.Web.HttpContext.Current, additionDependent);
                }  
                db.Entry(additionDependent).State = EntityState.Modified;
                db.SaveChanges();                
                return RedirectToAction("Index", new { accountCode = model.AccountCode });
            }

            ProcessOtherInfo(model);
            base.AdditionalDependentReadOnlyAttribute(model);

            return View(model);
        }

        public ActionResult CancelApplication(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.AdditionalDependents.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            model.Status = Corelib.Enums.RequestStatus.CancelledRequest;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            Helper.CorpAdminCancelAdditionalDependent(System.Web.HttpContext.Current, model);
            return RedirectToAction("Index", new { accountCode = model.AccountCode, messageType = "Success!", message = "Successfully cancelled application for Additional Dependent." });
        }

        public ActionResult ApproveApplication(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.AdditionalDependents.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;
            
            model.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            Helper.CorpAdminAdditionalDependent(System.Web.HttpContext.Current, model);
            return RedirectToAction("Index", new { accountCode = model.AccountCode, messageType = "Success!", message = "Successfully approved amendment." });
        }

        public ActionResult DisapproveApplication(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.AdditionalDependents.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;
            
            model.Status = Corelib.Enums.RequestStatus.CorporateAdminDisapproved;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { accountCode = model.AccountCode, messageType = "Success!", message = "Successfully disapproved amendment." });
        }

        #endregion

        #region -- Medical History --

        public ActionResult MedicalHistory(Guid? guid, string messageType, string message)
        {
            var model = db.AdditionalDependents.FirstOrDefault(t => t.Guid == guid);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;
            var member = db.Members.FirstOrDefault(t => t.Id == model.MemberId && !t.Deleted);
            Helper.IsAdditionalDependentMedicalHistoryComplete(db, member);

            var viewModel = new AdditionalDependentMedicalHistoryViewModel()
            {
                AdditionalDependentMedicalHistories = db.AdditionalDependentMedicalHistories.Include(t => t.Question).Where(t => t.AdditionalDependentId == model.Id).OrderBy(t => t.Question.DisplayOrder).ThenBy(t => t.Question.Description).ToList()
            };

            base.AdditionalDependentReadOnlyAttribute(model);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MedicalHistory(string accountCode,AdditionalDependentMedicalHistoryViewModel model)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            if (ModelState.IsValid)
            {
                if (model.AdditionalDependentMedicalHistories != null)
                {
                    foreach (var additionalDependentMedicalHistory in model.AdditionalDependentMedicalHistories)
                    {
                        db.Entry(additionalDependentMedicalHistory).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { accountCode = accountCode });
            }

            foreach (var additionalDependentMedicalHistory in model.AdditionalDependentMedicalHistories)
            {
                additionalDependentMedicalHistory.Question = db.Questions.Find(additionalDependentMedicalHistory.QuestionId);
            }

            return View(model);
        }

        #endregion

        #region -- Functions --

        public string GetDependentAppliedPlan(int? memberId)
        {
            var member = db.Members.FirstOrDefault(t => t.Id == memberId);
            var sb = new System.Text.StringBuilder();
            if (member != null)
            {
                var dependentPlans = Helper.GetLegacyRoomRates(member.AccountCode, false);
                var appliedPlans = dependentPlans.Where(t => t.Id == member.DependentAppliedPlan).ToList();
                if (appliedPlans.Count == 0) appliedPlans = dependentPlans.ToList();
                sb.Append(string.Format(@"<option value=''>-- Select Applied Plan --</option>"));
                foreach (var appliedPlan in appliedPlans)
                {
                    sb.Append(string.Format(@"<option value='{0}'>{1}</option>", appliedPlan.Id, appliedPlan.LongDescription));
                }
                return sb.ToString();
            }
            return sb.Append(string.Format(@"<option value=''>-- Select Member --</option>")).ToString();
        }

        public string GetDependentOptionalPlan(int? memberId)
        {
            var member = db.Members.FirstOrDefault(t => t.Id == memberId);
            var sb = new System.Text.StringBuilder();
            if (member != null)
            {
                var dependentPlans = Helper.GetLegacyRoomRates(member.AccountCode, false);

                var allowedPlans = new List<int>();
                if (!String.IsNullOrEmpty(member.AllowedDependentPlans))
                {
                    foreach (var allowedPlan in member.AllowedDependentPlans.Split(','))
                    {
                        allowedPlans.Add(int.Parse(allowedPlan));
                    }
                }

                var optionalPlans = dependentPlans.Where(t => t.Id != member.DependentAppliedPlan && allowedPlans.Contains(t.Id)).ToList();

                sb.Append(string.Format(@"<option value=''>-- Select Optional Plan --</option>"));
                foreach (var optionalPlan in optionalPlans)
                {
                    sb.Append(string.Format(@"<option value='{0}'>{1}</option>", optionalPlan.Id, optionalPlan.LongDescription));
                }
                return sb.ToString();
            }
            return sb.Append(string.Format(@"<option value=''>-- Select Member --</option>")).ToString();
        }

        public int GetDependentOptionalPlanCount(int? memberId)
        {
            var member = db.Members.FirstOrDefault(t => t.Id == memberId);
            if (member != null)
            {
                var dependentPlans = Helper.GetLegacyRoomRates(member.AccountCode, false);

                var allowedPlans = new List<int>();
                if (!String.IsNullOrEmpty(member.AllowedDependentPlans))
                {
                    foreach (var allowedPlan in member.AllowedDependentPlans.Split(','))
                    {
                        allowedPlans.Add(int.Parse(allowedPlan));
                    }
                }
                var optionalPlans = dependentPlans.Where(t => t.Id != member.DependentAppliedPlan && allowedPlans.Contains(t.Id)).ToList();

                return optionalPlans.Count(t => t.Id != member.AppliedPlan);
            }
            return 0;
        }

        #endregion

        #region -- Process Other Info --

        private void ProcessOtherInfo(AdditionalDependent model)
        {
            var member = db.Members.FirstOrDefault(t => t.Id == model.MemberId) ?? new Corelib.Models.Member();
            var dependentPlans = Helper.GetLegacyRoomRates(model.AccountCode, false);
            var appliedPlans = dependentPlans.Where(t => t.Id == member.DependentAppliedPlan).ToList();
            if (appliedPlans.Count == 0) appliedPlans = dependentPlans.ToList();

            var allowedPlans = new List<int>();
            if (!String.IsNullOrEmpty(member.AllowedDependentPlans))
            {
                foreach (var allowedPlan in member.AllowedDependentPlans.Split(','))
                {
                    allowedPlans.Add(int.Parse(allowedPlan));
                }
            }

            var optionalPlans = dependentPlans.Where(t => t.Id != member.DependentAppliedPlan && allowedPlans.Contains(t.Id)).ToList();

            ViewBag.AppliedPlanList = new SelectList(appliedPlans, "Id", "LongDescription", model.AppliedPlan);
            ViewBag.OptionalPlanList = new SelectList(optionalPlans, "Id", "LongDescription", model.OptionalPlan);
            ViewBag.GenderList = new SelectList(new List<string>() { "Male", "Female" }, model.Gender);
            ViewBag.CivilStatusList = new SelectList(new List<string>() { "Single", "Married", "Divorced", "Widowed" }, model.CivilStatus);
            ViewBag.RelationshipList = new SelectList(db.Relationships, "Code", "Description", model.RelationshipCode);
            ViewBag.MemberList = new SelectList(db.Members.Where(t => t.AccountCode == model.AccountCode), "Id", "FullName", model.MemberId);
            ViewBag.OptionalPlanCount = optionalPlans.Count(t => t.Id != member.AppliedPlan);
        }

        #endregion
    }
}