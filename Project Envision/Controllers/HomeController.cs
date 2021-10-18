using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
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
            string txtcmd2 = $"SELECT* FROM users where username = '" + username + "' AND password = '" + Password + "'"; 
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);

            MySqlDataReader dRead;

            if (ModelState.IsValid)
            {
                using (dRead = cmd2.ExecuteReader())
                {
                    if (dRead.Read())
                    {
                        ModelItems.m_username = username;
                        string txtcmd1 = "SELECT user_id FROM users where username='" + username + "'";
                        MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                        dRead.Close();
                        using (dRead = cmd1.ExecuteReader())
                        {
                            if (dRead.Read())
                            {
                                ModelItems.m_userid = Convert.ToInt32(dRead.GetValue(0).ToString());
                            }
                        }
                        txtcmd1 = "SELECT email FROM users where username='" + username + "'";
                        cmd1 = new MySqlCommand(txtcmd1, conn);
                        dRead.Close();
                        using (dRead = cmd1.ExecuteReader())
                        {
                            if (dRead.Read())
                            {
                                ModelItems.m_email = dRead.GetValue(0).ToString();
                            }
                        }
                        conn.Close();
                        dRead.Close();
                        return RedirectToAction("Index","MainWindow");
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

        public IActionResult ForgotPassword1(ForgotPassword1 fp1)
        {  
            string username = fp1.username;
            string email = fp1.email;
            
            MySqlConnection Conn = new MySqlConnection(Database_connection.m_connection);
            MySqlDataReader reader;
            string selectcmd = $"Select* FROM users where username = '" + username + "' and email = '" + email + "'";
            MySqlCommand selectCmd = new MySqlCommand(selectcmd, Conn);
            
            Conn.Open();

            if (ModelState.IsValid)
            {
                using (reader = selectCmd.ExecuteReader())
                {
                    if(reader.Read())
                    {

                        int random;
                        string codes = "";
                        Random rand = new Random();            
                        ModelItems.m_username = username;
                        ModelItems.m_email = email;

                        for (int i = 0; i < 6; i++)
                        {
                            random = rand.Next(10);
                            codes = random + codes;
                        }

                        ModelItems.code = codes;

                        MimeMessage codemail = new MimeMessage();
                        MailboxAddress from = new MailboxAddress("ProjectEnvision", "ProjectEnvision.gmail.com");
                        codemail.From.Add(from);

                        MailboxAddress to = new MailboxAddress(ModelItems.m_username, ModelItems.m_email);
                        codemail.To.Add(to);
                        codemail.Subject = "Forgot Password Security Code";

                        SmtpClient client = new SmtpClient();
                        client.Connect("smtp.gmail.com", 465, true);
                        client.Authenticate("ProjectEnvision2021@gmail.com", "guess123!");

                        codemail.Body = new TextPart("plain")
                        {

                            Text = @"ForgotPassword Securitycode:" + ModelItems.code.ToString()
                        };

                        client.Send(codemail);
                        client.Disconnect(true);
                        client.Dispose();
                        Conn.Close();  

                        return View("ForgotPassword2");


                    }
                    else
                    {
                        ViewBag.message = " Username or Email not Found";
                        Conn.Close();
                        return View();
                    }
                }
            }
            Conn.Close();
            return View();
        }

        public IActionResult ForgotPassword2(ForgotPassword2 fp2)
        {

          
            if (fp2.SecurityCode == ModelItems.code)
            {
                return View("ForgotPassword3");
            }
            else
            {
                return View();
            }
        }

        public IActionResult ForgotPassword3(ForgotPassword3 fp3)
        {
            string Password = fp3.Password;
            string ConfirmPass = fp3.confirmPassword;
            string updatecmd = "update users SET password = '" + Password + "' Where username = '" + ModelItems.m_username + "'";

            
            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);
            MySqlCommand datacmd = new MySqlCommand(updatecmd, conn);

            conn.Open();
            datacmd.ExecuteNonQuery();

            ViewBag.message = ("Password has been Changed");
            conn.Close();

            return View("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
