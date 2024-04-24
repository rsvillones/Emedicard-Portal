using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_AREA_LTBL")]
    public class LegacyArea
    {
        #region -- Properties --

        [Column("AREA_CODE")]
        [Key]
        public string AreaCode { get; set; }

        [Column("AREA_DESC")]
        public string Description { get; set; }

        [Column("ACCOUNT_CODE")]
        public string AccountCode { get; set; }

        #endregion
    }
}
