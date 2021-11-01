using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models.Board
{
    public class GetTaskProperties
    {

        public static string getTask_name { get; set; }
        public void SetgetTaskname(string getTaskName)
        {
            getTask_name = getTaskName;
        }

        public static string getTask_Description { get; set; }
        public void SetTaskDescript(string getTaskDescript)
        {
            getTask_Description = getTaskDescript;
        }

        public static int getTask_points { get; set; }
        public void SetTaskPoints(int getTaskPoints)
        {
            getTask_points = getTaskPoints;
        }

        public static string getAssignee { get; set; }
        public void SetgetAssignee(string Assignee)
        {
            getAssignee = Assignee;
        }

        public static string getusername { get; set; }
        public void Setgetusername(string Getusername)
        {
            getusername = Getusername;
        }

        public static string getlocation { get; set; }
        public void Setlocation(string getLocation)
        {
            getlocation = getLocation;
        }

    }
}
