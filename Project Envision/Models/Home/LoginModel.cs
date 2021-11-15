using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Project_Envision.Models
{
    public class LoginModel : ModelItems
    {
        [Required(ErrorMessage = " ")]
        [Display(Name = "UserName")]
        public string usernameInput { get => username; set => m_Username = value; }
  
        [Required(ErrorMessage = " ")]
        [DataType(DataType.Password)]
        [Display(Name = "Pass")]
        public string password { get; set; }

        public int idInput { get => id; set => m_UserId = value; }
    }

}
