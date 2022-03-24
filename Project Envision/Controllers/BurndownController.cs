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
            public ActionResult BurndownChart()
            {
                return View();
            }

            public ActionResult getChartParts()
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                MySqlCommand getIds = databaseConnection.CreateCommand();
                getIds.CommandText = "SELECT Sprint_id FROM sprint where board_id= @boardID";
                getIds.Parameters.AddWithValue("@boardID", boardItems.m_BoardId);

                MySqlDataReader reader = getIds.ExecuteReader();

                while (reader.Read())
                {
                    BurndownItems.m_SprintIds.Add(Convert.ToInt32(reader[0]));
                    
                }
                reader.Close();
                databaseConnection.Close();

                return View("BurndownChart");
            }


        }
    }
}
