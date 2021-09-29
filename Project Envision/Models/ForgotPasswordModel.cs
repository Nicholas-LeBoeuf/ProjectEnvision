using System.ComponentModel.DataAnnotations;
namespace Project_Envision.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Username:")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Email:")]
        [EmailAddress]
        [StringLength(32, MinimumLength = 0)]
        public string Email { get; set; }

        [Display(Name = "Security Code:")]
        public string SecurityCode { get; set; }
    }
}