﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class Group : BaseModel
    {
        #region -- Constructor --

        public Group()
        {
            this.Roles = new Collection<ApplicationRole>();
            this.Users = new Collection<ApplicationUser>();
            this.AccessibleGroups = new Collection<AccessibleGroup>();
        }

        #endregion

        #region -- Properties --

        [StringLength(64)]
        [Required]
        public string Name { get; set; }

        [Display(Name="Permissions")]
        public ICollection<ApplicationRole> Roles { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        [Display(Name = "Groups that can be assigned when creating a user")]
        public ICollection<AccessibleGroup> AccessibleGroups { get; set; }

        public bool IsSystemGroup { get; set; }

        [NotMapped]
        public string RolesDescription
        {
            get
            {
                var returnValue = new StringBuilder();

                if(this.Roles != null)
                {
                    foreach(var role in this.Roles)
                    {
                        if(returnValue.Length > 0) returnValue.Append(", ");

                        returnValue.Append(role.Name);
                    }
                }

                return returnValue.ToString();
            }
        }

        #endregion
    }
}
