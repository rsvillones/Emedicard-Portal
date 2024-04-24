using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corelib.Enums;

namespace Corelib.Models
{
    public class EndorsementLog : BaseModel
    {
        #region -- Constructors --

        public EndorsementLog()
        {
            this.DateProcessed = DateTime.Now;
        }

        #endregion

        #region -- Properties --

        public int NumberOfData { get; set; }

        public int CurrentData { get; set; }

        public DateTime DateProcessed { get; set; }

        public bool IsProcessed { get; set; }

        public string UserName { get; set; }

        public string Remark { get; set; }

        public Guid EndorsementGuid { get; set; }

        public LogType Type { get; set; }

        public Guid FileGuid { get; set; }

        public byte[] FileByte { get; set; }

        public string AccoutCode { get; set; }
        
        #endregion
    }
}
