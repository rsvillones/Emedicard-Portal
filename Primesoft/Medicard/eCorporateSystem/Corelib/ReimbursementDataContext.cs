using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corelib.Models;

namespace Corelib
{
    public class ReimbursementDataContext  : DbContext
    {
        
        #region -- Constructor --

        public ReimbursementDataContext()
            : base("name=Medicard.ReimbursementSystem")
        {

        }

        #endregion

        #region -- DbSets --        

        #endregion
    }
}
