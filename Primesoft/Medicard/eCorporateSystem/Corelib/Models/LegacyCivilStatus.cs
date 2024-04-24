using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_UWMEMBER_CSTATUS_LTBL")]
    public class LegacyCivilStatus
    {
        #region -- Properties --

        [Column("MEMCSTAT_ID")]
        [Key]
        public int Id { get; set; }

        [Column("MEMCSTAT_DESC")]
        public string Description { get; set; }

        #endregion
    }
}
