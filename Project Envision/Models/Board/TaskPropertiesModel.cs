using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project_Envision.Models
{
    public class TaskPropertiesModel
    {
        [Required(ErrorMessage = "Required field. *")]
        public string Task_name { get; set; }

        public string Task_Description { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        public int Task_points { get; set; }

    }
}
