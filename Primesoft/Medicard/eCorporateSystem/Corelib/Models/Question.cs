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
    public class Question : BaseModel, IValidatableObject
    {
        #region -- Constructor --

        public Question()
        {
            using (var db = new IdentityDataContext())
            {
                if (db.Questions.Any())
                {
                    this.DisplayOrder = db.Questions.Select(t => t.DisplayOrder).Max() + 1;
                }
                else
                {
                    this.DisplayOrder = 1;
                }
            }
        }

        #endregion

        #region -- Properties --

        [StringLength(512)]
        [Required]
        public string Description { get; set; }

        [Display(Name="Display Order")]
        public int DisplayOrder { get; set; }

        public QuestionType Type { get; set; }

        [Display(Name = "Available Answers")]
        public string Options { get; set; }

        public int QuestionGroupId { get; set; }
        [ForeignKey("QuestionGroupId")]
        public QuestionGroup QuestionGroup { get; set; }

        #endregion
        
        #region -- IValidatableObject Members --

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Type == QuestionType.SingleSelect || this.Type == QuestionType.MultipleSelect)
            {
                if (string.IsNullOrEmpty(this.Options))
                {
                    yield return new ValidationResult("Available Answers are required for question type single and multiple selection.", new List<string>() { "Options" });
                }
                else if (!string.IsNullOrEmpty(this.Options) && !this.Options.Contains("|"))
                {
                    yield return new ValidationResult("Available Answers must contain two or more selections. Words must be separated by \"|\"", new List<string>() { "Options" });
                }
                else if (!string.IsNullOrEmpty(this.Options) && this.Options.Contains("|"))
                {
                    foreach (var option in this.Options.Split(new char[] { '|' }))
                    {
                        if (string.IsNullOrEmpty(option))
                        {
                            yield return new ValidationResult("Available Answers contain empty word.", new List<string>() { "Options" });
                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
