using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class ApplicationRole : IdentityRole
    {
        #region -- Properties --

        public ICollection<Group> Groups { get; set; }
        
        #endregion
    }
}
