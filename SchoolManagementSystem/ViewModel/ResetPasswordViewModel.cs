using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagementSystem.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; } // Encrypted User Id

        [Required(ErrorMessage = "Please enter new password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please re-enter new password")]
        [Compare("Password", ErrorMessage = "Password's doesn't matched!")]
        public string ConfirmPassword { get; set; }
    }
}