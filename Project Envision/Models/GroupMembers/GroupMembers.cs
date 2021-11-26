using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project_Envision.Models
{
    public class GroupMembers
    {
        public string removeUsername { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Username:")]
        [StringLength(20, MinimumLength = 0)]
        public string username { get; set; }


    }
}
