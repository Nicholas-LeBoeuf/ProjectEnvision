using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project_Envision.Models
{
    public class BoardSettings
    {
            public string cardColor { get; set; }

            public string textColor { get; set; }

            public string backgroundColor { get; set; }

            public string boardName { get; set; }

            public string boardDescription { get; set; }
       
    }
}
