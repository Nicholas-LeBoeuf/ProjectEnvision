using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;

using Newtonsoft.Json;


namespace Project_Envision.Controllers
{
    public class BurndownController : Controller
    {
        public IActionResult BurndownChart()
        {
            return View("BurndownChart");
        }

        public object GetBurndownValues()
        {

            List<Burndown> vals = new List<Burndown>();
            vals.Add(new Burndown { Date = "Story Point Total", StoryPoints = Burndown.TaskTotal });

            for (int i = 0; i < (Burndown.m_GraphDates.Count()); i++)
            {
                if(DateTime.Compare(Burndown.m_GraphDates[i], DateTime.Now) <= 0)
                {
                     vals.Add(new Burndown { Date = Burndown.m_GraphDates[i].ToString("MM-dd-yyyy"), StoryPoints = Burndown.m_DataTaskPoints[i]});
                }
               
            }

            IEnumerable<Burndown> BurndownList = vals;

            return BurndownList;
        }

        public int dayCalc(DateTime startDate, DateTime endDate)
        {
            int Num = (endDate - startDate).Days;
            return Num;
        }

        public void graphDataPrep(Burndown burndown)
        {
            int addDays = dayCalc(Convert.ToDateTime(Burndown.sprintStartTime), Convert.ToDateTime(Burndown.sprintEndTime));
            DateTime dateTracker = Convert.ToDateTime(Burndown.sprintStartTime);
            double total = Burndown.TaskTotal;
            double dayTotal = 0;

            List<DateTime> graphDates = new List<DateTime>();
            List<double> graph_Task_Points = new List<double>();

            graphDates.Add(dateTracker.Date);
            for (int j = 0; j < Burndown.m_BurndownDates.Count(); j++)
            {
                if(dateTracker.DayOfYear == Burndown.m_BurndownDates[j].DayOfYear)
                {
                    dayTotal = dayTotal + Burndown.m_BurndownTaskPoints[j];
                    
                }

            }
            total = total - dayTotal;
            graph_Task_Points.Add(total);



            for (int i = 0; i < addDays; i++)
            {
                dateTracker = dateTracker.AddDays(1);
                graphDates.Add(dateTracker.Date);
                dayTotal = 0;
                for (int j = 0; j < Burndown.m_BurndownDates.Count(); j++)
                {
                    if (dateTracker.DayOfYear == Burndown.m_BurndownDates[j].DayOfYear)
                    {
                       dayTotal = dayTotal + Burndown.m_BurndownTaskPoints[j];
                        
                    }
                }
                total = total - dayTotal;
                graph_Task_Points.Add(total);
            }
            burndown.setGraphDates(graphDates);
            burndown.setGraphTaskPoints(graph_Task_Points);

        }
        public void getBurndownInfo(int sprintId, Burndown burndown)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand getInfo = connection.CreateCommand();
            getInfo.CommandText = "SELECT task_points, completedDate FROM burndownchart where Sprint_id= @sprint_id and user_Id= @userId";
            getInfo.Parameters.AddWithValue("@sprint_id", sprintId);
            getInfo.Parameters.AddWithValue("@userId", ModelItems.m_UserId);

            List<DateTime> completedDate = new List<DateTime>();
            List<double> task_Points = new List<double>();

            MySqlDataReader reader = getInfo.ExecuteReader();

            while (reader.Read())
            {
                task_Points.Add(Convert.ToDouble(reader[0]));
                completedDate.Add(Convert.ToDateTime(reader[1]));
            }

            burndown.setBurndownTaskPoints(task_Points);
            burndown.setBurndownDates(completedDate);
            reader.Close();
            connection.Close();

            totalTaskPoints(sprintId);
            startAndEnd(sprintId, burndown);
            graphDataPrep(burndown);
            
        }

        public void startAndEnd(int sprintId, Burndown burndown)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand getInfo = connection.CreateCommand();
            getInfo.CommandText = "SELECT Start_time, End_time FROM sprint where Sprint_id= @sprint_id";
            getInfo.Parameters.AddWithValue("@sprint_id", sprintId);
            getInfo.Parameters.AddWithValue("@userId", ModelItems.m_UserId);

            MySqlDataReader reader = getInfo.ExecuteReader();

            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    burndown.setSprintStartTime(Convert.ToString(reader[0]));
                    burndown.setSprintEndTime(Convert.ToString(reader[1]));
                }
                else
                {
                    burndown.setSprintStartTime("1/1/1");
                    burndown.setSprintEndTime("1/1/1");
                }
            }
            reader.Close();
            connection.Close();

        }

        public void totalTaskPoints(int sprintId)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand getInfo = connection.CreateCommand();
            getInfo.CommandText = "SELECT Sum(task_points) FROM tasks where Sprint_id= @sprint_id";
            getInfo.Parameters.AddWithValue("@sprint_id", sprintId);
            getInfo.Parameters.AddWithValue("@userId", ModelItems.m_UserId);

            MySqlDataReader reader = getInfo.ExecuteReader();

            while (reader.Read())
            {
                try
                {
                    Burndown.TaskTotal = Convert.ToInt32(reader[0]);
                }
                catch
                {
                    Burndown.TaskTotal = 0;
                }
            }
            reader.Close();
            connection.Close();

        }

        public IActionResult BurndownMenu(Burndown burndown)
        {
            if (burndown.sprintId != 0)
            {
                getBurndownInfo(burndown.sprintId,burndown);
                return RedirectToAction("BurndownChart");
            }
            return View();
        }
    }
}

