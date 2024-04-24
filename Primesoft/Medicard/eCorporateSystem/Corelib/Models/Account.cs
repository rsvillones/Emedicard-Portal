﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class Account : BaseModel
    {
        #region -- Properties --

        private string _code;
        [Required]
        [StringLength(25)]
        [Index]
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        [StringLength(512)]
        public string Name { get; set; }

        [Display(Name = "Read Only Access")]
        public bool IsReadOnly { get; set; }

        [Display(Name = "User serves as Corporate Administrator")]
        public bool IsCorporateAdmin { get; set; }

        [Display(Name = "User serves as Underwriter")]
        public bool IsUnderWriter { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        #endregion
    }
}
