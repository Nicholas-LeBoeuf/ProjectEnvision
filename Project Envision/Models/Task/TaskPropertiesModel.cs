using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Project_Envision.Models.Board;

namespace Project_Envision.Models
{
    public class TaskPropertiesModel: GetTaskProperties
    {

        [Required(ErrorMessage = "Required field. *")]
        public string task_Name { get; set; }

        public string task_Description { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        public int task_Points { get; set; }

        public string assignee { get; set; }

        public string username { get; set; }

        public string location { get; set; }

    }
}
