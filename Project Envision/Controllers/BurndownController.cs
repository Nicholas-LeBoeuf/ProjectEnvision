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
    public class BurndownController : Controller
    {
        public IActionResult GetPoints()
        {
            /*
            List<int> points = new List<int>();

            Burndown.m_BurndownTaskPoints = points;


            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT task_points FROM burndownchart where board_id = '" + boardItems.m_BoardId + "' AND user_id = '" + ModelItems.m_UserId + "'", connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        points.Add(Convert.ToInt32(reader));
                    }
                }
            connection.Close();
            */

            return View();

        }
    }
}
