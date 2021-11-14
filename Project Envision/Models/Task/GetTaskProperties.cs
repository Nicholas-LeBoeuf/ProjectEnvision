using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models.Board
{
    public class GetTaskProperties
    {

        public static string getTask_Name { get; set; }
        public void setGetTaskName(string getTaskName)
        {
            getTask_Name = getTaskName;
        }

        public static string getTask_Description { get; set; }
        public void setTaskDescript(string getTaskDescript)
        {
            getTask_Description = getTaskDescript;
        }

        public static int getTask_Points { get; set; }
        public void setTaskPoints(int getTaskPoints)
        {
            getTask_Points = getTaskPoints;
        }

        public static string getAssignee { get; set; }
        public void setGetAssignee(string assignee)
        {
            getAssignee = assignee;
        }

        public static string getUsername { get; set; }
        public void setGetUsername(string Username)
        {
            getUsername = Username;
        }

        public static string getLocation { get; set; }
        public void setLocation(string location)
        {
            getLocation = location;
        }

    }
}
