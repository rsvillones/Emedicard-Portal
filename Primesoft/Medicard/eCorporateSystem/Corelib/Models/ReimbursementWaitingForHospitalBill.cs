using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class ReimbursementWaitingForHospitalBill
    {
        #region -- Properties --

        [Column("box_1")]
        public bool BoxOne { get; set; }

        [Column("box_2")]
        public bool BoxTwo { get; set; }

        [Column("box_3")]
        public bool BoxThree { get; set; }

        [Column("hospital_name")]
        [Display(Name = "Hospital Name")]
        public string HospitalName { get; set; }

        [Column("box_2_txt")]
        public string BoxTwoRemark { get; set; }

        [Column("loa_no")]
        [Display(Name = "LOA Number")]
        public string LoaNumber { get; set; }
        
        [Column("lacking_date")]
        [Display(Name = "Final Memo Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LackingDate { get; set; }

        [Column("memo_released_date")]
        [Display(Name = "Memo Released Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleasedDate { get; set; }

        #endregion
    }
}
