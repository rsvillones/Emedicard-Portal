using System;
using Corelib.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class UtilizationReportExceptionForUser : BaseModel,IValidatableObject
    {
        #region -- Properties --

        [Required(ErrorMessage = "Member Name is required.")]      
        public string MemberCode { get; set; }

        [Required(ErrorMessage = "Users are required.")]
        public string UserIds { get; set; }

        [NotMapped]
        public string UserIdsForSelectPicker
        {
            get
            {
                var returnValue = new StringBuilder();
                if (UserIds != null)
                {
                    foreach (var userId in UserIds.Split(','))
                    {
                        if (returnValue.Length > 0) returnValue.Append(',');
                        returnValue.Append(String.Format("'{0}'", userId));
                    }
                }
                return returnValue.ToString();
            }
        }

        [Required]    
        public string AccountCode { get; set; }

        public string MemberName { get; set; }

        #endregion

        #region -- IValidatableObject Members --

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                if (db.UtilizationReportExceptionForUsers.Any(t => t.MemberCode == MemberCode && t.AccountCode == AccountCode && t.Id != Id && !t.Deleted))
                {
                    yield return new ValidationResult("Member already exist in the database.", new List<string>() { "MemberCode" });
                }
            }
        }

        #endregion
    }
}
