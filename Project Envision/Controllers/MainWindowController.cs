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

        public IActionResult CreateBoard(CreateboardModel Cb)
        {

            if (ModelState.IsValid)
            {

                MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

                conn.Open();

                string counttxt = $"SELECT * FROM createboard Where user_id = '" + ModelItems.m_userid + "'";
                MySqlCommand countcmd = new MySqlCommand(counttxt, conn);
                
                MySqlDataReader tRead;

                int count = 0;

                using (tRead = countcmd.ExecuteReader())
                {
                    if (tRead.HasRows)
                    {

                        while (tRead.Read())
                        {
                            count++;
                        }
                    }
                }
                if (count >= 5)
                {
                    ViewBag.message = "You Have Max Boards";
                    tRead.Close();
                    conn.Close();
                    return View("CreateBoard");
                }
                string txtcmd = $"SELECT* FROM createboard where board_name = '" + Cb.board_name + "' AND user_id = '" + ModelItems.m_userid + "'";
                MySqlCommand textcmd = new MySqlCommand(txtcmd, conn);
                

                    using (tRead = textcmd.ExecuteReader())
                    {
                        if (tRead.Read())
                        {
                            ViewBag.message = "You already have board with that name";
                            tRead.Close();
                            conn.Close();
                            return View("CreateBoard");
                        }
                    }

                    string txtcmd2 = $"Insert into createboard (board_name,user_id,username, board_description)" + $"values ( @board_name,@user_id,@username, @board_description) ";
                    MySqlCommand cmd3 = new MySqlCommand(txtcmd2, conn);
                    cmd3.CommandType = CommandType.Text;
                    cmd3.Parameters.AddWithValue("@board_name", Cb.board_name);
                    cmd3.Parameters.AddWithValue("@board_description", Cb.board_Description);
                    cmd3.Parameters.AddWithValue("@user_id", ModelItems.m_userid);
                    cmd3.Parameters.AddWithValue("@username", ModelItems.m_username);

                    cmd3.Prepare();
                    cmd3.ExecuteReader();
                    conn.Close();
                    return RedirectToAction("ChooseBoard");
            }
            
            return View("CreateBoard");
            }

            public IActionResult ChooseBoard()
        {
            return View();
        }
    }
}
