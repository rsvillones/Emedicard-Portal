using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_UWPRINCIPAL_ACTIVE_MTBL")]
    public class LegacyPrincipalProcess : IMemberProcess
    {
        #region -- Properties --

        [Key]
        [Column("PRIN_APPNUM")]
        public decimal PrinAppNum { get; set; }

        [NotMapped]
        public decimal? DepAppNum { get; set; }

        [Column("PRIN_CODE")]
        public string PrincipalCode { get; set; }

        [NotMapped]
        public string DependentCode { get; set; }

        [Column("MEM_OSTAT_CODE")]
        public int? StatCode { get; set; }

        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; }

        [Column("ALTERED_DATE")]
        public DateTime? AlteredDate { get; set; }

        [Column("DATE_AMEND")]
        public DateTime? AmendedDate { get; set; }

        [NotMapped]
        public string PrincipalOrDependent
        {
            get { return "Principal"; }
        }

        #endregion

        #region -- Additional Properties --

        [Column("ACCOUNT_CODE")]
        public string AccountCode { get; set; }

        [Column("MEM_LNAME")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column("MEM_FNAME")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Column("MEM_MI")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Column("MEM_BDAY")]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Column("MEM_AGE")]
        public Single Age { get; set; }

        [Column("AREA_CODE")]
        public string AreaCode { get; set; }

        [Column("PRIN_COMPID")]
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }

        [Column("RSPROOMRATE_ID")]
        [Display(Name = "Applied Plan")] 
        public int? AppliedPlan { get; set; }

        [Column("MEM_SEX")]
        public int GenderId { get; set; }

        [Column("MEM_CIVILSTAT")]
        public int? CivilStatusId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Column("EFF_DATE")]
        [Display(Name = "Effectivity Date")]
        public DateTime? EffectivityDate { get; set; }

        [Column("VAL_DATE")]
        [Display(Name = "Validity Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ValidityDate { get; set; }

        [Column("DTACARD_PRINTED")]
        public DateTime? DataCardPrintedDate { get; set; }

        [Column("DATE_RESGN")]
        public DateTime? DateResign { get; set; }

        [Column("ID_REM")]
        public string IdRemarkOne { get; set; }

        [Column("ID_REM2")]
        public string IdRemarkTwo { get; set; }
        
        [Column("ID_REM3")]
        public string IdRemarkThree { get; set; }

        [Column("ID_REM4")]
        public string IdRemarkFour { get; set; }

        [Column("ID_REM5")]
        public string IdRemarkFive { get; set; }

        [Column("ID_REM6")]
        public string IdRemarkSix { get; set; }

        [Column("ID_REM7")]
        public string IdRemarkSeven { get; set; }

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
        public string Area
        {
            get
            {
                using (var legacyDb = new LegacyDataContext())
                {
                    if (string.IsNullOrEmpty(this.AreaCode)) return "";
                    var area = legacyDb.LegacyAreas.FirstOrDefault(t => t.AreaCode == this.AreaCode);
                    return area != null ? area.Description : "";
                }
            }
        }

        [NotMapped]
        [Display(Name = "Civil Status")]
        public string CivilStatus
        {
            get
            {
                using (var legacyDb = new LegacyDataContext())
                {
                    var civilStatus = legacyDb.LegacyCivilStatuses.FirstOrDefault(t => t.Id == this.CivilStatusId);
                    return civilStatus != null ? civilStatus.Description : "";
                }
            }
        }

        [NotMapped]
        public string Gender
        {
            get
            {
                return this.GenderId == 1 ? "MALE" : "FEMALE";
            }
        }

        [NotMapped]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        #endregion
    }
}
