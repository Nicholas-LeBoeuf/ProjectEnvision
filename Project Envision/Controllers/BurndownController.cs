using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Controllers
{
    namespace DevExtreme.NETCore.Demos.Controllers
    {
        public class BurndownController : Controller
        {
            public ActionResult Line()
            {
                return View();
            }
        }
    }
}
