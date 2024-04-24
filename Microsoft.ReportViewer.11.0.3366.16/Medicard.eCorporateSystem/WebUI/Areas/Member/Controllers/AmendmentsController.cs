﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Member.Models;
using PagedList;
using Corelib;
using Corelib.Models;
using Corelib.Enums;
using System.IO;

namespace WebUI.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class AmendmentsController : BaseMemberController
    {
        #region -- Action Results --
        
        [Authorize(Roles = "Member")]
        public ActionResult Index(string sortOrder, string currentFilter, int? page, string messageType, string message)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;

            var model = db.Amendments
                .Include(t => t.EndorsementBatch)
                .Include(t => t.Reason)
                .Include(t => t.DocumentType)
                .Where(t => !t.Deleted && t.MemberId == this.Member.Id)
                .OrderByDescending(t => t.RequestDate).ToList();

            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted).ToList();

            return View(model.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }
        
        [Authorize(Roles = "Member")]
        public ActionResult Amendment(Guid? guid)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;
                        
            Amendment model;
            if (guid != null && db.Amendments.Any(t => t.Guid == guid))
            {
                model = db.Amendments.Include(t => t.EndorsementBatch).FirstOrDefault(t => t.Guid == guid);
            }
            else
            {
                model = new Amendment()
                {
                    MemberId = this.Member.Id,
                    Status = Corelib.Enums.RequestStatus.Saved
                };
            }

            base.AmendmentReadOnlyAttribute(model);

            ViewBag.Reasons = db.Reasons.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted && t.MemberId == this.Member.Id).OrderBy(t => t.LastName);
            ViewBag.DocumentTypes = db.DocumentTypes.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Members = db.Members.Where(t => !t.Deleted && t.Id == this.Member.Id).OrderBy(t => t.LastName);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public ActionResult Amendment(Amendment model, string submit)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;
                        
            if (model.FileWrapper != null)
            {
                using (var binaryReader = new BinaryReader(model.FileWrapper.InputStream))
                {
                    model.DocumentFile = binaryReader.ReadBytes(model.FileWrapper.ContentLength);
                    model.DocumentContentType = model.FileWrapper.ContentType;
                    model.DocumentFileName = model.FileWrapper.FileName;
                }
            }

            if (ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var endorsementBatch = new EndorsementBatch()
                    {
                        Deadline = DateTime.Now,
                        ReferenceNumber = Helper.GenerateRandomEndorsementBatchReferenceNumber(),
                        Date = DateTime.Now,
                        EndorsementType = "Amendment",
                        BatchType = "Amendment",
                        AccountCode = this.Member.AccountCode,
                        Status = Corelib.Enums.EndorsementBatchStatus.New
                    };
                    db.EndorsementBatches.Add(endorsementBatch);

                    model.AccountCode = this.Member.AccountCode;
                    model.EndorsementBatchId = endorsementBatch.Id;
                    db.Amendments.Add(model);
                    if (submit == "Submit Request")
                    {
                        var setting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == this.Member.AccountCode) ?? new AccountSetting();
                        if (setting.BypassHRManagerApproval)
                        {
                            model.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
                        }
                        else
                        {
                            model.Status = Corelib.Enums.RequestStatus.Submitted;
                        }
                        Helper.MemberAmendment(System.Web.HttpContext.Current, model, setting.BypassHRManagerApproval);
                    }                    
                }
                else
                {
                    var currentAmendment = db.Amendments.FirstOrDefault(t => t.Id == model.Id && !t.Deleted);
                    db.Entry(currentAmendment).CurrentValues.SetValues(model);
                    db.Entry(currentAmendment).State = EntityState.Modified;
                    if (submit == "Submit Request")
                    {
                        var setting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == this.Member.AccountCode) ?? new AccountSetting();
                        if (setting.BypassHRManagerApproval)
                        {
                            currentAmendment.Status = Corelib.Enums.RequestStatus.CorporateAdminApproved;
                        }
                        else
                        {
                            currentAmendment.Status = Corelib.Enums.RequestStatus.Submitted;
                        }
                        Helper.MemberAmendment(System.Web.HttpContext.Current, model, setting.BypassHRManagerApproval);
                    }  
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            base.AmendmentReadOnlyAttribute(model);

            ViewBag.Reasons = db.Reasons.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);
            ViewBag.Dependents = db.Dependents.Where(t => !t.Deleted && t.MemberId == this.Member.Id).OrderBy(t => t.LastName);
            ViewBag.DocumentTypes = db.DocumentTypes.Where(t => !t.Deleted).OrderBy(t => t.DisplayOrder);

            return View(model);
        }
        
        [Authorize(Roles = "Member")]
        public ActionResult CancelAmendment(Guid? guid)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;

            var amendment = db.Amendments.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (amendment == null) throw new Exception("Amendment Not Found!.");

            amendment.Status = Corelib.Enums.RequestStatus.Saved;
            db.Entry(amendment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { messageType = "Success!", message = "Amendment Successfully cancelled." });
        }
        
        [Authorize(Roles = "Member")]
        public ActionResult Delete(Guid? guid)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;

            var amendment = db.Amendments.FirstOrDefault(t => !t.Deleted && t.Guid == guid);
            if (amendment == null) throw new Exception("Amendment Not Found!.");

            db.Entry(amendment).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index", new { messageType = "Success!", message = "Amendment successfully deleted ." });
        }

        #endregion

        #region -- Functions --
                
        public FileResult DownloadFile(Guid? guid)
        {
            if (guid != null && db.Amendments.Any(t => t.Guid == guid))
            {
                var amendment = db.Amendments.FirstOrDefault(t => t.Guid == guid);
                if (amendment.DocumentFile != null || !string.IsNullOrEmpty(amendment.DocumentContentType) || !string.IsNullOrEmpty(amendment.DocumentFileName))
                { return File(amendment.DocumentFile, amendment.DocumentContentType, amendment.DocumentFileName); }
            }
            return null;
        }

        public string GetPropertyValue(int? memberId, int propertyName, int requestForId, int? dependentId)
        {
            var dataType = Convert.ToString((Corelib.Enums.RequestDataType)propertyName);
            var requestFor = (Corelib.Enums.RequestFor)requestForId;
            var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == memberId);
            var dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.MemberId == memberId && t.Id == dependentId.Value);
            var returnValue = "";
            switch (requestFor)
            {
                case Corelib.Enums.RequestFor.Principal:
                    if (member != null)
                    {
                        var propertyInfo = member.GetType().GetProperty(dataType);

                        if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            var date = Convert.ToDateTime(propertyInfo.GetValue(member, null));
                            returnValue = date.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            returnValue = Convert.ToString(propertyInfo.GetValue(member, null));
                        }
                        return returnValue;
                    }
                    return "Member not found.";
                case Corelib.Enums.RequestFor.Dependent:
                    if (dependent != null && member != null)
                    {
                        var propertyInfo = dependent.GetType().GetProperty(dataType);
                        if (propertyInfo != null)
                        {
                            if (propertyInfo.PropertyType == typeof(DateTime))
                            {
                                var date = Convert.ToDateTime(propertyInfo.GetValue(dependent, null));
                                returnValue = date.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                returnValue = Convert.ToString(propertyInfo.GetValue(dependent, null));
                            }
                            return returnValue;
                        }
                        return "Data type of dependent not found.";
                    }
                    return "Select dependent name.";
                default:
                    if (member != null)
                    {
                        var accounts = Helper.GetLegacyAccounts(db, legacyDb);
                        var account = accounts.FirstOrDefault(t => t.Code == member.AccountCode);
                        if (account == null) break;
                        var propertyInfo = account.GetType().GetProperty("Name");
                        if (propertyInfo != null)
                        {
                            returnValue = Convert.ToString(propertyInfo.GetValue(account, null));

                            return returnValue;
                        }
                        return "Data type of Account not found.";
                    }
                    return "Member not found.";
            }
            return returnValue;
        }

        public bool IsPropertyDateTime(int? memberId, int propertyName)
        {
            var dataType = Convert.ToString((Corelib.Enums.RequestDataType)propertyName);
            var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == memberId);
            if (member != null)
            {
                var propertyInfo = member.GetType().GetProperty(dataType);
                if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        
        public string GetDataType(int requestForId)
        {
            var requestFor = (Corelib.Enums.RequestFor)requestForId;
            var sb = new System.Text.StringBuilder();
            switch (requestFor)
            {
                case RequestFor.Dependent:
                    foreach (var datatype in (RequestDataType[])Enum.GetValues(typeof(RequestDataType)))
                    {
                        var intValue = Convert.ChangeType(datatype, datatype.GetTypeCode());
                        var description = EnumHelper<RequestDataType>.GetDisplayValue(datatype);
                        if (datatype != RequestDataType.CostCenter && datatype != RequestDataType.DateHired && datatype != RequestDataType.Area &&
                            datatype != RequestDataType.EffectivityDate)
                        {
                            sb.Append(string.Format(@"<option value='{0}'>{1}</option>", intValue, description));
                        }
                    }
                    break;
                case RequestFor.Account:
                    sb.Append(string.Format(@"<option value=''>Request for account is not applicable for Member</option>"));
                    break;
                default:
                    foreach (var datatype in (RequestDataType[])Enum.GetValues(typeof(RequestDataType)))
                    {
                        var intValue = Convert.ChangeType(datatype, datatype.GetTypeCode());
                        var description = EnumHelper<RequestDataType>.GetDisplayValue(datatype);
                        sb.Append(string.Format(@"<option value='{0}'>{1}</option>", intValue, description));
                    }
                    break;
            }
            return sb.ToString();
        }

        #endregion
            
    }
}