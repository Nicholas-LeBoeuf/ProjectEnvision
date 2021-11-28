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

        List<int> getburndownpoints(Burndown burndown)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();


            MySqlCommand getPoints = connection.CreateCommand();


            getPoints.CommandText = $"SELECT task_points FROM burndownchart where board_id = '" + boardItems.m_BoardId + "' AND user_id = '" + ModelItems.m_UserId + "'";

            MySqlDataReader reader = getPoints.ExecuteReader();


            List<int> BurndownTaskPointsPlaceholder = new List<int>();
            List<int> BurndownTaskPoints = new List<int>();


            while (reader.Read())
            {
                BurndownTaskPointsPlaceholder.Add(Convert.ToInt32(reader[0]));
            }
            reader.Close();


            int length = BurndownTaskPointsPlaceholder.Count();
            int temp, addpoint;
            int sum = BurndownTaskPointsPlaceholder.Sum();

            for (int i = 0; i == length; i++)
            {
                temp = BurndownTaskPointsPlaceholder[i];
                addpoint = sum - temp;
                BurndownTaskPoints.Add(addpoint);

                sum = addpoint;

            }

            Burndown.m_BurndownTaskPoints = BurndownTaskPoints;


            connection.Close();

            return BurndownTaskPoints;

        }

        void getburndowndates(Burndown Burndown)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            List<string> Burndowndates = new List<string>();

            MySqlCommand getDates = connection.CreateCommand();

            getDates.CommandText = $"SELECT completedDate FROM burndownchart where board_id = '" + boardItems.m_BoardId + "' AND user_id = '" + ModelItems.m_UserId + "'";

            MySqlDataReader reader2 = getDates.ExecuteReader();

            while (reader2.Read())
            {
                Burndowndates.Add(Convert.ToString(reader2[0]));
            }
            reader2.Close();

            Burndown.m_Burndowndates = Burndowndates;

            connection.Close();

        }



    }
}
