using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_PAYMODE_LTBL")]
    public class LegacyPaymode
    {
        [Column("PREMIUM_PAYMENT_FOR")]
        public int Id { get; set; }

        [Column("PAYMODE_DESC")]
        public string Description { get; set; }
    }
}
