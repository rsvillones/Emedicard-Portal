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

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    [Authorize(Roles = "SysAd, CanViewMembershipEndorsements")]
    public class EndorsementController : BaseAccountController
    {
        #region -- Variable Declarations --

        private const int NEW_EMAIL_ADDRESS = 1;
        private const int NEW_EMPLOYEE_NUMBER = NEW_EMAIL_ADDRESS + 1;
        private const int NEW_AREA = NEW_EMPLOYEE_NUMBER + 1;
        private const int NEW_COST_CENTER = NEW_AREA + 1;
        private const int NEW_LAST_NAME = NEW_COST_CENTER + 1;
        private const int NEW_FIRST_NAME = NEW_LAST_NAME + 1;
        private const int NEW_MIDDLE_NAME = NEW_FIRST_NAME + 1;
        private const int NEW_SUFFIX = NEW_MIDDLE_NAME + 1;
        private const int NEW_GENDER = NEW_SUFFIX + 1;
        private const int NEW_DATE_OF_BIRTH = NEW_GENDER + 1;
        private const int NEW_AGE = NEW_DATE_OF_BIRTH + 1;
        private const int NEW_CIVIL_STATUS = NEW_AGE + 1;
        private const int NEW_EFFECTIVITY_DATE = NEW_CIVIL_STATUS + 1;
        private const int NEW_VALIDITY_DATE = NEW_EFFECTIVITY_DATE + 1;
        private const int NEW_DATE_HIRED = NEW_VALIDITY_DATE + 1;
        private const int NEW_APPLIED_PLAN = NEW_DATE_HIRED + 1;
        private const int NEW_OPTIONAL_PLAN = NEW_APPLIED_PLAN + 1;
        private const int NEW_NUMBER_OF_ALLOWED_DEPENDENTS = NEW_OPTIONAL_PLAN + 1;
        private const int NEW_DEPENDENT_APPLIED_PLAN = NEW_NUMBER_OF_ALLOWED_DEPENDENTS + 1;
        private const int NEW_DEPENDENT_OPTIONAL_PLAN = NEW_DEPENDENT_APPLIED_PLAN + 1;

        private const int RENEWAL_MEMBER_CODE = 1;
        private const int RENEWAL_LAST_NAME = RENEWAL_MEMBER_CODE + 1;
        private const int RENEWAL_FIRST_NAME = RENEWAL_LAST_NAME + 1;
        private const int RENEWAL_MIDDLE_NAME = RENEWAL_FIRST_NAME + 1;
        private const int RENEWAL_EMAIL = RENEWAL_MIDDLE_NAME + 1;
        private const int RENEWAL_DATE_OF_BIRTH = RENEWAL_EMAIL + 1;
        private const int RENEWAL_AGE = RENEWAL_DATE_OF_BIRTH + 1;
        private const int RENEWAL_AREA = RENEWAL_AGE + 1;
        private const int RENEWAL_EMPLOYEE_NUMBER = RENEWAL_AREA + 1;
        private const int RENEWAL_APPLIED_PLAN = RENEWAL_EMPLOYEE_NUMBER + 1;
        private const int RENEWAL_ALLOWED_PLANS = RENEWAL_APPLIED_PLAN + 1;
        private const int RENEWAL_TYPE = RENEWAL_ALLOWED_PLANS + 1;
        private const int RENEWAL_PRINCIPAL_MEMBER_CODE = RENEWAL_TYPE + 1;
        private const int RENEWAL_GENDER = RENEWAL_PRINCIPAL_MEMBER_CODE + 1;
        private const int RENEWAL_CIVIL_STATUS = RENEWAL_GENDER + 1;
        private const int RENEWAL_WAIVER = RENEWAL_CIVIL_STATUS + 1;
        private const int RENEWAL_EFFECTIVITY_DATE = RENEWAL_WAIVER + 1;
        private const int RENEWAL_VALIDITY_DATE = RENEWAL_EFFECTIVITY_DATE + 1;
        private const int RENEWAL_REMARKS = RENEWAL_VALIDITY_DATE + 1;

        private const int CANCELLATION_MEMBER_CODE = 1;
        private const int CANCELLATION_LAST_NAME = CANCELLATION_MEMBER_CODE + 1;
        private const int CANCELLATION_FIRST_NAME = CANCELLATION_LAST_NAME + 1;
        private const int CANCELLATION_MIDDLE_NAME = CANCELLATION_FIRST_NAME + 1;
        private const int CANCELLATION_EMAIL = CANCELLATION_MIDDLE_NAME + 1;
        private const int CANCELLATION_DATE_OF_BIRTH = CANCELLATION_EMAIL + 1;
        private const int CANCELLATION_AGE = CANCELLATION_DATE_OF_BIRTH + 1;
        private const int CANCELLATION_AREA = CANCELLATION_AGE + 1;
        private const int CANCELLATION_EMPLOYEE_NUMBER = CANCELLATION_AREA + 1;
        private const int CANCELLATION_APPLIED_PLAN = CANCELLATION_EMPLOYEE_NUMBER + 1;
        private const int CANCELLATION_TYPE = CANCELLATION_APPLIED_PLAN + 1;
        private const int CANCELLATION_GENDER = CANCELLATION_TYPE + 1;
        private const int CANCELLATION_CIVIL_STATUS = CANCELLATION_GENDER + 1;
        private const int CANCELLATION_WAIVER = CANCELLATION_CIVIL_STATUS + 1;
        private const int CANCELLATION_EFFECTIVITY_DATE = CANCELLATION_WAIVER + 1;
        private const int CANCELLATION_VALIDITY_DATE = CANCELLATION_EFFECTIVITY_DATE + 1;
        private const int CANCELLATION_REMARKS = CANCELLATION_VALIDITY_DATE + 1;
        
        #endregion

        #region -- Action Results --

        public ActionResult Index(string accountCode, int? page, string messageType, string message, string endorsementType, string fileName, DateTime? dateCreated)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;

            var model = db.EndorsementBatches.Where(t => t.AccountCode == this.LegacyAccount.Code && !t.Deleted &&
                (
                    t.EndorsementType == "New" 
                    || t.EndorsementType == "Renewal" 
                    || t.EndorsementType == "Cancel Membership"
                )).OrderByDescending(t => t.Date).ToList();

            if (!string.IsNullOrEmpty(endorsementType)) model = model.Where(t => t.EndorsementType.Contains(endorsementType)).ToList();
            if (!string.IsNullOrEmpty(fileName)) model = model.Where(t => t.Filename.Contains(fileName)).ToList();
            var dateSting = "";
            if (dateCreated != null)
            {
                model = model.Where(t => t.Date.Date == dateCreated.Value.Date).ToList();
                dateSting = dateCreated.Value.ToString("MM/dd/yyyy");
            }

            ViewBag.EndorsementType = endorsementType;
            ViewBag.FileName = fileName;
            ViewBag.DateSting = dateSting;

            ViewBag.EndorsementType = new SelectList(new List<string>() { "New", "Renewal", "Cancel Membership"}, endorsementType);
            ViewBag.SearchValue = String.Format("{0} {1} {2}", endorsementType, fileName, dateSting).Trim();

            return View(model.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }

        public ActionResult BatchUpload(string accountCode, Guid? guid)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            EndorsementBatch model;
            if (guid != null && db.EndorsementBatches.Any(t => t.Guid == guid))
            {
                model = db.EndorsementBatches.Include(t => t.Members).FirstOrDefault(t => t.Guid == guid);
                ViewBag.Genders = new List<string>() { "Male", "Female" };
                ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
                ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
                ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
                ViewBag.Validate = true;
            }
            else
            {
                model = new EndorsementBatch()
                {
                    ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber()
                };
            }

            base.ReadOnlyAttribute(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchUpload(EndorsementBatch endorsementBatch, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            endorsementBatch.Date = DateTime.Now;
            endorsementBatch.EndorsementType = "New";
            endorsementBatch.BatchType = "Batch Upload";
            endorsementBatch.AccountCode = accountCode;
            endorsementBatch.Status = Corelib.Enums.EndorsementBatchStatus.New;

            if (ModelState.IsValid)
            {
                foreach (var member in endorsementBatch.Members)
                {
                    member.LegacyMapCode = Helper.GenerateLegacyMapCode(db);
                }

                if (endorsementBatch.Id == 0)
                {
                    db.EndorsementBatches.Add(endorsementBatch);
                }
                else
                {
                    var currentMemberIds = endorsementBatch.Members.Select(t => t.Id).Distinct();
                    var membersToDelete = db.Members.Where(t => t.EndorsementBatchId == endorsementBatch.Id && !currentMemberIds.Contains(t.Id));
                    db.Members.RemoveRange(membersToDelete);

                    foreach (var member in endorsementBatch.Members)
                    {
                        if (member.Id == 0) db.Entry(member).State = EntityState.Added;
                    }
                    db.Entry(endorsementBatch).State = EntityState.Modified;
                    if (endorsementBatch.Members != null && endorsementBatch.Members.Count > 0)
                    {
                        for (var index = endorsementBatch.Members.Count - 1; index >= 0; index--)
                        {
                            var member = endorsementBatch.Members.ElementAt(index);
                            if (member.Id != 0) db.Entry(member).State = EntityState.Modified;
                        }
                    }
                }
                endorsementBatch.EndorsementCount = endorsementBatch.Members != null ? endorsementBatch.Members.Count(t => !t.Deleted) : 0;
                db.SaveChanges();
                return RedirectToAction("Index", new { accountCode = accountCode });
            }

            base.ReadOnlyAttribute(endorsementBatch);

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            ViewBag.Validate = true;

            return View(endorsementBatch);
        }

        public ActionResult Renewal(string accountCode, Guid? guid)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            EndorsementBatch model;
            if (guid != null && db.EndorsementBatches.Any(t => t.Guid == guid))
            {
                model = db.EndorsementBatches
                    .Include(t => t.RenewalMembers)
                    .FirstOrDefault(t => t.Guid == guid);
                ViewBag.Genders = new List<string>() { "Male", "Female" };
                ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
                ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
                ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
                ViewBag.Validate = true;
            }
            else
            {
                model = new EndorsementBatch()
                {
                    ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber()
                };
            }

            base.ReadOnlyAttribute(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renewal(EndorsementBatch endorsementBatch, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            endorsementBatch.Date = DateTime.Now;
            endorsementBatch.EndorsementType = "Renewal";
            endorsementBatch.BatchType = "Batch Upload";
            endorsementBatch.AccountCode = accountCode;
            endorsementBatch.Status = Corelib.Enums.EndorsementBatchStatus.New;

            if (ModelState.IsValid)
            {
                if (endorsementBatch.Id == 0)
                {
                    db.EndorsementBatches.Add(endorsementBatch);
                }
                else
                {
                    var currentRenewalMemberIds = endorsementBatch.RenewalMembers.Select(t => t.Id).Distinct();
                    var renewalMembersToDelete = db.RenewalMembers.Where(t => t.EndorsementBatchId == endorsementBatch.Id && !currentRenewalMemberIds.Contains(t.Id));
                    db.RenewalMembers.RemoveRange(renewalMembersToDelete);

                    foreach (var renewalMember in endorsementBatch.RenewalMembers)
                    {
                        if (renewalMember.Id == 0)
                        {
                            db.Entry(renewalMember).State = EntityState.Added;
                        } 
                    }
                    db.Entry(endorsementBatch).State = EntityState.Modified;
                    if (endorsementBatch.RenewalMembers != null && endorsementBatch.RenewalMembers.Count > 0)
                    {
                        for (var index = endorsementBatch.RenewalMembers.Count - 1; index >= 0; index--)
                        {
                            var renewalMember = endorsementBatch.RenewalMembers.ElementAt(index);
                            if (renewalMember.Id != 0) db.Entry(renewalMember
                            ).State = EntityState.Modified;
                        }
                    }
                }
                endorsementBatch.EndorsementCount = endorsementBatch.RenewalMembers != null ? endorsementBatch.RenewalMembers.Count(t => !t.Deleted) : 0;
                db.SaveChanges();
                return RedirectToAction("Index", new { accountCode = accountCode });
            }

            base.ReadOnlyAttribute(endorsementBatch);

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            ViewBag.Validate = true;

            return View(endorsementBatch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateUploadedExcel(EndorsementBatch endorsementBatch, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            if (ModelState.IsValid) { }

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            ViewBag.Validate = true;
            if (endorsementBatch.Members != null)
            {
                foreach (var member in endorsementBatch.Members)
                {
                    member.EndorsementBatchId = endorsementBatch.Id;
                }
            }

            base.ReadOnlyAttribute(endorsementBatch);

            return View("BatchUpload", endorsementBatch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateUploadedRenewalExcel(EndorsementBatch endorsementBatch, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            ViewBag.Validate = true;
            if (endorsementBatch.Members != null)
            {
                foreach (var member in endorsementBatch.Members)
                {
                    member.EndorsementBatchId = endorsementBatch.Id;
                }
            }

            base.ReadOnlyAttribute(endorsementBatch);

            return View("Renewal", endorsementBatch);
        }

        public JsonResult UploadExcel(HttpPostedFileBase fileData, string accountCode, int endorsementBatchId)
        {
            var fileExtension = fileData.FileName.Substring(fileData.FileName.LastIndexOf(".") + 1);
            if (fileExtension != "xlsm") throw new Exception("File Not Valid");

            var originalFilename = fileData.FileName;
            var originalFilepath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", originalFilename));

            var filename = String.Format("{0}.{1}", Guid.NewGuid().ToString(), fileExtension);
            var filepath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", filename));

            fileData.SaveAs(filepath);

            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Filename", originalFilename);
            dictionary.Add("GuidFilename", filename);

            dictionary.Add("TableData", RenderPartialViewToString("~/Areas/CorporateAdministrator/Views/Endorsement/_MemberWrapper.cshtml", ImportNewExcel(accountCode, endorsementBatchId, filepath)));

            return this.Json(dictionary);
        }

        public JsonResult UploadRenewalExcel(HttpPostedFileBase fileData, string accountCode, int endorsementBatchId)
        {
            var fileExtension = fileData.FileName.Substring(fileData.FileName.LastIndexOf(".") + 1);
            if (fileExtension != "xlsm") throw new Exception("File Not Valid");

            var originalFilename = fileData.FileName;
            var originalFilepath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", originalFilename));

            var filename = String.Format("{0}.{1}", Guid.NewGuid().ToString(), fileExtension);
            var filepath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", filename));

            fileData.SaveAs(filepath);

            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Filename", originalFilename);
            dictionary.Add("GuidFilename", filename);

            dictionary.Add("TableData", RenderPartialViewToString("~/Areas/CorporateAdministrator/Views/Endorsement/_RenewalMemberWrapper.cshtml", ImportRenewalExcel(accountCode, endorsementBatchId, filepath)));

            return this.Json(dictionary);
        }

        public FileResult DownloadNewApplicationTemplate(string accountCode)
        {
            return File(ExcelTools.NewApplicationExcelDownload(accountCode), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", String.Format("New Application ({0:MMddyyyyhhmmss}).xlsm", DateTime.Now));
        }

        public FileResult DownloadRenewalTemplate(string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null)
            {
                return null;
            }

            var account = legacyDb.LegacyAccounts
                .Include(t => t.LegacyRoomRates)
                .Include(t => t.LegacyRoomRates.Select(lr => lr.LegacyPaymode))
                .Include(t => t.LegacyRoomRates.Select(rr => rr.LegacyPlan))
                .FirstOrDefault(t => t.Code == accountCode);

            var activeMembers = Helper.GetActiveMembers(db, legacyDb, accountCode);

            byte[] fileBuffer;
            var templatePath = Server.MapPath("~/ExcelTemplates/Renewal.xlsm");
            var targetPath = String.Format(@"{0}\{1}.xlsm", Server.MapPath("~/Uploads"), Guid.NewGuid());
            System.IO.File.Copy(templatePath, targetPath);

            using (var package = new ExcelPackage(new FileInfo(targetPath)))
            {
                var wb = package.Workbook;
                if (wb == null) throw new Exception("Invalid WorkBook.");
                if (wb.Worksheets.Count <= 0) throw new Exception("Worksheet doesn't exist.");
                var ws = wb.Worksheets[1];

                var principalPlanCount = 0;
                var dependentPlanCount = 0;
                var availablePlansWS = wb.Worksheets["AvailablePlansForPrincipal"];
                var row = 1;
                var principalRoomRates = account.LegacyRoomRates.Where(t => t.PaymentFor == 0 || t.PaymentFor == 5);
                foreach (var legacyRoomRate in principalRoomRates)
                {
                    availablePlansWS.Cells[row, 1].Value = String.Format("{1} - {2} ({3:Php 0,0.00} Limit)|{0}", legacyRoomRate.Id, legacyRoomRate.LegacyPlan.Description, legacyRoomRate.For, legacyRoomRate.Limit);
                    row++;
                    principalPlanCount++;
                }

                availablePlansWS = wb.Worksheets["AvailablePlansForDependent"];
                row = 1;
                var dependentRoomRates = account.LegacyRoomRates.Where(t => t.PaymentFor == 1 || t.PaymentFor == 5);
                foreach (var legacyRoomRate in dependentRoomRates)
                {
                    availablePlansWS.Cells[row, 1].Value = String.Format("{1} - {2} ({3:Php 0,0.00} Limit)|{0}", legacyRoomRate.Id, legacyRoomRate.LegacyPlan.Description, legacyRoomRate.For, legacyRoomRate.Limit);
                    row++;
                    dependentPlanCount++;
                }

                var currentRow = 3;
                foreach (var member in activeMembers)
                {
                    ws.Cells[currentRow, RENEWAL_MEMBER_CODE].Value = member.Code;
                    ws.Cells[currentRow, RENEWAL_LAST_NAME].Value = member.LastName;
                    ws.Cells[currentRow, RENEWAL_FIRST_NAME].Value = member.FirstName;
                    ws.Cells[currentRow, RENEWAL_MIDDLE_NAME].Value = member.MiddleName;
                    ws.Cells[currentRow, RENEWAL_EMAIL].Value = member.EmailAddress;
                    ws.Cells[currentRow, RENEWAL_DATE_OF_BIRTH].Value = member.DateOfBirth;
                    ws.Cells[currentRow, RENEWAL_AREA].Value = member.Area;
                    ws.Cells[currentRow, RENEWAL_EMPLOYEE_NUMBER].Value = member.EmployeeNumber;
                    ws.Cells[currentRow, RENEWAL_TYPE].Value = member.Type;
                    ws.Cells[currentRow, RENEWAL_PRINCIPAL_MEMBER_CODE].Value = member.PrincipalMemberCode;
                    ws.Cells[currentRow, RENEWAL_GENDER].Value = member.Gender;
                    ws.Cells[currentRow, RENEWAL_CIVIL_STATUS].Value = member.CivilStatus;
                    ws.Cells[currentRow, RENEWAL_WAIVER].Value = member.Waiver;
                    ws.Cells[currentRow, RENEWAL_EFFECTIVITY_DATE].Value = member.EffectivityDate;
                    ws.Cells[currentRow, RENEWAL_VALIDITY_DATE].Value = member.ValidityDate;
                    ws.Cells[currentRow, RENEWAL_REMARKS].Value = member.Remarks;

                    var appliedPlanAddress = ws.Cells[currentRow, RENEWAL_APPLIED_PLAN].Address;
                    var allowedPlansAddress = ws.Cells[currentRow, RENEWAL_ALLOWED_PLANS].Address;

                    var appliedPlanValidation = ws.DataValidations.AddListValidation(appliedPlanAddress);
                    var allowedPlansValidation = ws.DataValidations.AddListValidation(allowedPlansAddress);
                    if (member.Type == "Dependent")
                    {
                        appliedPlanValidation.Formula.ExcelFormula = String.Format("'AvailablePlansForDependent'!$A$1:$A${0}", dependentPlanCount);
                        ws.Cells[currentRow, RENEWAL_APPLIED_PLAN].Value = (dependentRoomRates.FirstOrDefault(t => t.Id == member.AppliedPlan) ?? new LegacyRoomRate()).DescriptionForExcel;
                        allowedPlansValidation.Formula.ExcelFormula = String.Format("'AvailablePlansForDependent'!$A$1:$A${0}", dependentPlanCount);
                    }
                    else
                    {
                        appliedPlanValidation.Formula.ExcelFormula = String.Format("'AvailablePlansForPrincipal'!$A$1:$A${0}", principalPlanCount);
                        ws.Cells[currentRow, RENEWAL_APPLIED_PLAN].Value = (principalRoomRates.FirstOrDefault(t => t.Id == member.AppliedPlan) ?? new LegacyRoomRate()).DescriptionForExcel;
                        allowedPlansValidation.Formula.ExcelFormula = String.Format("'AvailablePlansForPrincipal'!$A$1:$A${0}", principalPlanCount);
                    }
                    appliedPlanValidation.ShowErrorMessage = true;
                    appliedPlanValidation.ErrorTitle = "An invalid item was selected";
                    appliedPlanValidation.Error = "Selected item must be in the list";
                    allowedPlansValidation.ShowErrorMessage = true;
                    allowedPlansValidation.ErrorTitle = "An invalid item was selected";
                    allowedPlansValidation.Error = "Selected item must be in the list";

                    currentRow++;
                }

                fileBuffer = package.GetAsByteArray();
            }


            System.IO.File.Delete(targetPath);

            return File(fileBuffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", String.Format("Active Members for Renewal ({0:MMddyyyyhhmmss}).xlsm", DateTime.Now));
        }

        public ActionResult ProcessBatch(Guid guid, string accountCode)
        {
            var batch = db.EndorsementBatches.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (batch == null) throw new Exception("Batch Not Found!.");

            if (batch.EndorsementType == "Renewal")
            {
                Helper.CreateMembersForRenewalBatch(db, batch.Id);
                var renewalMembers = db.RenewalMembers.Where(t => !t.Deleted && t.EndorsementBatchId == batch.Id).ToList();
                Helper.MembershipRenewal(System.Web.HttpContext.Current, renewalMembers);
                Helper.CorpAdminMembershipRenewalBatchSummary(System.Web.HttpContext.Current, renewalMembers, accountCode);
            }

            Helper.CreateUserForBatch(batch.Id);

            var members = db.Members.Where(t => !t.Deleted && t.EndorsementBatchId == batch.Id).ToList();
            Helper.BatchSummaryNotification(System.Web.HttpContext.Current, members, accountCode);

            batch.Status = Corelib.Enums.EndorsementBatchStatus.ForEmployeeUpdating;
            db.Entry(batch).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { accountCode = accountCode, messageType = "Success!", message = "Endorsement batch has been process." });
        }

        public ActionResult DeleteBatch(Guid guid, string accountCode)
        {
            var batch = db.EndorsementBatches.Include(t => t.Members).FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (batch == null) throw new Exception("Batch Not Found!.");

            var cancelledMember = db.CancelledMembers.FirstOrDefault(t => t.EndorsementBatchId == batch.Id);
            if (cancelledMember != null) db.CancelledMembers.Remove(cancelledMember);

            db.EndorsementBatches.Remove(batch);
            db.SaveChanges();
            return RedirectToAction("Index", new { accountCode = accountCode, messageType = "Success!", message = "Endorsement batch is successfully deleted." });
        }

        #endregion

        #region -- Functions --

        private string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName)) viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        private IEnumerable<MemberWrapper> ImportNewExcel(string accountCode, int endorsementBatchId, string path)
        {
            var returnValue = new List<MemberWrapper>();
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var wb = package.Workbook;
                if (wb == null) throw new Exception("Error opening workbook.");
                if (wb.Worksheets.Count <= 0) throw new Exception("No worksheet found.");

                var ws = wb.Worksheets[1];
                var startRow = 3;
                var lastRow = 3;
                while (true)
                {
                    var employeeNumber = Convert.ToString(ws.Cells[lastRow, NEW_EMPLOYEE_NUMBER].Value).Trim();
                    if (string.IsNullOrEmpty(employeeNumber))
                    {
                        lastRow--;
                        break;
                    }
                    lastRow++;
                }

                for (int currentRow = startRow; currentRow <= lastRow; currentRow++)
                {

                    returnValue.Add(new MemberWrapper()
                    {
                        EmailAddress = StringValue(ws.Cells[currentRow, NEW_EMAIL_ADDRESS]),
                        EmployeeNumber = StringValue(ws.Cells[currentRow, NEW_EMPLOYEE_NUMBER]),
                        Area = StringValue(ws.Cells[currentRow, NEW_AREA]),
                        CostCenter = StringValue(ws.Cells[currentRow, NEW_COST_CENTER]),
                        LastName = StringValue(ws.Cells[currentRow, NEW_LAST_NAME]),
                        FirstName = StringValue(ws.Cells[currentRow, NEW_FIRST_NAME]),
                        MiddleName = StringValue(ws.Cells[currentRow, NEW_MIDDLE_NAME]),
                        Suffix = StringValue(ws.Cells[currentRow, NEW_SUFFIX]),
                        Gender = StringValue(ws.Cells[currentRow, NEW_GENDER]),
                        DateOfBirth = DateValue(ws.Cells[currentRow, NEW_DATE_OF_BIRTH]),
                        CivilStatus = StringValue(ws.Cells[currentRow, NEW_CIVIL_STATUS]),
                        EffectivityDate = DateValue(ws.Cells[currentRow, NEW_EFFECTIVITY_DATE]),
                        ValidityDate = DateValue(ws.Cells[currentRow, NEW_VALIDITY_DATE]),
                        DateHired = DateValue(ws.Cells[currentRow, NEW_DATE_HIRED]),
                        AppliedPlanFromExcel = StringValue(ws.Cells[currentRow, NEW_APPLIED_PLAN]),
                        AllowedPlansFromExcel = StringValue(ws.Cells[currentRow, NEW_OPTIONAL_PLAN]),
                        Dependent = IntegerValue(ws.Cells[currentRow, NEW_NUMBER_OF_ALLOWED_DEPENDENTS]),
                        DependentAppliedPlanFromExcel = StringValue(ws.Cells[currentRow, NEW_DEPENDENT_APPLIED_PLAN]),
                        AllowedDependentPlansFromExcel = StringValue(ws.Cells[currentRow, NEW_DEPENDENT_OPTIONAL_PLAN]),
                        AccountCode = accountCode,
                        EndorsementBatchId = endorsementBatchId.ToString(),
                        EndorsementType = "New",
                        Code = Helper.GenerateMemberCode()
                    });
                }
            }

            return returnValue;
        }

        private IEnumerable<RenewalMemberWrapper> ImportRenewalExcel(string accountCode, int endorsementBatchId, string path)
        {
            var returnValue = new List<RenewalMemberWrapper>();
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var wb = package.Workbook;
                if (wb == null) throw new Exception("Error opening workbook.");
                if (wb.Worksheets.Count <= 0) throw new Exception("No worksheet found.");

                var ws = wb.Worksheets[1];
                var startRow = 3;
                var lastRow = 3;
                while (true)
                {
                    var memberCode = Convert.ToString(ws.Cells[lastRow, RENEWAL_MEMBER_CODE].Value).Trim();
                    if (string.IsNullOrEmpty(memberCode))
                    {
                        lastRow--;
                        break;
                    }
                    lastRow++;
                }

                for (int currentRow = startRow; currentRow <= lastRow; currentRow++)
                {

                    returnValue.Add(new RenewalMemberWrapper()
                    {
                        Code = StringValue(ws.Cells[currentRow, RENEWAL_MEMBER_CODE]),
                        LastName = StringValue(ws.Cells[currentRow, RENEWAL_LAST_NAME]),
                        FirstName = StringValue(ws.Cells[currentRow, RENEWAL_FIRST_NAME]),
                        MiddleName = StringValue(ws.Cells[currentRow, RENEWAL_MIDDLE_NAME]),
                        EmailAddress = StringValue(ws.Cells[currentRow, RENEWAL_EMAIL]),
                        DateOfBirth = StringValue(ws.Cells[currentRow, RENEWAL_DATE_OF_BIRTH]),
                        Area = StringValue(ws.Cells[currentRow, RENEWAL_AREA]),
                        EmployeeNumber = StringValue(ws.Cells[currentRow, RENEWAL_EMPLOYEE_NUMBER]),
                        AppliedPlanFromExcel = StringValue(ws.Cells[currentRow, RENEWAL_APPLIED_PLAN]),
                        AllowedPlansFromExcel = StringValue(ws.Cells[currentRow, RENEWAL_ALLOWED_PLANS]),
                        Type = StringValue(ws.Cells[currentRow, RENEWAL_TYPE]),
                        PrincipalMemberCode = StringValue(ws.Cells[currentRow, RENEWAL_PRINCIPAL_MEMBER_CODE]),
                        Gender = StringValue(ws.Cells[currentRow, RENEWAL_GENDER]),
                        CivilStatus = StringValue(ws.Cells[currentRow, RENEWAL_CIVIL_STATUS]),
                        Waiver = StringValue(ws.Cells[currentRow, RENEWAL_WAIVER]),
                        EffectivityDate = StringValue(ws.Cells[currentRow, RENEWAL_EFFECTIVITY_DATE]),
                        ValidityDate = StringValue(ws.Cells[currentRow, RENEWAL_VALIDITY_DATE]),
                        Remarks = StringValue(ws.Cells[currentRow, RENEWAL_REMARKS]),
                        AccountCode = accountCode,
                        EndorsementBatchId = endorsementBatchId.ToString()
                    });
                }
            }

            return returnValue;
        }

        private IEnumerable<CancelledMemberWrapper> ImportCancellationExcel(string accountCode, int endorsementBatchId, string path)
        {
            var returnValue = new List<CancelledMemberWrapper>();
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var wb = package.Workbook;
                if (wb == null) throw new Exception("Error opening workbook.");
                if (wb.Worksheets.Count <= 0) throw new Exception("No worksheet found.");

                var ws = wb.Worksheets[1];
                var startRow = 3;
                var lastRow = 3;
                while (true)
                {
                    var memberCode = Convert.ToString(ws.Cells[lastRow, CANCELLATION_MEMBER_CODE].Value).Trim();
                    if (string.IsNullOrEmpty(memberCode))
                    {
                        lastRow--;
                        break;
                    }
                    lastRow++;
                }

                for (int currentRow = startRow; currentRow <= lastRow; currentRow++)
                {

                    returnValue.Add(new CancelledMemberWrapper()
                    {
                        MemberCode = StringValue(ws.Cells[currentRow, CANCELLATION_MEMBER_CODE]),
                        LastName = StringValue(ws.Cells[currentRow, CANCELLATION_LAST_NAME]),
                        FirstName = StringValue(ws.Cells[currentRow, CANCELLATION_FIRST_NAME]),
                        MiddleName = StringValue(ws.Cells[currentRow, CANCELLATION_MIDDLE_NAME]),
                        EmailAddress = StringValue(ws.Cells[currentRow, CANCELLATION_EMAIL]),
                        DateOfBirth = StringValue(ws.Cells[currentRow, CANCELLATION_DATE_OF_BIRTH]),
                        Area = StringValue(ws.Cells[currentRow, CANCELLATION_AREA]),
                        EmployeeNumber = StringValue(ws.Cells[currentRow, CANCELLATION_EMPLOYEE_NUMBER]),
                        AppliedPlanFromExcel = StringValue(ws.Cells[currentRow, CANCELLATION_APPLIED_PLAN]),
                        Type = StringValue(ws.Cells[currentRow, CANCELLATION_TYPE]),
                        Gender = StringValue(ws.Cells[currentRow, CANCELLATION_GENDER]),
                        CivilStatus = StringValue(ws.Cells[currentRow, CANCELLATION_CIVIL_STATUS]),
                        Waiver = StringValue(ws.Cells[currentRow, CANCELLATION_WAIVER]),
                        EffectivityDate = StringValue(ws.Cells[currentRow, CANCELLATION_EFFECTIVITY_DATE]),
                        ValidityDate = StringValue(ws.Cells[currentRow, CANCELLATION_VALIDITY_DATE]),
                        Remarks = StringValue(ws.Cells[currentRow, CANCELLATION_REMARKS]),
                        EndorsementBatchId = endorsementBatchId.ToString()
                    });
                }
            }

            return returnValue;
        }

        #endregion

        #region -- Excel Extensions --

        public string StringValue(ExcelRange range)
        {
            return Convert.ToString(range.Value).Trim();
        }

        public string IntegerValue(ExcelRange range)
        {
            int? returnValue = null;
            int temp;
            if (int.TryParse(StringValue(range), out temp)) returnValue = int.Parse(StringValue(range));
            return returnValue.HasValue ? returnValue.Value.ToString() : null;
        }

        public string DateValue(ExcelRange range)
        {
            DateTime? returnValue = null;
            DateTime temp;
            if (DateTime.TryParse(StringValue(range), out temp)) returnValue = DateTime.Parse(StringValue(range));
            return returnValue.HasValue ? returnValue.Value.ToString("MM/dd/yyyy") : null;
        }

        #endregion

        #region -- Single Endorsement --

        public ActionResult SingleEndorsement(string accountCode, Guid? guid)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            EndorsementBatch model;
            if (guid != null && db.EndorsementBatches.Any(t => t.Guid == guid))
            {
                model = db.EndorsementBatches.Include(t => t.Members).FirstOrDefault(t => t.Guid == guid);
            }
            else
            {
                model = new EndorsementBatch()
                {
                    ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber()
                };
            }

            base.ReadOnlyAttribute(model);

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingleEndorsement(EndorsementBatch endorsementBatch, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            endorsementBatch.Date = DateTime.Now;
            endorsementBatch.EndorsementType = "New";
            endorsementBatch.BatchType = "Single Endorsement";
            endorsementBatch.AccountCode = accountCode;
            endorsementBatch.Status = Corelib.Enums.EndorsementBatchStatus.New;

            if (ModelState.IsValid)
            {
                if (endorsementBatch.Id == 0)
                {
                    db.EndorsementBatches.Add(endorsementBatch);
                    if (endorsementBatch.Members != null)
                    {
                        var member = endorsementBatch.Members.FirstOrDefault();
                        member.EndorsementBatchId = endorsementBatch.Id;
                        db.Members.Add(member);
                    }
                }
                else
                {
                    db.Entry(endorsementBatch).State = EntityState.Modified;
                    if (endorsementBatch.Members != null)
                    {
                        var member = endorsementBatch.Members.FirstOrDefault();
                        var currentMember = db.Members.FirstOrDefault(t => t.Id == member.Id && !t.Deleted);
                        db.Entry(currentMember).CurrentValues.SetValues(member);
                        currentMember.EndorsementBatchId = endorsementBatch.Id;
                        db.Entry(currentMember).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { accountCode = accountCode });
            }

            base.ReadOnlyAttribute(endorsementBatch);

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);

            return View(endorsementBatch);
        }

        #endregion

        #region -- Cancelled Membership --

        public ActionResult CancelledMember(string accountCode, Guid? guid)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            var activeMembers = Helper.GetActiveMembers(db, legacyDb, accountCode);

            CancelledMember model;
            if (guid != null && db.EndorsementBatches.Any(t => t.Guid == guid))
            {
                var endorsement = db.EndorsementBatches.FirstOrDefault(t=>t.Guid == guid && !t.Deleted);
                if (endorsement == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
                model = db.CancelledMembers.Include(t => t.EndorsementBatch).FirstOrDefault(t => t.EndorsementBatchId == endorsement.Id && !t.Deleted);
                if (model == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                base.CancelledMemberReadOnlyAttribute(true);
            }
            else
            {
                model = new CancelledMember() { Status = Corelib.Enums.CancelledMembershipStatus.CorporateAdminApproved };
                base.CancelledMemberReadOnlyAttribute(false);
            }

            ViewBag.MemberList = activeMembers;
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelledMember(CancelledMember model, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;
            
            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var endorsementBatch = new EndorsementBatch()
                    {
                        Deadline = DateTime.Now,
                        ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber(),
                        Date = DateTime.Now,
                        EndorsementType = "Cancel Membership",
                        BatchType = "Single Cancellation",
                        EndorsementCount = 1,
                        AccountCode = accountCode,
                        Status = Corelib.Enums.EndorsementBatchStatus.New
                    };
                    db.EndorsementBatches.Add(endorsementBatch);
                    model.EndorsementBatch = endorsementBatch;
                    db.CancelledMembers.Add(model);
                }
                else
                {
                    db.Entry(model).State = EntityState.Modified;
                }
                db.SaveChanges();

                var cancellmembers = new List<CancelledMember>();
                cancellmembers.Add(model);
                Helper.CorpAdminMembershipCancellationBatchSummary(System.Web.HttpContext.Current, cancellmembers, accountCode);
                Helper.MembershipCancellation(System.Web.HttpContext.Current, cancellmembers);

                return RedirectToAction("Index", new { accountCode = accountCode });
            }

            base.CancelledMemberReadOnlyAttribute(false);

            var activeMembers = Helper.GetActiveMembers(db, legacyDb, accountCode);
            ViewBag.MemberList = activeMembers;
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            
            return View(model);
        }

        public ActionResult GetMemberProfile(string memberCode,string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue; 

            var activeMembers = Helper.GetActiveMembers(db, legacyDb, accountCode);
            var activeMember = activeMembers.FirstOrDefault(t => t.Code == memberCode);

            base.CancelledMemberReadOnlyAttribute(false);
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            if (activeMember == null){
                return PartialView("_CancelledMember", new CancelledMember());
            }

            return PartialView("_CancelledMember", new CancelledMember(activeMember));
        }

        public FileResult DownloadCancellationTemplate(string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null)
            {
                return null;
            }

            var account = legacyDb.LegacyAccounts
                .Include(t => t.LegacyRoomRates)
                .Include(t => t.LegacyRoomRates.Select(lr => lr.LegacyPaymode))
                .Include(t => t.LegacyRoomRates.Select(rr => rr.LegacyPlan))
                .FirstOrDefault(t => t.Code == accountCode);

            var activeMembers = Helper.GetActiveMembers(db, legacyDb, accountCode);

            byte[] fileBuffer;
            var templatePath = Server.MapPath("~/ExcelTemplates/Cancel Member.xlsm");
            var targetPath = String.Format(@"{0}\{1}.xlsm", Server.MapPath("~/Uploads"), Guid.NewGuid());
            System.IO.File.Copy(templatePath, targetPath);

            using (var package = new ExcelPackage(new FileInfo(targetPath)))
            {
                var wb = package.Workbook;
                if (wb == null) throw new Exception("Invalid WorkBook.");
                if (wb.Worksheets.Count <= 0) throw new Exception("Worksheet doesn't exist.");
                var ws = wb.Worksheets[1];

                var principalPlanCount = 0;
                var dependentPlanCount = 0;
                var availablePlansWS = wb.Worksheets["AvailablePlansForPrincipal"];
                var row = 1;
                var principalRoomRates = account.LegacyRoomRates.Where(t => t.PaymentFor == 0 || t.PaymentFor == 5);
                foreach (var legacyRoomRate in principalRoomRates)
                {
                    availablePlansWS.Cells[row, 1].Value = String.Format("{1} - {2} ({3:Php 0,0.00} Limit)|{0}", legacyRoomRate.Id, legacyRoomRate.LegacyPlan.Description, legacyRoomRate.For, legacyRoomRate.Limit);
                    row++;
                    principalPlanCount++;
                }

                availablePlansWS = wb.Worksheets["AvailablePlansForDependent"];
                row = 1;
                var dependentRoomRates = account.LegacyRoomRates.Where(t => t.PaymentFor == 1 || t.PaymentFor == 5);
                foreach (var legacyRoomRate in dependentRoomRates)
                {
                    availablePlansWS.Cells[row, 1].Value = String.Format("{1} - {2} ({3:Php 0,0.00} Limit)|{0}", legacyRoomRate.Id, legacyRoomRate.LegacyPlan.Description, legacyRoomRate.For, legacyRoomRate.Limit);
                    row++;
                    dependentPlanCount++;
                }

                var currentRow = 3;
                foreach (var member in activeMembers)
                {
                    ws.Cells[currentRow, CANCELLATION_MEMBER_CODE].Value = member.Code;
                    ws.Cells[currentRow, CANCELLATION_LAST_NAME].Value = member.LastName;
                    ws.Cells[currentRow, CANCELLATION_FIRST_NAME].Value = member.FirstName;
                    ws.Cells[currentRow, CANCELLATION_MIDDLE_NAME].Value = member.MiddleName;
                    ws.Cells[currentRow, CANCELLATION_EMAIL].Value = member.EmailAddress;
                    ws.Cells[currentRow, CANCELLATION_DATE_OF_BIRTH].Value = member.DateOfBirth;
                    ws.Cells[currentRow, CANCELLATION_AREA].Value = member.Area;
                    ws.Cells[currentRow, CANCELLATION_EMPLOYEE_NUMBER].Value = member.EmployeeNumber;
                    ws.Cells[currentRow, CANCELLATION_TYPE].Value = member.Type;
                    ws.Cells[currentRow, CANCELLATION_GENDER].Value = member.Gender;
                    ws.Cells[currentRow, CANCELLATION_CIVIL_STATUS].Value = member.CivilStatus;
                    ws.Cells[currentRow, CANCELLATION_WAIVER].Value = member.Waiver;
                    ws.Cells[currentRow, CANCELLATION_EFFECTIVITY_DATE].Value = member.EffectivityDate;
                    ws.Cells[currentRow, CANCELLATION_VALIDITY_DATE].Value = member.ValidityDate;
                    ws.Cells[currentRow, CANCELLATION_REMARKS].Value = member.Remarks;

                    var appliedPlanAddress = ws.Cells[currentRow, RENEWAL_APPLIED_PLAN].Address;

                    var appliedPlanValidation = ws.DataValidations.AddListValidation(appliedPlanAddress);
                    if (member.Type == "Dependent")
                    {
                        appliedPlanValidation.Formula.ExcelFormula = String.Format("'AvailablePlansForDependent'!$A$1:$A${0}", dependentPlanCount);
                        ws.Cells[currentRow, RENEWAL_APPLIED_PLAN].Value = (dependentRoomRates.FirstOrDefault(t => t.Id == member.AppliedPlan) ?? new LegacyRoomRate()).DescriptionForExcel;
                        
                    }
                    else
                    {
                        appliedPlanValidation.Formula.ExcelFormula = String.Format("'AvailablePlansForPrincipal'!$A$1:$A${0}", principalPlanCount);
                        ws.Cells[currentRow, RENEWAL_APPLIED_PLAN].Value = (principalRoomRates.FirstOrDefault(t => t.Id == member.AppliedPlan) ?? new LegacyRoomRate()).DescriptionForExcel;
                        
                    }
                    appliedPlanValidation.ShowErrorMessage = true;
                    appliedPlanValidation.ErrorTitle = "An invalid item was selected";
                    appliedPlanValidation.Error = "Selected item must be in the list";

                    currentRow++;
                }

                fileBuffer = package.GetAsByteArray();
            }

            System.IO.File.Delete(targetPath);

            return File(fileBuffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", String.Format("Active Members for Cancellation of Membership ({0:MMddyyyyhhmmss}).xlsm", DateTime.Now));
        }

        public ActionResult BatchUploadCancellation(string accountCode, Guid? guid)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            EndorsementBatch model;
            if (guid != null && db.EndorsementBatches.Any(t => t.Guid == guid))
            {
                model = db.EndorsementBatches
                    .Include(t => t.CancelledMembers)
                    .FirstOrDefault(t => t.Guid == guid);
                ViewBag.Validate = true;
            }
            else
            {
                model = new EndorsementBatch()
                {
                    ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber()
                };
            }

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            ViewBag.Validate = true;

            base.ReadOnlyAttribute(model);

            return View(model);
        }

        public JsonResult UploadCancellationExcel(HttpPostedFileBase fileData, string accountCode, int endorsementBatchId)
        {
            var fileExtension = fileData.FileName.Substring(fileData.FileName.LastIndexOf(".") + 1);
            if (fileExtension != "xlsm") throw new Exception("File Not Valid");

            var originalFilename = fileData.FileName;
            var originalFilepath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", originalFilename));

            var filename = String.Format("{0}.{1}", Guid.NewGuid().ToString(), fileExtension);
            var filepath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", filename));

            fileData.SaveAs(filepath);

            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Filename", originalFilename);
            dictionary.Add("GuidFilename", filename);

            dictionary.Add("TableData", RenderPartialViewToString("~/Areas/CorporateAdministrator/Views/Endorsement/_CancelledMemberWrapper.cshtml", ImportCancellationExcel(accountCode, endorsementBatchId, filepath)));

            return this.Json(dictionary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateUploadedCancellationExcel(EndorsementBatch endorsementBatch, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            ViewBag.Validate = true;
            if (endorsementBatch.CancelledMembers != null)
            {
                foreach (var member in endorsementBatch.CancelledMembers)
                {
                    member.EndorsementBatchId = endorsementBatch.Id;
                }
            }

            base.ReadOnlyAttribute(endorsementBatch);

            return View("BatchUploadCancellation", endorsementBatch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchUploadCancellation(EndorsementBatch endorsementBatch, string accountCode)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            endorsementBatch.Date = DateTime.Now;
            endorsementBatch.EndorsementType = "Cancel Membership";
            endorsementBatch.BatchType = "Batch Upload";
            endorsementBatch.AccountCode = accountCode;
            endorsementBatch.Status = Corelib.Enums.EndorsementBatchStatus.New;

            if (ModelState.IsValid)
            {
                if (endorsementBatch.Id == 0)
                {
                    db.EndorsementBatches.Add(endorsementBatch);
                }
                else
                {
                    var currentCancelMemberIds = endorsementBatch.CancelledMembers.Select(t => t.Id).Distinct();
                    var cancelMembersToDelete = db.CancelledMembers.Where(t => t.EndorsementBatchId == endorsementBatch.Id && !currentCancelMemberIds.Contains(t.Id));
                    db.CancelledMembers.RemoveRange(cancelMembersToDelete);

                    foreach (var cancelledMember in endorsementBatch.CancelledMembers)
                    {
                        if (cancelledMember.Id == 0)
                        {
                            cancelledMember.Status = Corelib.Enums.CancelledMembershipStatus.CorporateAdminApproved;
                            db.Entry(cancelledMember).State = EntityState.Added;
                        }
                    }
                    db.Entry(endorsementBatch).State = EntityState.Modified;
                    if (endorsementBatch.CancelledMembers != null && endorsementBatch.CancelledMembers.Count > 0)
                    {
                        for (var index = endorsementBatch.CancelledMembers.Count - 1; index >= 0; index--)
                        {
                            var cancelledMember = endorsementBatch.CancelledMembers.ElementAt(index);
                            if (cancelledMember.Id != 0) db.Entry(cancelledMember
                            ).State = EntityState.Modified;
                        }
                    }
                }
                                
                endorsementBatch.EndorsementCount = endorsementBatch.CancelledMembers != null ? endorsementBatch.CancelledMembers.Count(t => !t.Deleted) : 0;
                db.SaveChanges();

                var cancellmembers = new List<CancelledMember>(endorsementBatch.CancelledMembers);
                Helper.CorpAdminMembershipCancellationBatchSummary(System.Web.HttpContext.Current, cancellmembers, accountCode);
                Helper.MembershipCancellation(System.Web.HttpContext.Current, cancellmembers);

                return RedirectToAction("Index", new { accountCode = accountCode });
            }

            base.ReadOnlyAttribute(endorsementBatch);

            ViewBag.Genders = new List<string>() { "Male", "Female" };
            ViewBag.CivilStatuses = new List<string>() { "Single", "Married", "Divorced", "Widowed" };
            ViewBag.PrincipalPlans = Helper.GetLegacyRoomRates(accountCode, true);
            ViewBag.DependentPlans = Helper.GetLegacyRoomRates(accountCode, false);
            ViewBag.Validate = true;

            return View(endorsementBatch);
        }
        
        #endregion
    }
}