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
    public class RenewalMember : BaseModel, IValidatableObject
    {
        #region -- Properties --

        public int Row { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DisplayFormat(DataFormatString = BaseModel.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Age")]
        [NotMapped]
        public int? Age
        {
            get
            {
                if (this.DateOfBirth == DateTime.MinValue) return null;

                DateTime today = DateTime.Today;
                int age = today.Year - this.DateOfBirth.Year;
                if (this.DateOfBirth > today.AddYears(-age)) age--;

                return age;
            }
        }

        public string Area { get; set; }

        public string EmployeeNumber { get; set; }

        public int AppliedPlan { get; set; }

        public string AllowedPlans { get; set; }

        [Required]
        public string Type { get; set; }

        public string PrincipalMemberCode { get; set; }

        public string Relationship { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string CivilStatus { get; set; }

        public string Waiver { get; set; }

        [DisplayFormat(DataFormatString = BaseModel.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime? EffectivityDate { get; set; }

        [DisplayFormat(DataFormatString = BaseModel.DateFormat, ApplyFormatInEditMode = true)]
        public DateTime? ValidityDate { get; set; }

        public string Remarks { get; set; }

        [ForeignKey("EndorsementBatch")]
        public int EndorsementBatchId { get; set; }
        public EndorsementBatch EndorsementBatch { get; set; }

        [Required]
        [StringLength(25)]
        public string AccountCode { get; set; }

        [Index]
        [StringLength(25)]
        public string ControlNumber { get; set; }

        public string AppliedPlanFromExcel { get; set; }

        public string AllowedPlansFromExcel { get; set; }

        public string RelationshipExcel { get; set; }

        public string AreaExcel { get; set; }

        [StringLength(32)]
        public string EndorsementType { get; set; }

        public string DependentStringValue { get; set; }

        public RenewalStatus RenewalStatus { get; set; }

        #endregion

        #region -- Not Mapped Properties --

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                var fullName = string.Format("{0}, {1}", LastName, FirstName); ;
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    fullName = string.Format("{0}, {1} {2}", LastName, FirstName, MiddleName.Substring(0, 1).ToUpper());
                }
                return fullName;
            }
        }

        [NotMapped]
        public IEnumerable<LegacyRoomRate> LegacyRoomRates { get; set; }

        [NotMapped]
        public IEnumerable<LegacyPrincipalProcess> LegacyPrincipalProcesses { get; set; }

        [NotMapped]
        public IEnumerable<LegacyDependentProcess> LegacyDependentProcesses { get; set; }

        #endregion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Type == "Principal" && this.RenewalStatus == Enums.RenewalStatus.ForRenewal)
            {
                //if (this.LegacyPrincipalProcesses.ToList().Count > 0 && !this.LegacyPrincipalProcesses
                //    .Any(t => t.PrincipalCode == this.Code &&
                //        t.FirstName.ToLower() == this.FirstName.ToLower() && t.LastName.ToLower() == this.LastName.ToLower() && t.DateOfBirth == this.DateOfBirth &&
                //        ((!string.IsNullOrEmpty(t.MiddleName) && !string.IsNullOrEmpty(this.MiddleName) && t.MiddleName.ToLower() == this.MiddleName.ToLower()) || (string.IsNullOrEmpty(t.MiddleName) && string.IsNullOrEmpty(this.MiddleName)))
                //        ))
                //{
                //    yield return new ValidationResult("Member does not have an Active Subscription");
                //}
                if (this.LegacyRoomRates.ToList().Count > 0 && !this.LegacyRoomRates.Any(t => t.Id == this.AppliedPlan &&
                    (t.LegacyPaymode.Id == 0 || t.LegacyPaymode.Id == 5 || t.LegacyPaymode.Id == 8)))
                {
                    yield return new ValidationResult("Invalid applied plan for Principal", new Collection<string>() { "AppliedPlan" });
                }
                if (!string.IsNullOrEmpty(this.AllowedPlans))
                {
                    foreach (var allowedId in this.AllowedPlans.Split(',').ToArray())
                    {
                        if (this.LegacyRoomRates.ToList().Count > 0 && !this.LegacyRoomRates.Any(t => t.Id.ToString() == allowedId &&
                            (t.LegacyPaymode.Id == 0 || t.LegacyPaymode.Id == 5 || t.LegacyPaymode.Id == 8)))
                        {
                            yield return new ValidationResult("Invalid optional plan for Principal", new Collection<string>() { "AllowedPlans" });
                        }
                    }
                }
                using (var db = new IdentityDataContext())
                {
                    if (this.Id == 0 && db.Members.Any(t => t.Code == this.Code && t.Id != this.Id &&
                                t.Status != MembershipStatus.Approved && t.Status != MembershipStatus.Disapproved &&
                                t.Status != MembershipStatus.Cancelled && t.Status != MembershipStatus.PermanentResigned))
                    {
                        yield return new ValidationResult("Member is currently for renewal");
                    }
                }
            }
            else if (this.Type == "Dependent" && this.RenewalStatus == Enums.RenewalStatus.ForRenewal)
            {
                //if (this.LegacyDependentProcesses.ToList().Count > 0 &&
                //    !this.LegacyDependentProcesses
                //    .Any(t => t.DependentCode == this.Code &&
                //        t.FirstName.ToLower() == this.FirstName.ToLower() && t.LastName.ToLower() == this.LastName.ToLower() && t.DateOfBirth.Value.Date == this.DateOfBirth.Date &&
                //        ((!string.IsNullOrEmpty(t.MiddleName) && !string.IsNullOrEmpty(this.MiddleName) && t.MiddleName.ToLower() == this.MiddleName.ToLower()) || (string.IsNullOrEmpty(t.MiddleName) && string.IsNullOrEmpty(this.MiddleName)))
                //        ))
                
                //{
                //    yield return new ValidationResult("Member does not have an Active Subscription");
                //}
                if (this.LegacyRoomRates.ToList().Count > 0 && !this.LegacyRoomRates.Any(t => t.Id == this.AppliedPlan &&
                    (t.LegacyPaymode.Id == 1 || t.LegacyPaymode.Id == 2 || t.LegacyPaymode.Id == 5 || t.LegacyPaymode.Id == 8 || t.LegacyPaymode.Id == 9)))
                {
                    yield return new ValidationResult("Invalid applied plan for Dependent", new Collection<string>() { "AppliedPlan" });
                }
                if (!string.IsNullOrEmpty(this.AllowedPlans))
                {
                    foreach (var allowedId in this.AllowedPlans.Split(',').ToArray())
                    {
                        if (this.LegacyRoomRates.ToList().Count > 0 && !this.LegacyRoomRates.Any(t => t.Id.ToString() == allowedId &&
                            (t.LegacyPaymode.Id == 1 || t.LegacyPaymode.Id == 2 || t.LegacyPaymode.Id == 5 || t.LegacyPaymode.Id == 8 || t.LegacyPaymode.Id == 9)))
                        {
                            yield return new ValidationResult("Invalid optional plan for Dependent", new Collection<string>() { "AllowedPlans" });
                        }
                    }
                } 
                using (var db = new IdentityDataContext())
                {
                    if (this.Id == 0 && this.RenewalStatus == Enums.RenewalStatus.ForRenewal && db.Dependents.Any(t => t.Code == this.Code && t.Id != this.Id &&
                                t.Status != MembershipStatus.Approved && t.Status != MembershipStatus.Disapproved &&
                                t.Status != MembershipStatus.Cancelled && t.Status != MembershipStatus.PermanentResigned))
                    {
                        yield return new ValidationResult("Member is currently for renewal");
                    }
                }
                if (string.IsNullOrEmpty(this.Relationship))
                {
                    yield return new ValidationResult("Relationship field is required.", new Collection<string>() { "Relationship" });
                }
            }

            using (var db = new IdentityDataContext())
            {
                var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == this.AccountCode) ?? new AccountSetting();
                if (accountSetting.UseEmailAsLogin && String.IsNullOrEmpty(this.EmailAddress))
                {
                    yield return new ValidationResult("Email address is required to be used for login.", new Collection<string>() { "EmailAddress" });
                }
                if (!String.IsNullOrEmpty(accountSetting.DomainEmail))
                {
                    if (!String.IsNullOrEmpty(this.EmailAddress) && this.EmailAddress.Substring(this.EmailAddress.IndexOf("@") + 1) != accountSetting.DomainEmail)
                    {
                        yield return new ValidationResult(String.Format("Invalid Email. Only @{0} email accounts are valid.", accountSetting.DomainEmail), new Collection<string>() { "EmailAddress" });
                    }
                }
                if (this.DateOfBirth > DateTime.Now)
                {
                    yield return new ValidationResult("Date of Birth should not be a future date.", new List<string>() { "DateOfBirth" });
                }
            }

            if (string.IsNullOrEmpty(this.Area) && LegacyHelper.GetLegacyArea(this.AccountCode).Count() > 0)
            {
                yield return new ValidationResult("Area field is Required.", new List<string>() { "Area" });
            }
        }
    }
}