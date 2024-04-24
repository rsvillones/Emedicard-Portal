using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_SOB_INFORMATION_MTBL")]
    public class LegacySob
    {
        #region -- Properties --

        [Key]
        [Column("ACCOUNT_CODE")]
        public string AccountCode { get; set; }

        [Column("EFFECTIVITY_DATE")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EffectivityDate { get; set; }

        [Column("VALIDITY_DATE")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ValidityDate { get; set; }

        #endregion
    }
}
