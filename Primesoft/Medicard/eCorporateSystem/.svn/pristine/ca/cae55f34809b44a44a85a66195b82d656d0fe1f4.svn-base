using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class ReimbursementDisapproved
    {
        #region -- Properties --

        [Column("memo_txt")]
        public string Remark { get; set; }

        [Column("denied_date")]
        [Display(Name = "Final Memo Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DeniedDate { get; set; }

        [Column("memo_released_date")]
        [Display(Name = "Memo Released Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleasedDate { get; set; }

        #endregion
    }
}
