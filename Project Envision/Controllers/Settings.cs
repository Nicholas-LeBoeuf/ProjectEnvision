using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Controllers
{
    public class Settings : Controller
    {
        public IActionResult UserSettings()
        {
            return View();
        }
    }
}
