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
    public class BoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Board(int Boardid)
        {
            if (Boardid != 0)
            {
                BoardItems.m_Boardid = Boardid;
            }

            if (BoardModel.m_gotTask == false)
            {
                return RedirectToAction("GetTask");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Deletetask(int taskid)
        {


            DeletetaskParts(taskid, "tasks");

            BoardModel.m_gotTask = false;
            
            if (BoardModel.m_gotTask == false)
            {
                return RedirectToAction("GetTask");
            }

            return View();
        }

        void DeletetaskParts(int taskid, string tableName)
        {

            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

            conn.Open();

            MySqlCommand DeleteTaskpart = conn.CreateCommand();

            DeleteTaskpart.CommandText = "Delete FROM " + tableName + " where board_id = @boardID AND task_id = @taskId";

            DeleteTaskpart.Parameters.AddWithValue("@boardID", BoardModel.m_Boardid);
            DeleteTaskpart.Parameters.AddWithValue("@taskId", taskid);

            DeleteTaskpart.Prepare();
            DeleteTaskpart.ExecuteReader();
            conn.Close();
        }

        public IActionResult GetTask(BoardModel bm)
        {
            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

            conn.Open();

            MySqlCommand getTasks = conn.CreateCommand();
            getTasks.CommandText = "SELECT taskname, location, task_id FROM tasks where board_id= @boardID";
            getTasks.Parameters.AddWithValue("@boardID", bm.Boardid);

            MySqlDataReader reader = getTasks.ExecuteReader();

            List<string> TaskList = new List<string>();
            List<string> TaskLocationList = new List<string>();
            List<int> task_id = new List<int>();

            while (reader.Read())
            {
                TaskList.Add(Convert.ToString(reader[0]));
                TaskLocationList.Add(Convert.ToString(reader[1]));
                task_id.Add(Convert.ToInt32(reader[2]));
            }
            reader.Close();

            bm.SetTasklistListAttr(TaskList);
            bm.SetTaskLocationlistListAttr(TaskLocationList);
            bm.SetTaskIdlistListAttr(task_id);

            conn.Close();
            
            BoardModel.m_gotTask = true;
            
            return RedirectToAction("Board");
        }

        public IActionResult CreateTask(TaskPropertiesModel tpm)
        {
            if (ModelState.IsValid)
            {

                MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

                conn.Open();

                string txtcmd2 = $"Insert into tasks(taskname, taskdescription, board_id, location, task_points)" + $"values ( @taskname,@taskdescription,@board_id, @location, @Task_point) ";
                MySqlCommand cmd = new MySqlCommand(txtcmd2, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@taskname", tpm.Task_name);
                cmd.Parameters.AddWithValue("@taskdescription", tpm.Task_Description);
                cmd.Parameters.AddWithValue("@board_id", BoardItems.m_Boardid);
                cmd.Parameters.AddWithValue("@Task_point", tpm.Task_points);
                cmd.Parameters.AddWithValue("@location", "Backlog");

                cmd.Prepare();
                cmd.ExecuteReader();
                conn.Close();
                BoardModel.m_gotTask = false;

                return RedirectToAction("Board");
            }
            return View();
        }
        public IActionResult ProductBacklog()
        {
            return View();
        }
        public IActionResult BurndownChart()
        {
            return View();
        }
    }
}
