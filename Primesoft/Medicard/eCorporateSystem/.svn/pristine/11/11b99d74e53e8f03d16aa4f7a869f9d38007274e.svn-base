using Corelib.Enums;
using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Corelib.Models
{
    public class LegacyReimbursement
    {

        [Column("member_code")]
        public string MemberCode { get; set; }

        [Column("control_code")]
        public string ControlCode { get; set; }

        [Column("received_date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReceivedDate { get; set; }

        [Column("due_date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        [Column("visit_date")]
        public string VisitDate { get; set; }

        [Column("reim_status")]
        public string Status { get; set; }

    }
}
