using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class DeclinedIdReplacement : BaseModel
    {
        #region -- Properties --
        
        [Required]
        public string Remarks { get; set; }

        public Guid EndorsementBatchGuid { get; set; }

        #endregion
    }
}
