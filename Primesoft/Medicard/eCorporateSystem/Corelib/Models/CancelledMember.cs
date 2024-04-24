using Corelib.Enums;
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
    public class CancelledMember : BaseModel, IValidatableObject
    {
        #region -- Constructors --

        public CancelledMember()
        {
            this.DateCancelled = DateTime.Now;
        }

        #endregion

        #region -- Properties --

        [StringLength(25)]
        [Index]
        public string ControlNumber { get; set; }

        [StringLength(25)]
        [Index]
        [Required]
        public string MemberCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCancelled { get; set; }

        public CancelledMembershipStatus Status { get; set; }

        [ForeignKey("EndorsementBatch")]
        public int EndorsementBatchId { get; set; }
        public EndorsementBatch EndorsementBatch { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string AccountCode { get; set; }

        #endregion

        #region -- Not Mapped Properties --

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(MemberCode)) return "";
                var fullName = string.Format("{0}, {1}", LastName, FirstName); ;
                if (!string.IsNullOrEmpty(MiddleName))
                {
                    fullName = string.Format("{0}, {1} {2}", LastName, FirstName, MiddleName.Substring(0, 1).ToUpper());
                }
                return fullName;
            }
        }

        #endregion

        #region -- IValidatableObject Members --

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                using (var legacyDb = new LegacyDataContext())
                {
                    if (db.CancelledMembers.Any(t => t.MemberCode == MemberCode && t.Status != CancelledMembershipStatus.Cancelled && t.Id != Id && !t.Deleted))
                    {
                        yield return new ValidationResult("Member is currently for membership cancellation.", new List<string>() { "MemberCode" });
                    }
                    else if (this.Id == 0 && !legacyDb.LegacyPrincipalProcesses.Any(t => t.PrincipalCode == this.MemberCode &&
                        t.FirstName.ToLower() == this.FirstName.ToLower() && t.LastName.ToLower() == this.LastName.ToLower()
                        && t.AccountCode == this.AccountCode && Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode) && t.ValidityDate >= DateTime.Now))
                    {
                        yield return new ValidationResult("Member does not have an active subscription", new List<string>() { "MemberCode" });
                    }
                }
            }
        }

        #endregion
    }
}
