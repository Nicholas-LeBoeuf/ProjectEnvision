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
        public static List<int> m_BurndownTaskPoints { get; set; }

        public void setburndowntaskpoints(List<int> getburndown_points)
        {
            m_BurndownTaskPoints = getburndown_points;
        }
        public static List<string> m_Burndowndates { get; set; }


    }
}
