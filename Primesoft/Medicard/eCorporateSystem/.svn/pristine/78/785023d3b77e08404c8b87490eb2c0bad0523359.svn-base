using System;
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
    [Authorize(Roles = "SysAd, CanViewApplicationManagement")]
    public class DependentCancellationsController : BaseAccountController
    {
        #region -- Action Results --

        public ActionResult Index(string accountCode, int? page, string messageType, string message, string dependentName, string memberName, DateTime? requestDate, string requestReason)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;

            var model = db.DependentCancellations
                .Include(t => t.EndorsementBatch)
                .Include(t=> t.Reason)
                .Include(t=>t.DocumentType)
                .Where(t => t.AccountCode == this.LegacyAccount.Code && !t.Deleted &&
                    t.Status != RequestStatus.Saved)
                .OrderByDescending(t => t.RequestDate).ToList();

            if (!string.IsNullOrEmpty(dependentName))
            {
                var dependentIds = db.Dependents.Where(t => t.FirstName.Contains(dependentName) || t.MiddleName.Contains(dependentName) || t.LastName.Contains(dependentName)).Select(t=>t.Id).ToList();
                model = model.Where(t => dependentIds.Contains(t.DependentId)).ToList();
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
            var requestDateString = "";
            if (requestDate != null)
            {
                model = model.Where(t => t.RequestDate.Date == requestDate.Value.Date).ToList();
                requestDateString = requestDate.Value.ToString("MM/dd/yyyy");
            }
            ViewBag.SearchValue = String.Format("{0} {1} {2} {3}",requestDateString, memberName, dependentName, requestReason).Trim();

            ViewBag.DependentName = dependentName;
            ViewBag.MemberName = memberName;
            ViewBag.RequestDate = requestDateString;
            ViewBag.RequestReason = requestReason;

            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted).ToList();
            ViewBag.Members = db.Members.Where(t => !t.Deleted).ToList();

            return View(model.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }

        public ActionResult Create(string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            var model = new DependentCancellation()
            {
                AccountCode = accountCode,
                Status = RequestStatus.Submitted
            };

            base.DependentCancellationReadOnlyAttribute(model);
            ViewBag.Reasons = db.Reasons.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Dependents = new List<Dependent>();
            ViewBag.DocumentTypes = db.DocumentTypes.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Members = db.Members.Where(t => !t.Deleted && t.AccountCode == accountCode).OrderBy(t => t.LastName);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DependentCancellation model, string submit)
        {
            var returnValue = base.ValidateAccountCode(model.AccountCode);
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
                var endorsementBatch = new EndorsementBatch()
                {
                    Deadline = DateTime.Now,
                    ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber(),
                    Date = DateTime.Now,
                    EndorsementType = "Dependent Cancellation",
                    BatchType = "Dependent Cancellation",
                    EndorsementCount = 1,
                    AccountCode = model.AccountCode,
                    Status = Corelib.Enums.EndorsementBatchStatus.New
                };
                db.EndorsementBatches.Add(endorsementBatch);

                model.EndorsementBatch = endorsementBatch;
                if (submit == "Submit Request")
                {
                    model.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
                    Helper.CorpAdminDependentCancellation(System.Web.HttpContext.Current, model);
                }
                db.DependentCancellations.Add(model);
                db.SaveChanges();

                return RedirectToAction("Index", new { accountCode = model.AccountCode });
            }

            base.DependentCancellationReadOnlyAttribute(model);
            ViewBag.Reasons = db.Reasons.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted && t.MemberId == model.MemberId).OrderBy(t => t.LastName);
            ViewBag.DocumentTypes = db.DocumentTypes.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Members = db.Members.Where(t => !t.Deleted && t.AccountCode == model.AccountCode).OrderBy(t => t.LastName);

            return View(model);
        }

        public ActionResult Edit(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.DependentCancellations.FirstOrDefault(t => t.Guid == guid && !t.Deleted);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            base.DependentCancellationReadOnlyAttribute(model);
            ViewBag.Reasons = db.Reasons.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted && t.MemberId == model.MemberId).OrderBy(t => t.LastName);
            ViewBag.DocumentTypes = db.DocumentTypes.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Members = db.Members.Where(t => !t.Deleted && t.AccountCode == model.AccountCode).OrderBy(t => t.LastName);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DependentCancellation model, string submit)
        {
            var returnValue = base.ValidateAccountCode(model.AccountCode);
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
                var denpendentCancellation = db.DependentCancellations.FirstOrDefault(t => t.Id == model.Id && !t.Deleted);
                db.Entry(denpendentCancellation).CurrentValues.SetValues(model);
                if (submit == "Submit Request")
                {
                    denpendentCancellation.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
                    Helper.CorpAdminDependentCancellation(System.Web.HttpContext.Current, model);
                }
                db.Entry(denpendentCancellation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { accountCode = model.AccountCode });
            }

            base.DependentCancellationReadOnlyAttribute(model);
            ViewBag.Reasons = db.Reasons.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted && t.MemberId == model.MemberId).OrderBy(t => t.LastName);
            ViewBag.DocumentTypes = db.DocumentTypes.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Members = db.Members.Where(t => !t.Deleted && t.AccountCode == model.AccountCode).OrderBy(t => t.LastName);


            return View(model);
        }

        public ActionResult CancelDependentCancellation(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.DependentCancellations.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            model.Status = Corelib.Enums.RequestStatus.CancelledRequest;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            Helper.CorpAdminCancelDependentCancellation(System.Web.HttpContext.Current, model);
            return RedirectToAction("Index", new { accountCode = model.AccountCode, messageType = "Success!", message = "Successfully cancelled application for Additional Dependent." });
        }

        public ActionResult ApproveDependentCancellation(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.DependentCancellations.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            model.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            Helper.CorpAdminDependentCancellation(System.Web.HttpContext.Current, model);
            return RedirectToAction("Index", new { accountCode = model.AccountCode, messageType = "Success!", message = "Successfully approved amendment." });
        }

        public ActionResult DisapproveDependentCancellation(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = db.DependentCancellations.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var returnValue = base.ValidateAccountCode(model.AccountCode);
            if (returnValue != null) return returnValue;

            model.Status = Corelib.Enums.RequestStatus.CorporateAdminDisapproved;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { accountCode = model.AccountCode, messageType = "Success!", message = "Successfully disapproved amendment." });
        }

        #endregion

        #region -- Functions --

        public string GetDependents(int? memberId, int? modelId)
        {
            var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == memberId) ?? new Corelib.Models.Member();
            var dependents = db.Dependents.Where(t => !t.Deleted && t.MemberId == member.Id).ToList();
            var dependentCancellation = db.DependentCancellations.FirstOrDefault(t => t.Id == modelId && !t.Deleted) ?? new DependentCancellation();
            var sb = new System.Text.StringBuilder();
            if (memberId != null)
            {
                sb.Append(string.Format(@"<option value=''>-- Select Dependent --</option>"));
                foreach (var dependent in dependents)
                {
                    if (dependent.Id == dependentCancellation.DependentId)
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
            if (guid != null && db.DependentCancellations.Any(t => t.Guid == guid))
            {
                var model = db.DependentCancellations.FirstOrDefault(t => t.Guid == guid);
                if (model.DocumentFile != null || !string.IsNullOrEmpty(model.DocumentContentType) || !string.IsNullOrEmpty(model.DocumentFileName))
                { return File(model.DocumentFile, model.DocumentContentType, model.DocumentFileName); }
            }
            return null;
        }
        
        #endregion
    }
}