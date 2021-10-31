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
        [Required(ErrorMessage = "*")]
        [Display(Name = "UserName")]
        public string Username { get => username; set => m_username = value; }
  
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Pass")]
        public string password { get; set; }

        public int id { get => Id; set => m_userid = value; }
    }

}
