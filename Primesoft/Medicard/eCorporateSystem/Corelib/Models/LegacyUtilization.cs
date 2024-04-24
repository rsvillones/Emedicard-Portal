using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class LegacyUtilization
    {
        #region -- Properties --

        [Key]
        [Column("BILLCODE")]
        public string BillCode { get; set; }

        [Column("CONTROL_CODE")]
        public string ControlCode { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Column("AVAIL_FR")]
        public DateTime AvailmentDate { get; set; }

        [Column("DIAG_DESC")]
        public string Diagnosis { get; set; }

        [Column("RX_REM")]
        public string Remarks { get; set; }

        [Column("HOSPITAL_NAME")]
        public string Hospital { get; set; }

        [Column("APPROVED")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public decimal ApprovedAmount { get; set; }

        #endregion
    }
}
