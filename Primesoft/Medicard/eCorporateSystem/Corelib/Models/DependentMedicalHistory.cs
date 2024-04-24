using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class DependentMedicalHistory : BaseModel, IMedicalHistory
    {
        #region -- Properties --

        public bool? Answer { get; set; }

        [StringLength(1024)]
        public string Remarks { get; set; }

        [StringLength(1024)]
        public string SelectedOptions { get; set; }

        [NotMapped]
        public IEnumerable<string> SelectedOptionsList
        {
            get
            {
                if (!String.IsNullOrEmpty(this.SelectedOptions))
                {
                    return this.SelectedOptions.Split('|');
                }
                else
                {
                    return new List<string>();
                }

            }
            set
            {
                var temp = "";
                foreach (var val in value)
                {
                    if (temp != "") temp += "|";
                    temp += val;
                }
                this.SelectedOptions = temp;
            }
        }


        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        public int DependentId { get; set; }
        [ForeignKey("DependentId")]
        public Dependent Dependent { get; set; }

        #endregion
    }
}
