using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class QuestionGroup : BaseModel
    {
        #region -- Constructor --

        public QuestionGroup()
        {
            using (var db = new IdentityDataContext())
            {
                if (db.QuestionGroups.Any())
                {
                    this.DisplayOrder = db.QuestionGroups.Select(t => t.DisplayOrder).Max() + 1;
                }
                else
                {
                    this.DisplayOrder = 1;
                }
            }
        }

        #endregion

        #region -- Properties --

        [Required]
        [StringLength(256)]
        public string Question { get; set; }

        public int DisplayOrder { get; set; }

        public ICollection<Question> Questions { get; set; }

        #endregion
    }
}
