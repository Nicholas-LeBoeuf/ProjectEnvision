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
        public List<int> SprintIds { get; set; }

        public List<string> SprintNames { get; set; }

        public List<int> SprintPoints { get; set; }

        public List<string> SprintDates { get; set; }


    }
}
