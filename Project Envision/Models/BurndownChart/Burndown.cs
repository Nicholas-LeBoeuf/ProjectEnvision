using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{ 
    public class Burndown 
    {
        public int Date { get; set; }
        public double StoryPoints { get; set; }
        
        public static List<double> m_BurndownTaskPoints { get; set; }
        public void setBurndownTaskPoints(List<double> getBurndownTaskPoints)
        {
            m_BurndownTaskPoints = getBurndownTaskPoints;
        }

        public static List<DateTime> m_BurndownDates { get; set; }
        public void setBurndownDates(List<DateTime> getDates)
        {
            m_BurndownDates = getDates;
        }

        public static List<double> m_DataTaskPoints { get; set; }
        public void setGraphTaskPoints(List<double> getTaskPoints)
        {
            m_DataTaskPoints = getTaskPoints;
        }

        public static List<DateTime> m_GraphDates { get; set; }
        public void setGraphDates(List<DateTime> getDates)
        {
            m_GraphDates = getDates;
        }

        public static int TaskTotal { get; set; }

        public static string sprintStartTime { get; set; }
        public void setSprintStartTime(string sprintStartTimeInput)
        {
            sprintStartTime = sprintStartTimeInput;
        }

        public static String sprintEndTime { get; set; }

        public void setSprintEndTime(string sprintEndTimeInput)
        {
            sprintEndTime = sprintEndTimeInput;
        }
    }

   


}
