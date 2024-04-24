using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_PROVINCE_LTBL")]
    public class LegacyProvince
    {
        #region -- Properties --

        [Key]
        [Column("PROVINCE_CODE")]
        public string Code { get; set; }

        [Column("PROVINCE_NAME")]
        public string Name { get; set; }

        #endregion
    }
}
