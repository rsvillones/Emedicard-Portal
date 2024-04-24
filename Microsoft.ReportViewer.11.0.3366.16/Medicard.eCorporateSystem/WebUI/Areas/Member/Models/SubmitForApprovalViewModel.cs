using Corelib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Member.Models
{
    public class SubmitForApprovalViewModel : IValidatableObject
    {
        #region -- Properties --

        public Guid MemberGuid { get; set; }

        public bool Accepted { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        #endregion

        #region -- IValidatableObject Members --

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.Accepted)
            {
                yield return new ValidationResult("Please certify that the information that you are submitting are correct and true.", new List<string>() { "Accepted" });
            }

            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Guid == this.MemberGuid);
                if (member == null)
                {
                    yield return new ValidationResult("Invalid Member Information detected");
                }
                else
                {
                    if (!Helper.IsMedicalHistoryComplete(db, member))
                    {
                        yield return new ValidationResult("Please fill up your Medical History.");
                    }

                    if (!Helper.IsDependentMedicalHistoryComplete(db, member))
                    {
                        yield return new ValidationResult("Please fill up Medical History of all your dependents");
                    }
                }
            }
        }

        #endregion
    }
}