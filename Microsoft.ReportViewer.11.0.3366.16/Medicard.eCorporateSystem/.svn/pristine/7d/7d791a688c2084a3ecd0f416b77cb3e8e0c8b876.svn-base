using System;
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
    public class UtilizationReportException : BaseModel,IValidatableObject
    {
        #region -- Properties --

        [Required(ErrorMessage = "Member Name is required.")]      
        public string MemberCode { get; set; }

        [Required]    
        public string AccountCode { get; set; }

        #endregion

        #region -- IValidatableObject Members --

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                if (db.UtilizationReportExceptions.Any(t => t.MemberCode == MemberCode && t.AccountCode == AccountCode && t.Id != Id && !t.Deleted))
                {
                    yield return new ValidationResult("Member already exist in the database.", new List<string>() { "MemberCode" });
                }
            }
        }

        #endregion
    }
}
