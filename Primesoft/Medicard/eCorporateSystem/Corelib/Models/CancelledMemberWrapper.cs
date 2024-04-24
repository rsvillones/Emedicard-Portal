using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class CancelledMemberWrapper : BaseModel
    {
        
        #region -- Properties --

        public string MemberCode { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
          
        public string EndorsementBatchId { get; set; }

        public string DateCancelled { get; set; }

        public string AccountCode { get; set; }

        #endregion
    }
}
