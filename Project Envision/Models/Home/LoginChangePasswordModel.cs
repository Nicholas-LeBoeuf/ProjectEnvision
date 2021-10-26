using System.ComponentModel.DataAnnotations;
namespace Project_Envision.Models
{
    public class LoginChangePasswordModel
    {                    
        [Required(ErrorMessage = "Required field. *")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password:")]
        [StringLength(32, MinimumLength = 0)]
        public string new_Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required. *")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        [StringLength(32, MinimumLength = 0)]
        [Compare("NewPassword", ErrorMessage = "Passwords are not the same. *")]
        public string confirm_Password { get; set; }      
    
        public string username { get; set; }
    }
}