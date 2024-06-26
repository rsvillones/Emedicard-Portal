﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Corelib.Enums;
using System.Web;
using System.IO;

namespace Corelib.Models
{
    public class Dependent : BaseModel,IValidatableObject
    {
        #region -- Constructor --

        public Dependent()
        {
            this.DateOfBirth = DateTime.MinValue;
        }

        #endregion

        #region -- Properties --

        [StringLength(25)]
        [Index]
        public string ControlNumber { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(32)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(32)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(32)]
        public string MiddleName { get; set; }

        [StringLength(8)]
        [Display(Name = "Suffix")]
        public string Suffix { get; set; }

        [Display(Name = "Area")]
        public string Area { get; set; }

        [StringLength(32)]
        [Display(Name = "Relationship")]
        [Required]
        public string Relationship { get; set; }
        [ForeignKey("Relationship")]
        public Relationship RelationshipClass { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date Of Birth")]
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

        [StringLength(32)]
        [Display(Name = "Gender")]
        [Required]
        public string Gender { get; set; }

        [StringLength(32)]
        [Display(Name = "Civil Status")]
        [Required]
        public string CivilStatus { get; set; }

        [StringLength(32)]
        [Display(Name = "Telephone")]
        public string Telephone { get; set; }

        [StringLength(32)]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [StringLength(64)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [StringLength(64)]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [StringLength(32)]
        [Display(Name = "Province")]
        public string Province { get; set; }

        [NotMapped]
        public string ProvinceDescription
        {
            get
            {
                using (var legacyDb = new LegacyDataContext())
                {
                    var legacyProvince = legacyDb.LegacyProvinces.FirstOrDefault(t => t.Code == this.Province) ?? new LegacyProvince();
                    return legacyProvince.Name;
                }
            }
        }

        [StringLength(32)]
        [Display(Name = "City")]
        public string City { get; set; }

        [NotMapped]
        public string CityDescription
        {
            get
            {
                using (var legacyDb = new LegacyDataContext())
                {
                    var legacyCity = legacyDb.LegacyCities.FirstOrDefault(t => t.Code == this.City) ?? new LegacyCity();
                    return legacyCity.Name;
                }
            }
        }

        [Display(Name = "Zip")]
        public string Zip { get; set; }

        public int? HeightFeet { get; set; }

        public int? HeightInches { get; set; }

        [StringLength(32)]
        [Display(Name = "Occupation")]
        public string Occupation { get; set; }

        [Display(Name = "Weight")]
        public int? Weight { get; set; }

        [Display(Name = "Applied Plan")]
        public int AppliedPlan { get; set; }
        
        [NotMapped]
        public string AppliedPlanDescription
        {
            get
            {
                using (var legacyDb = new LegacyDataContext())
                {
                    var roomRate = legacyDb.LegacyRoomRates.Include(t => t.LegacyPlan).FirstOrDefault(t => t.Id == this.AppliedPlan) ?? new LegacyRoomRate() { LegacyPlan = new LegacyPlan() };
                    return roomRate.LongDescription;
                }
            }
        }

        [Display(Name = "Optional Plan")]
        public int? OptionalPlan { get; set; }

        [NotMapped]
        public string OptionalPlanDescription
        {
            get
            {
                if (!this.OptionalPlan.HasValue) return null;

                using (var legacyDb = new LegacyDataContext())
                {
                    var roomRate = legacyDb.LegacyRoomRates.Include(t => t.LegacyPlan).FirstOrDefault(t => t.Id == this.OptionalPlan.Value) ?? new LegacyRoomRate() { LegacyPlan = new LegacyPlan() };
                    return roomRate.LongDescription;
                }
            }
        }

        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        public ICollection<DependentMedicalHistory> MedicalHistories { get; set; }

        public Corelib.Enums.MembershipStatus Status { get; set; }

        [StringLength(25)]
        [Index]
        public string Code { get; set; }

        [Index]
        public int? DepAppNum { get; set; }

        public DateTime? EffectivityDate { get; set; }

        public DateTime? ValidityDate { get; set; }

        [Display(Name = "Member Type")]
        public MemberType MemberType { get; set; }

        public DependentType DependentType { get; set; }

        public byte[] DocumentFile { get; set; }

        public string DocumentContentType { get; set; }

        public string DocumentFileName { get; set; }

        [NotMapped]
        public HttpPostedFileBase FileWrapper { get; set; }

        #endregion

        #region -- Not Mapped --

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
        public string Address
        {
            get
            {
                return string.Format("{0}, {1}, {2}, {3}.", Street, City, Province, Zip);
            }
        }

        [NotMapped]
        public string Position
        {
            get
            {
                return Occupation;
            }
        }

        #endregion

        #region -- IValidatableObject --

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                if (db.Dependents.Any(t => (t.DateOfBirth == this.DateOfBirth && t.LastName.ToLower() == this.LastName.ToLower() && t.FirstName.ToLower() == this.FirstName.ToLower() &&
                    ((!string.IsNullOrEmpty(t.MiddleName) && !string.IsNullOrEmpty(this.MiddleName) && t.MiddleName.ToLower() == this.MiddleName.ToLower()) ||
                    (string.IsNullOrEmpty(t.MiddleName) && string.IsNullOrEmpty(this.MiddleName)))
                    && t.Id != this.Id && t.MemberId == MemberId) && !t.Deleted))
                {
                    yield return new ValidationResult("Dependent already exist in database.");
                }
                if (this.DateOfBirth > DateTime.Now)
                {
                    yield return new ValidationResult("Date of Birth should not be a future date.", new List<string>() { "DateOfBirth" });
                }
            }
            if (this.FileWrapper != null)
            {
                var allowedExtensions = new[] { ".doc", ".docx", ".jpeg", ".pdf", ".png", ".jpg" };
                var extension = Path.GetExtension(this.FileWrapper.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    yield return new ValidationResult("Invalid file extension.", new List<string>() { "FileWrapper" });
                }
            }
        }

        #endregion
    }
}
