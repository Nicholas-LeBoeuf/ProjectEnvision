using System.ComponentModel.DataAnnotations;
namespace Project_Envision.Models
{
    public class ForgotPassword2
    {

        [Display(Name = "Security Code:")]
        [StringLength(6, MinimumLength = 0)]
        public string SecurityCode { get; set; }
    }
}