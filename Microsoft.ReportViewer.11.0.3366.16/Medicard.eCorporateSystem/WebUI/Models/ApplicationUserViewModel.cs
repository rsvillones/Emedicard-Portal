﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corelib.Models;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class ApplicationUserViewModel : ApplicationUser, IValidatableObject
    {
        #region -- Properties --

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool AgentCodeValidated { get; set; }

        #endregion
    }
}