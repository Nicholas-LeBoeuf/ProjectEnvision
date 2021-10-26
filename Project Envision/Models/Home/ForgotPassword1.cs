using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class ForgotPassword1
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Username:")]
        public string username { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email:")]
        [EmailAddress]
        public string email { get; set; }

    }
}
