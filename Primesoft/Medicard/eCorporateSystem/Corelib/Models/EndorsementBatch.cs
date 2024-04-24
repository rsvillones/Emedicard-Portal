﻿using Corelib.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class EndorsementBatch : BaseModel, IValidatableObject
    {
        #region -- Constructor --

        public EndorsementBatch()
        {
            this.Members = new Collection<Member>();
            this.Deadline = DateTime.Now.AddDays(30);
        }

        #endregion

        #region -- Properties --

        [Display(Name = "Control Number")]
        public string ControlNumber { get; set; }

        [StringLength(25)]
        [Index]
        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Received")]
        public DateTime? DateReceived { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateForwarded { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateEndorsed { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        [StringLength(128)]
        public string Filename { get; set; }

        [StringLength(64)]
        public string GuidFilename { get; set; }

        public EndorsementBatchStatus Status { get; set; }

        public int EndorsementCount { get; set; }

        private string _accountCode;
        [StringLength(25)]
        [Index]
        public string AccountCode
        {
            get
            {
                return _accountCode;
            }
            set
            {
                _accountCode = value;

                using (var legacyDb = new LegacyDataContext())
                {
                    var legacyAccount = legacyDb.LegacyAccounts.FirstOrDefault(t => t.Code == this.AccountCode) ?? new LegacyAccount();
                    this.CompanyName = legacyAccount.Name;
                }
            }
        }

        [StringLength(150)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Index]
        [StringLength(32)]
        [Display(Name = "Type")]
        public string EndorsementType { get; set; }

        [StringLength(32)]
        public string BatchType { get; set; }

        public string Remarks { get; set; }

        [StringLength(25)]
        [Index]
        public string ReplyTo { get; set; }

        [Display(Name = "Assigned Processor")]
        public string AssignedProcessor { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public virtual ICollection<RenewalMember> RenewalMembers { get; set; }

        public virtual ICollection<ActionMemo> ActionMemos { get; set; }

        public virtual ICollection<CancelledMember> CancelledMembers { get; set; }

        public virtual ICollection<Amendment> Amendments { get; set; }

        public virtual ICollection<IdReplacement> IdReplacements { get; set; }

        public virtual ICollection<AdditionalDependent> AdditionalDependents { get; set; }

        public virtual ICollection<DependentCancellation> DependentCancellations { get; set; }

        #endregion

        #region -- Not Mapped --

        [NotMapped]
        public bool CanBeReceived
        {
            get
            {
                using (var db = new IdentityDataContext())
                {
                    UrgSetting urgSetting = (db.AccountSettings.FirstOrDefault(t => t.AccountCode == this.AccountCode) ?? new AccountSetting()).UrgSetting;

                    if (urgSetting == UrgSetting.All)
                    {
                        switch (this.EndorsementType)
                        {
                            case "New":
                            case "Renewal":
                                if (this.Members.Count == 0) return false;
                                else if (this.Members.Any(t => t.Status == MembershipStatus.New || t.Status == MembershipStatus.Saved || t.Status == MembershipStatus.SubmittedToCorporateAdmin)) return false;
                                else if (this.Members.Any(t => t.Status == MembershipStatus.CorporateAdminApproved)) return true;
                                return false;
                            //case "Action Memo":
                            //    return this.ActionMemos.Count > 0 && !this.ActionMemos.Any(t => t.Status != ActionMemoStatus.Replied);
                            case "Action Memo":
                                return this.ActionMemos.Count > 0 && this.ActionMemos.Any(t => t.Status == ActionMemoStatus.Replied);
                            case "Amendment":
                                return this.Amendments.Count > 0 && !this.Amendments.Any(t => t.Status != RequestStatus.CorporateAdminApproved);
                            case "ID Replacement":
                                return this.IdReplacements.Count > 0 && !this.IdReplacements.Any(t => t.Status != RequestStatus.CorporateAdminApproved);
                            case "Additional Dependent":
                                return this.AdditionalDependents.Count > 0 && !this.AdditionalDependents.Any(t => t.Status != RequestStatus.CorporateAdminApproved);
                            case "Dependent Cancellation":
                                return this.DependentCancellations.Count > 0 && !this.DependentCancellations.Any(t => t.Status != RequestStatus.CorporateAdminApproved);
                            case "Cancel Membership":
                                return this.CancelledMembers.Count > 0 && !this.CancelledMembers.Any(t => t.Status != CancelledMembershipStatus.CorporateAdminApproved);
                        }
                    }
                    else if (urgSetting == UrgSetting.Partial)
                    {
                        switch (this.EndorsementType)
                        {
                            case "New":
                            case "Renewal":
                                return this.Members.Count > 0 && this.Members.Any(t => t.Status == MembershipStatus.CorporateAdminApproved);
                            case "Action Memo":
                                return this.ActionMemos.Count > 0 && this.ActionMemos.Any(t => t.Status == ActionMemoStatus.Replied);
                            case "Amendment":
                                return this.Amendments.Count > 0 && this.Amendments.Any(t => t.Status == RequestStatus.CorporateAdminApproved);
                            case "ID Replacement":
                                return this.IdReplacements.Count > 0 && this.IdReplacements.Any(t => t.Status == RequestStatus.CorporateAdminApproved);
                            case "Additional Dependent":
                                return this.AdditionalDependents.Count > 0 && this.AdditionalDependents.Any(t => t.Status == RequestStatus.CorporateAdminApproved);
                            case "Dependent Cancellation":
                                return this.DependentCancellations.Count > 0 && this.DependentCancellations.Any(t => t.Status == RequestStatus.CorporateAdminApproved);
                            case "Cancel Membership":
                                return this.CancelledMembers.Count > 0 && this.CancelledMembers.Any(t => t.Status == CancelledMembershipStatus.CorporateAdminApproved);
                        }
                    }
                    else if (urgSetting == UrgSetting.Principal)
                    {
                        switch (this.EndorsementType)
                        {
                            case "New":
                            case "Renewal":
                                return this.Members.Count > 0 && this.Members.Any(t => (int)t.Status < (int)MembershipStatus.ForProcessing && t.Status != MembershipStatus.CorporateAdminDisapproved);
                            case "Action Memo":
                                return this.ActionMemos.Count > 0 && this.ActionMemos.Any(t => t.Status == ActionMemoStatus.Replied);
                            case "Amendment":
                                return this.Amendments.Count > 0 && this.Amendments.Any(t => t.Status == RequestStatus.CorporateAdminApproved);
                            case "ID Replacement":
                                return this.IdReplacements.Count > 0 && this.IdReplacements.Any(t => t.Status == RequestStatus.CorporateAdminApproved);
                            case "Additional Dependent":
                                return this.AdditionalDependents.Count > 0 && this.AdditionalDependents.Any(t => t.Status == RequestStatus.CorporateAdminApproved);
                            case "Dependent Cancellation":
                                return this.DependentCancellations.Count > 0 && this.DependentCancellations.Any(t => t.Status == RequestStatus.CorporateAdminApproved);
                            case "Cancel Membership":
                                return this.CancelledMembers.Count > 0 && this.CancelledMembers.Any(t => t.Status == CancelledMembershipStatus.CorporateAdminApproved);
                        }
                    }
                }

                return false;
            }
        }

        [NotMapped]
        public bool CanBeDownloaded
        {
            get
            {
                return this.EndorsementType == "New" || this.EndorsementType == "Renewal" || this.EndorsementType == "Cancel Membership";
            }
        }

        [NotMapped]
        public bool CanBeDownloadedFromHistory
        {
            get
            {
                return this.EndorsementType == "New" || this.EndorsementType == "Renewal";
            }
        }

        [NotMapped]
        public bool HasDetails
        {
            get
            {
                return this.EndorsementType == "New" || this.EndorsementType == "Renewal" || this.EndorsementType == "Cancel Membership" || this.EndorsementType == "Action Memo" || this.EndorsementType == "Amendment" || this.EndorsementType == "ID Replacement"
                    || this.EndorsementType == "Additional Dependent" || this.EndorsementType == "Dependent Cancellation";
            }
        }

        [NotMapped]
        public bool HasEndorsementError
        {
            get
            {
                using (var db = new IdentityDataContext())
                {
                    var returnValue = db.MemberWrappers.Any(t => t.EndorsementBatchGuid == this.Guid && !t.IsValid);
                    return returnValue;
                }
            }
        }

        public bool SendTransmittal { get; set; }

        public bool SendActionMemo { get; set; }

        [NotMapped]
        public bool IsBeingProcess
        {
            get
            {
                using (var db = new IdentityDataContext())
                {
                    return db.EndorsementLogs.Any(t => !t.IsProcessed && t.EndorsementGuid == this.Guid && t.Type != LogType.ForDownload);
                }
            }
        }

        [NotMapped]
        public bool CanBeDeclined
        {
            get
            {
                var returnValue = false;
                switch (this.EndorsementType)
                {
                    case Constants.ID_REPLACEMENT_ENDORSEMENT_TYPE:
                        returnValue = this.Status == EndorsementBatchStatus.ApprovedByCorporateAdmin;
                        break;
                    case Constants.AMENDMENT_ENDORSEMENT_TYPE:
                        foreach (var amendment in this.Amendments)
                        {
                            returnValue = this.Status == EndorsementBatchStatus.ApprovedByCorporateAdmin && Config.IsSimpleAmend(amendment.DataType);
                        }
                        break;
                }
                return returnValue;
            }
        }

        [NotMapped]
        public string MemberName
        {
            get
            {
                if (this.BatchType != "Single") return "";
                switch (this.EndorsementType)
                {
                    case "New":
                        return this.Members.FirstOrDefault() != null ? this.Members.FirstOrDefault().FullName : "";
                    case "Cancel Membership":
                        return this.CancelledMembers.FirstOrDefault() != null ? this.CancelledMembers.FirstOrDefault().FullName : "";
                }
                return "";
            }
        }
        
        #endregion

        #region -- IValidatableObject Members --

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (this.EndorsementType == Constants.CANCEL_MEMBERSHIP_ENDORSEMENT_TYPE)
            {
                var cancellations = this.CancelledMembers.GroupBy(t => new { t.MemberCode });
                var duplicateCancellations = cancellations.SelectMany(group => group.Skip(1).Take(1)).ToList();
                var duplicateCancellationsCount = cancellations.SelectMany(group => group.Skip(1)).ToList().Count();
                if (duplicateCancellations != null && duplicateCancellations.Count() > 0)
                {
                    foreach (var duplicate in duplicateCancellations)
                    {
                        yield return new ValidationResult(String.Format("Duplicate entries found for Member Code {0}. Count {1}", duplicate.MemberCode, duplicateCancellationsCount));
                    }
                }
            }

            using (var db = new IdentityDataContext())
            {
                if (this.EndorsementType == Constants.NEW_ENDORSEMENT_TYPE)
                {
                    var memberWrappers = db.MemberWrappers.Where(t => t.EndorsementBatchGuid == this.Guid).GroupBy(t => new { t.LastName, t.FirstName, t.MiddleName, t.DateOfBirth });
                    var duplicateWrappers = memberWrappers.SelectMany(group => group.OrderBy(t => t.LastName).ThenBy(t => t.FirstName).ThenBy(t => t.MiddleName).ThenBy(t => t.DateOfBirth).Skip(1).Take(1)).ToList();
                    var duplicateWrappersCount = memberWrappers.SelectMany(group => group.OrderBy(t => t.LastName).ThenBy(t => t.FirstName).ThenBy(t => t.MiddleName).ThenBy(t => t.DateOfBirth).Skip(1)).ToList().Count();
                    foreach (var duplicateWrapper in duplicateWrappers)
                    {
                        yield return new ValidationResult(String.Format("Duplicate entries found for {1}, {0} {2} ({3:MM/dd/yyyy}). Count {4}", duplicateWrapper.FirstName, duplicateWrapper.LastName, duplicateWrapper.MiddleName, duplicateWrapper.DateOfBirth, duplicateWrappersCount));
                    }
                }
                if (this.EndorsementType == Constants.RENEWAL_ENDORSEMENT_TYPE)
                {
                    var renewalMemberWrappers = db.RenewalMemberWrappers
                        .Where(t => t.EndorsementBatchGuid == this.Guid).GroupBy(t => new { t.LastName, t.FirstName, t.MiddleName, t.DateOfBirth });
                        
                    var duplicateRenewalWrappers = renewalMemberWrappers.SelectMany(group => group.OrderBy(t => t.LastName).ThenBy(t => t.FirstName)
                            .ThenBy(t => t.MiddleName).ThenBy(t => t.DateOfBirth).Skip(1).Take(1)).ToList();

                    var duplicateRenewalWrappersCount = renewalMemberWrappers.SelectMany(group => group.OrderBy(t => t.LastName).ThenBy(t => t.FirstName)
                            .ThenBy(t => t.MiddleName).ThenBy(t => t.DateOfBirth).Skip(1)).ToList().Count();

                    foreach (var duplicateWrapper in duplicateRenewalWrappers)
                    {
                        yield return new ValidationResult(String.Format("Duplicate entries found for {1}, {0} {2} ({3:MM/dd/yyyy}). Count {4}", duplicateWrapper.FirstName, duplicateWrapper.LastName, duplicateWrapper.MiddleName, duplicateWrapper.DateOfBirth,duplicateRenewalWrappersCount));
                    }
                }
            }

            if (this.Deadline.Date < DateTime.Now.Date && this.Id == 0 && (this.EndorsementType == "New" || this.EndorsementType == "Renewal" || this.EndorsementType == "Cancel Membership"))
            {
                yield return new ValidationResult("Submission Deadline must be a future date.", new List<string>() { "Deadline" });
            }
        }

        #endregion
    }
}
