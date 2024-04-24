using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class Reason : BaseModel,IValidatableObject
    {
        #region -- Properties --

        [Required]
        [StringLength(64)]
        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        #endregion

        #region -- IValidatableObject Members --

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                if (db.Reasons.Any(t => !t.Deleted && t.Description == Description && t.Id != Id))
                {
                    yield return new ValidationResult("Reason already exists in the database");
                }
            }
        }

        #endregion
    }
}
