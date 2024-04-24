using Corelib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class DocumentType : BaseModel,IValidatableObject
    {
        #region -- Properties --

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public DocumentClassification Classification { get; set; }

        #endregion

        #region -- IValidatableObject Members --

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                if (db.DocumentTypes.Any(t => !t.Deleted && t.Name == this.Name && t.Classification == this.Classification && t.Id != this.Id))
                {
                    yield return new ValidationResult("Document type already exists in the database");
                }
            }
        }

        #endregion

    }
}
