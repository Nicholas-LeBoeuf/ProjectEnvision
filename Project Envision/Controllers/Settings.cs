using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Project_Envision.Models;


namespace Project_Envision.Controllers
{
    public class Settings : Controller
    {
        public IActionResult UserSettings()
        {
            return View();
        }

        public IActionResult getBoardParts()
        {

            MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

            databaseConnection.Open();

            MySqlCommand getBoards = databaseConnection.CreateCommand();
            getBoards.CommandText = "SELECT username, password FROM users where user_id= @userID";
            getBoards.Parameters.AddWithValue("@userID", ModelItems.m_UserId);

            MySqlDataReader reader = getBoards.ExecuteReader();

            while (reader.Read())
            {
                UserSettingsItems.m_Username = Convert.ToString(reader[0]);
                UserSettingsItems.m_Password = Convert.ToString(reader[1]);
            }
            reader.Close();
            databaseConnection.Close();
            return RedirectToAction("UserSettings");
        }
        public IActionResult ChangeUsername(UserSettings edituser)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string selectCommand = $"SELECT * FROM users where username = '" + edituser.Username + "'";
                MySqlCommand command = new MySqlCommand(selectCommand, databaseConnection);
                MySqlDataReader sRead;

                using (sRead = command.ExecuteReader())
                {
                    if (sRead.Read())
                    {
                        databaseConnection.Close();
                        ViewBag.message = "Username already exists";
                        return RedirectToAction("UserSettings");
                    }
                }

                sRead.Close();

                string insertCommand = $"Update users set username ='" + edituser.Username + "' where user_id ='" + ModelItems.m_UserId + "'";
                command = new MySqlCommand(insertCommand, databaseConnection);
                command.Prepare();
                command.ExecuteReader();
                databaseConnection.Close();

                
            }
            

            return View("UserSettings");
        }

        public IActionResult ChangePassword(UserSettings editpassword)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string selectCommand = $"SELECT * FROM users where password = '"+ editpassword.Password +"'";
                MySqlCommand command = new MySqlCommand(selectCommand, databaseConnection);
                MySqlDataReader sRead;

                using (sRead = command.ExecuteReader())
                {
                    if (sRead.Read())
                    {
                        ViewBag.message = "Password cannot be previous password";
                        sRead.Close();
                        databaseConnection.Close();
                        return RedirectToAction("UserSettings");
                    }
                }

                if (editpassword.Password != editpassword.ConfirmPassword)
                {
                    ViewBag.message = "Passwords don't match";
                    sRead.Close();
                    databaseConnection.Close();
                    return RedirectToAction("UserSettings");
                }
                else
                {

                    string insertCommand = $"Update users set password ='" + editpassword.Password + "' where user_id ='" + ModelItems.m_UserId + "'";
                    command = new MySqlCommand(insertCommand, databaseConnection);
                    command.Prepare();
                    command.ExecuteReader();
                    databaseConnection.Close();

                }


            }


            return View("UserSettings");
        }






        public IActionResult BoardSettings()
        {
            return View();
        }
    }
}
