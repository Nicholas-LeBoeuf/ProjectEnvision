using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models.Sprint
{
    public class GetSprintProperties
    {
        public static string getSprint_Name { get; set; }
        public void setGetSprintName(string getSprintName)
        {
            getSprint_Name = getSprintName;
        }

        public static string getSprint_Description { get; set; }
        public void setSprintDescript(string getSprintDescript)
        {
            getSprint_Description = getSprintDescript;
        }

        public static DateTime getSprint_Start { get; set; }
        public void setSprintStart(DateTime getSprintStart)
        {
            getSprint_Start = getSprintStart;
        }

        public static DateTime getSprint_End { get; set; }
        public void setSprintEnd(DateTime getSprintEnd)
        {
            getSprint_End = getSprintEnd;
        }

    }
}
