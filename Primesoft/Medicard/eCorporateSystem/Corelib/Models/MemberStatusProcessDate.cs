using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class MemberStatusProcessDate : BaseModel
    {
        #region -- Properties --

        [Index]
        public DateTime Date { get; set; }

        #endregion
    }
}
