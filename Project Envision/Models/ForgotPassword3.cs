using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class ForgotPassword3
    {
        [Required(ErrorMessage = "Required field. *")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        [StringLength(20, MinimumLength = 0)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required. *")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        [StringLength(20, MinimumLength = 0)]
        [Compare("Password", ErrorMessage = "Passwords are not the same. *")]
        public string confirmPassword { get; set; }
    }
}
