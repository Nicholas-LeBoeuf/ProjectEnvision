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
            return Models.SampleData.BurndownList;
        }
    }
}

