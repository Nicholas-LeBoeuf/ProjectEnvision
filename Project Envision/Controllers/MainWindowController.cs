using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_Envision.Models;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Project_Envision.Controllers
{
    public class MainWindowController : Controller
    {
        public IActionResult Index()
        {
            return View("ChooseBoard");
        }

        public IActionResult Main()
        {
            return View();
        }

        public IActionResult CreateBoard(CreateboardModel Cb)
        {
                return View();
        }
        public IActionResult ChooseBoard()
        {
            return View();
        }
    }
}
