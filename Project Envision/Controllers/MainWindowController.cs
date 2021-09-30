using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Controllers
{
    public class MainWindowController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Main()
        {
            return View();
        }

        public IActionResult CreateBoard()
        {
            return View();
        }

        public IActionResult ChooseBoard()
        {
            return View();
        }
    }
}
