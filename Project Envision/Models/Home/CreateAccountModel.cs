using System.ComponentModel.DataAnnotations;
namespace Project_Envision.Models
{
    public class CreateAccountModel
    {
        [StringLength(20, MinimumLength = 0)]
        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "First Name:")]
        
        public string firstName { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Last Name:")]
        [StringLength(20, MinimumLength = 0)]
        public string lastName { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Username:")]
        [StringLength(20, MinimumLength = 0)]
        public string username { get; set; }


        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Email Address:")]
        [EmailAddress]
        [StringLength(30, MinimumLength = 0)]
        public string email { get; set; }

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