using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class UtilizationMemberAllService
    {
        #region -- Properties --

        [Column("CONTROL_CODE")]
        public string ControlNumber { get; set; }

        [Column("AVAIL_FR")]
        public DateTime? AvailmentFrom { get; set; }

        [Column("DIAG_DESC")]
        public string DiagnosisDescription { get; set; }

        [Column("DX_REM")]
        public string DiagnosisRemarks { get; set; }

        [Column("HOSPITAL_NAME")]
        public string HospitalName { get; set; }

        [Column("APPROVED")]
        public decimal Approved { get; set; }

        [Column("DISAPPROVED")]
        public decimal Disapproved { get; set; }

        [Column("ADVANCES")]
        public decimal Advances { get; set; }
        
        [Column("ERC")]
        public decimal Erc { get; set; }

        [Column("MEMCODE")]
        public string MemberCode { get; set; }

        [Column("REMARKS2")]
        public string Remarks2 { get; set; }
        
        [Column("HOSP_SOA")]
        public decimal HospitalSoa { get; set; }
        
        #endregion
    }
}
