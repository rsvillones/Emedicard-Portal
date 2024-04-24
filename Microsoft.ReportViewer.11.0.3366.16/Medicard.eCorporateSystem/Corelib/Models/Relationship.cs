using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class Relationship
    {
        #region -- Properties --

        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(40)]
        public string Description { get; set; }

        public int Gender { get; set; }

        #endregion
    }
}
