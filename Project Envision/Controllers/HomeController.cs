using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(LoginModel lm)
        {
            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);


            string username = lm.username;
            string Password = lm.password;

            conn.Open();
            string txtcmd2 = $"SELECT* FROM users where username = '" + username + "' AND password = '" + Password + "'"; // the command
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);

            MySqlDataReader dRead;

            if (ModelState.IsValid)
            {
                using (dRead = cmd2.ExecuteReader())
                {
                    if (dRead.Read())
                    {

                        string txtcmd1 = "SELECT user_id FROM users where username='" + username + "'";
                        MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                        dRead.Close();
                        using (dRead = cmd1.ExecuteReader())
                        {
                            if (dRead.Read())
                            {
                                lm.id = Convert.ToInt32(dRead.GetValue(0).ToString());
                            }
                        }
                        txtcmd1 = "SELECT email FROM users where username='" + username + "'";
                        cmd1 = new MySqlCommand(txtcmd1, conn);
                        dRead.Close();
                        using (dRead = cmd1.ExecuteReader())
                        {
                            if (dRead.Read())
                            {
                                DBObject.m_email = dRead.GetValue(0).ToString();
                            }
                        }
                        conn.Close();
                        dRead.Close();
                        return View("/Main");
                    }
                    else
                    {
                        conn.Close();
                        dRead.Close();

                        ViewBag.message = "username not found or password incorrect!";
                        return View("Login");
                    }

                }

            }
            return View("Login");
    }


        public IActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccount(CreateAccountModel Ca)
        {

            if (ModelState.IsValid)
            {
                MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

                conn.Open();

                string datatxt = $"SELECT user_id FROM users where username='" + Ca.username + "'";
                MySqlCommand datacmd = new MySqlCommand(datatxt, conn);
                MySqlDataReader dRead;
                using (dRead = datacmd.ExecuteReader())
                {
                    if (dRead.Read())
                    {
                        conn.Close();
                        ViewBag.message = "username has already been taken";
                        return View("CreateAccount");

                    }
                }
                dRead.Close();

                if (Ca.Password != Ca.confirmPassword)
                {

                    conn.Close();
                    return View("CreateAccount");
                }

                else
                {
                    string txtcmd = $"Insert into users (first_name,last_name,username,Password,email)" + $"values ( @firstName, @lastName,@username,@Password,@email) ";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@firstName", Ca.firstName);
                    cmd.Parameters.AddWithValue("@lastName", Ca.lastName);
                    cmd.Parameters.AddWithValue("@username", Ca.username);
                    cmd.Parameters.AddWithValue("@Password", Ca.Password);
                    cmd.Parameters.AddWithValue("@email", Ca.email);
                    cmd.Prepare();
                    cmd.ExecuteReader();
                    conn.Close();
                    return View("Login");
                }
            }
            return View("CreateAccount");
        }

        public IActionResult ForgotPassword1()
        {
            return View();
        }
        public IActionResult ForgotPassword2()
        {
            return View();
        }
        public IActionResult ForgotPassword3()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
