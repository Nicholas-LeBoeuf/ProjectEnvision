using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

using Newtonsoft.Json;


namespace Project_Envision.Controllers
{
    public class BurndownController : Controller
    {
        public IActionResult BurndownChart()
        {
            return View();
        }

        public object GetBurndownValues()
        {

            List<Burndown> vals = new List<Burndown>();

            for (int i = 10; i >= 1; i--)
            {
                vals.Add(new Burndown { Date = i, StoryPoints = i });
            }

            IEnumerable<Burndown> BurndownList = vals;

            return BurndownList;
        }
    }
}

