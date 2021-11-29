using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Project_Envision.Models.Board;

namespace Project_Envision.Models
{
    public class SprintPropertiesModel
    {
        [Required(ErrorMessage = "Required field. *")]
        public string sprint_Name { get; set; }

        public string sprint_Description { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        public DateTime start_Time { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        public DateTime end_Time { get; set; }


    }
}
