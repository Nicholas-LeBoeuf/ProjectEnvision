using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using System;
using System.Data;
using System.Diagnostics;

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

        void setUserId(string username)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);
            connection.Open();

            MySqlDataReader dRead;

            string selectCommand = "SELECT user_id FROM users where username='" + username + "'";
            MySqlCommand command = new MySqlCommand(selectCommand, connection);
            using (dRead = command.ExecuteReader())
            {
                if (dRead.Read())
                {
                    ModelItems.m_UserId = Convert.ToInt32(dRead.GetValue(0).ToString());
                }
            }
            connection.Close();
            dRead.Close();
        }

        void setEmail(string username)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);
            connection.Open();

            MySqlDataReader dRead;

            string selectCommand = "SELECT email FROM users where username='" + username + "'";
            MySqlCommand command = new MySqlCommand(selectCommand, connection);

            using (dRead = command.ExecuteReader())
            {
                if (dRead.Read())
                {
                    ModelItems.m_Email = dRead.GetValue(0).ToString();
                }
            }
            connection.Close();
            dRead.Close();
        }

        public IActionResult Login(LoginModel loginModel)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            string username = loginModel.username;
            string password = loginModel.password;

            if (username != null || password != null)
            {

                connection.Open();

                string selectCommand = $"SELECT* FROM users where username = '" + username + "' AND password = '" + password + "'";
                MySqlCommand command = new MySqlCommand(selectCommand, connection);

                MySqlDataReader dRead;

                if (ModelState.IsValid)
                {
                    using (dRead = command.ExecuteReader())
                    {
                        if (dRead.Read())
                        {
                            setUserId(username);
                            setEmail(username);

                            connection.Close();
                            dRead.Close();

                            return RedirectToAction("GetBoarditems", "Board");
                        }
                        else
                        {
                            connection.Close();
                            dRead.Close();

                            ViewBag.message = "username not found or password incorrect!";
                            return View("Login");
                        }
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
        public IActionResult CreateAccount(CreateAccountModel createAccount)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

                string selectCommand = $"SELECT user_id FROM users where username='" + createAccount.username + "'";
                MySqlCommand dataCommand = new MySqlCommand(selectCommand, connection);
                MySqlDataReader dRead;
                
                using (dRead = dataCommand.ExecuteReader())
                {
                    if (dRead.Read())
                    {
                        connection.Close();
                        ViewBag.message = "username has already been taken";
                        return View("CreateAccount");

                    }
                }

                dRead.Close();

                if (createAccount.Password != createAccount.confirmPassword)
                {
                    connection.Close();
                    return View("CreateAccount");
                }

                else
                {
                    string insertCommand = $"Insert into users (first_name,last_name,username,Password,email)" + $"values ( @firstName, @lastName,@username,@Password,@email) ";
                    MySqlCommand command = new MySqlCommand(insertCommand, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@firstName", createAccount.firstName);
                    command.Parameters.AddWithValue("@lastName", createAccount.lastName);
                    command.Parameters.AddWithValue("@username", createAccount.username);
                    command.Parameters.AddWithValue("@Password", createAccount.Password);
                    command.Parameters.AddWithValue("@email", createAccount.email);
                    command.Prepare();
                    command.ExecuteReader();
                    connection.Close();
                    return View("Login");
                }
            }
            return View("CreateAccount");
        }

        void generateSecuriteCode()
        {
            int random;
            string codes = "";
            Random rand = new Random();

            for (int i = 0; i < 6; i++)
            {
                random = rand.Next(10);
                codes = random + codes;
            }

            ModelItems.code = codes;
        }

        void sendEmail()
        {
            MimeMessage codemail = new MimeMessage();
            MailboxAddress from = new MailboxAddress("ProjectEnvision", "ProjectEnvision.gmail.com");
            codemail.From.Add(from);

            MailboxAddress to = new MailboxAddress(ModelItems.m_Username, ModelItems.m_Email);
            codemail.To.Add(to);
            codemail.Subject = "Forgot Password Security Code";

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);

            client.Authenticate("ProjectEnvision2021@gmail.com", "ProjectEnvision123!");

            codemail.Body = new TextPart("plain")
            {

                Text = @"Forgot-Password Security-code: " + ModelItems.code.ToString()
            };

            client.Send(codemail);
            client.Disconnect(true);
            client.Dispose();
        }

        public IActionResult ForgotPassword1(ForgotPassword1 forgotPassword1)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);
            MySqlDataReader reader;
            string selectcommand = $"Select* FROM users where username = '" + forgotPassword1.username + "' and email = '" + forgotPassword1.email + "'";
            MySqlCommand command = new MySqlCommand(selectcommand, connection);

            connection.Open();

            if (ModelState.IsValid)
            {
                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ModelItems.m_Username = forgotPassword1.username;
                        ModelItems.m_Email = forgotPassword1.email;

                        generateSecuriteCode();
                        sendEmail();
            
                        connection.Close();

                        return View("ForgotPassword2");
                    }
                    else
                    {
                        ViewBag.message = " Username or Email not Found";
                        connection.Close();
                        return View();
                    }
                }
            }

            connection.Close();
            return View();
        }

        public IActionResult ForgotPassword2(ForgotPassword2 forgotPassword2)
        {
            if (forgotPassword2.SecurityCode == ModelItems.code)
            {
                return View("ForgotPassword3");
            }
            else
            {
                ViewBag.message = "Incorrect Code";
                return View();
            }
        }

        void updatePassword (string password, string confirmPassword)
        {
            string updatecommand = "update users SET password = '" + password + "' Where username = '" + ModelItems.m_Username + "'";

            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);
            MySqlCommand dataCommand = new MySqlCommand(updatecommand, connection);

            connection.Open();
            dataCommand.ExecuteNonQuery();

            ViewBag.message = ("Password has been Changed");
            connection.Close();
        }

        public IActionResult ForgotPassword3(ForgotPassword3 forgotPassword3)
        {

            if (forgotPassword3.Password != forgotPassword3.confirmPassword)
            {
                return View();
            }

            updatePassword(forgotPassword3.Password, forgotPassword3.confirmPassword);

            return View("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}