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

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    [Authorize(Roles = "SysAd, CanViewRequestManagement")]
    public class IdReplacementsController : BaseAccountController
    {        
        #region  -- Action Results --

        public ActionResult Index(string accountCode, int? page, string messageType, string message, DateTime? requestDate, string memberName, int? requestForId,string requestReason)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;

            var model = db.IdReplacements
                .Include(t => t.Reason)
                .Include(t=>t.EndorsementBatch)
                .Where(t => t.AccountCode == this.LegacyAccount.Code && !t.Deleted &&
                    t.Status != RequestStatus.Saved )
                .OrderByDescending(t => t.RequestDate).ToList();

            var requestDateString = "";
            var requestForString = "";
            if (requestDate != null)
            {
                model = model.Where(t => t.RequestDate.Date == requestDate.Value.Date).ToList();
                requestDateString = requestDate.Value.ToString("MM/dd/yyyy");
            }
            if (requestReason != null)
            {
                model = model.Where(t => t.Reason.Description.Contains(requestReason)).ToList();
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
            if (requestForId != null)
            {
                var requestFor = (Corelib.Enums.RequestFor)requestForId;
                requestForString = ((Corelib.Enums.RequestFor)requestForId).ToString();
                model = model.Where(t => t.RequestFor == requestFor).ToList();
            }

            ViewBag.RequestDate = requestDateString;
            ViewBag.MemberName = memberName;
            ViewBag.RequestForId = requestForId;
            ViewBag.RequestReason = requestReason;

            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted).ToList();
            ViewBag.Members = db.Members.Where(t => !t.Deleted).ToList();

            ViewBag.EnumSelectList = new SelectList(Enum.GetValues(typeof(RequestFor)).Cast<RequestFor>().Select(t => new SelectListItem
            {
                Text = t.ToString(),
                Value = ((int)t).ToString()
            }).ToList(), "Value", "Text", requestForId.ToString());
            ViewBag.SearchValue = String.Format("{0} {1} {2} {3}", requestDateString, memberName,requestReason,requestForString).Trim();

            return View(model.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }
                
        #endregion

        #region -- Id Replacement --

        public ActionResult IdReplacement(string accountCode, Guid? guid, Guid? memberGuid)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            int memberId = new int();
            if (memberGuid != null && db.Members.Any(t => t.Guid == memberGuid)) memberId = db.Members.FirstOrDefault(t => !t.Deleted && t.Guid == memberGuid).Id;

            IdReplacement model;
            if (guid != null && db.IdReplacements.Any(t => t.Guid == guid))
            {
                model = db.IdReplacements.FirstOrDefault(t => t.Guid == guid);
            }
            else
            {
                model = new IdReplacement()
                {
                    MemberId = memberId,
                    Status = Corelib.Enums.RequestStatus.Submitted
                };
            }

            base.IdReplacementReadOnlyAttribute(model);

            ViewBag.Reasons = db.Reasons.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted).OrderBy(t => t.LastName);
            ViewBag.Members = db.Members.Where(t => !t.Deleted && t.AccountCode == accountCode).OrderBy(t => t.LastName);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IdReplacement(IdReplacement model, string accountCode, string submit)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            if (model.FileWrapper != null)
            {
                using (var binaryReader = new BinaryReader(model.FileWrapper.InputStream))
                {
                    model.DocumentFile = binaryReader.ReadBytes(model.FileWrapper.ContentLength);
                    model.DocumentFileName = model.FileWrapper.FileName;
                    model.DocumentContentType = model.FileWrapper.ContentType;
                }
            }

            if (ModelState.IsValid)
            {
                if (submit == "Submit Request") model.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
                if (model.Id == 0)
                {
                    var endorsementBatch = new EndorsementBatch()
                    {
                        Deadline = DateTime.Now,
                        ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber(),
                        Date = DateTime.Now,
                        EndorsementType = "ID Replacement",
                        BatchType = "ID Replacement",
                        EndorsementCount = 1,
                        AccountCode = accountCode,
                        Status = Corelib.Enums.EndorsementBatchStatus.New
                    };

                    db.EndorsementBatches.Add(endorsementBatch);

                    model.EndorsementBatch = endorsementBatch;
                    model.AccountCode = accountCode;
                    db.IdReplacements.Add(model);
                }
                else
                {
                    var currentIdReplacement = db.IdReplacements.FirstOrDefault(t => t.Id == model.Id && !t.Deleted);
                    db.Entry(currentIdReplacement).CurrentValues.SetValues(model);
                    db.Entry(currentIdReplacement).State = EntityState.Modified;
                }
                if (submit == "Submit Request") Helper.CorpAdminIdReplacement(System.Web.HttpContext.Current, model);
                db.SaveChanges();
                return RedirectToAction("Index", new { accountCode = accountCode });
            }

            base.IdReplacementReadOnlyAttribute(model);

            ViewBag.Reasons = db.Reasons.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted).OrderBy(t => t.LastName);
            ViewBag.Members = db.Members.Where(t => !t.Deleted && t.AccountCode == accountCode).OrderBy(t => t.LastName);

            return View(model);
        }

        public ActionResult CancelIdReplacement(Guid? guid, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            var idReplacement = db.IdReplacements.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (idReplacement == null) throw new Exception("id Replacement Not Found!.");

            idReplacement.Status = Corelib.Enums.RequestStatus.CancelledRequest;
            db.Entry(idReplacement).State = EntityState.Modified;
            db.SaveChanges();
            Helper.CorpAdminCancelIdReplacement(System.Web.HttpContext.Current, idReplacement);
            return RedirectToAction("Index", new { accountCode = accountCode, messageType = "Success!", message = "Successfully cancelled ID Replacement." });
        }

        public ActionResult ApproveIdReplacement(Guid? guid, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            var idReplacement = db.IdReplacements.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (idReplacement == null) throw new Exception("Amendment Not Found!.");

            idReplacement.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
            db.Entry(idReplacement).State = EntityState.Modified;
            db.SaveChanges();
            Helper.CorpAdminIdReplacement(System.Web.HttpContext.Current, idReplacement);
            return RedirectToAction("Index", new { accountCode = accountCode, messageType = "Success!", message = "Successfully approved ID Replacement." });
        }

        public ActionResult DisapproveIdReplacement(Guid? guid, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            var idReplacement = db.IdReplacements.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (idReplacement == null) throw new Exception("Amendment Not Found!.");

            idReplacement.Status = Corelib.Enums.RequestStatus.CorporateAdminDisapproved;
            db.Entry(idReplacement).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { accountCode = accountCode, messageType = "Success!", message = "Successfully disapproved ID Replacement." });
        }

        #endregion

        #region -- Functions --

        public string GetDependents(int? memberId, int? amendmentId)
        {
            var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == memberId) ?? new Corelib.Models.Member();
            var dependents = db.Dependents.Where(t => !t.Deleted && t.MemberId == member.Id).ToList();
            var amendment = db.Amendments.FirstOrDefault(t => t.Id == amendmentId && !t.Deleted) ?? new Amendment();
            var sb = new System.Text.StringBuilder();
            if (memberId != null)
            {
                sb.Append(string.Format(@"<option value=''>-- Select Dependent --</option>"));
                foreach (var dependent in dependents)
                {
                    if (dependent.Id == amendment.DependentId)
                    {
                        sb.Append(string.Format(@"<option value='{0}' selected='selected'>{1}</option>", dependent.Id, dependent.FullName));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<option value='{0}'>{1}</option>", dependent.Id, dependent.FullName));
                    }
                }
            }
            else
            {
                sb.Append(string.Format(@"<option value=''>-- Select Member --</option>"));
            }

            return sb.ToString();
        }

        public FileResult DownloadFile(Guid? guid)
        {
            if (guid != null && db.IdReplacements.Any(t => t.Guid == guid))
            {
                var model = db.IdReplacements.FirstOrDefault(t => t.Guid == guid);
                if (model.DocumentFile != null || !string.IsNullOrEmpty(model.DocumentContentType) || !string.IsNullOrEmpty(model.DocumentFileName))
                { return File(model.DocumentFile, model.DocumentContentType, model.DocumentFileName); }
            }
            return null;
        }

        #endregion
    }
}