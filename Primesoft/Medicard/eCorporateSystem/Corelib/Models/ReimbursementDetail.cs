using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class ReimbursementDetail
    {
        #region -- Properties --

        [Column("total_approved")]
        [Display(Name = "Approved Amount")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}", ApplyFormatInEditMode = true)]
        public decimal TotalApproved { get; set; }

        [Column("check_no")]
        [Display(Name = "Check Number")]
        public string CheckNumber { get; set; }

        [Column("check_date")]
        [Display(Name = "Check Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckDate { get; set; }

        [Column("rmd_hold_rem")]
        public string RmdHoldRemark { get; set; }

        [Column("for_release_date")]
        [Display(Name = "Ready for Release")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ForReleaseDate { get; set; }

        [Column("check_released_date")]
        [Display(Name = "Released Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CheckReleasedDate { get; set; }

        #endregion
    }
}
