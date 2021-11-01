using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models.Sprint
{
    public class GetSprintProperties
    {
        public static string getSprint_name { get; set; }
        public void SetgetSprintname(string getSprintName)
        {
            getSprint_name = getSprintName;
        }

        public static string getSprint_Description { get; set; }
        public void SetSprintDescript(string getSprintDescript)
        {
            getSprint_Description = getSprintDescript;
        }

        public static DateTime getSprint_Start { get; set; }
        public void SetSprintStart(DateTime getSprintStart)
        {
            getSprint_Start = getSprintStart;
        }

        public static DateTime getSprint_End { get; set; }
        public void SetSprintEnd(DateTime getSprintEnd)
        {
            getSprint_End = getSprintEnd;
        }

    }
}
