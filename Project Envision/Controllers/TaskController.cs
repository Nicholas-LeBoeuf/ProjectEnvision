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
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
            getTasks.CommandText = "SELECT taskname, location, task_id, taskdescription, task_points, user_id FROM tasks where board_id= @boardID";
            getTasks.Parameters.AddWithValue("@boardID", bm.Boardid);

            MySqlDataReader reader = getTasks.ExecuteReader();

            List<string> TaskList = new List<string>();
            List<string> TaskLocationList = new List<string>();
            List<string> TaskDescriptList = new List<string>();
            List<string> AssigneeList = new List<string>();
            List<int> task_id = new List<int>();
            List<int> task_points = new List<int>();
            List<int> user_id = new List<int>();

            while (reader.Read())
            {
                TaskList.Add(Convert.ToString(reader[0]));
                TaskLocationList.Add(Convert.ToString(reader[1]));
                task_id.Add(Convert.ToInt32(reader[2]));
                TaskDescriptList.Add(Convert.ToString(reader[3]));
                task_points.Add(Convert.ToInt32(reader[4]));
                getUsername(Convert.ToInt32(reader[5]));
                AssigneeList.Add(TaskPropertiesModel.getAssignee);

            }
            reader.Close();

            bm.SetTasklistListAttr(TaskList);
            bm.SetTaskLocationlistListAttr(TaskLocationList);
            bm.SetTaskIdlistListAttr(task_id);
            bm.SetTaskDescriptlistListAttr(TaskDescriptList);
            bm.SetTaskPointsListAttr(task_points);
            bm.SetAsigneeListAttr(AssigneeList);

            conn.Close();
            
            BoardModel.m_gotTask = true;
            
            return RedirectToAction("ProductBacklog","Board");
        }

        public IActionResult Getusernames(BoardModel bm)
        {
            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

            conn.Open();

            MySqlCommand getTasks = conn.CreateCommand();
            getTasks.CommandText = "SELECT username FROM Createboard where board_id= @boardID";
            getTasks.Parameters.AddWithValue("@boardID", bm.Boardid);

            MySqlDataReader reader = getTasks.ExecuteReader();

            List<string> usernameList = new List<string>();

            while (reader.Read())
            {
                usernameList.Add(Convert.ToString(reader[0]));
            }
            reader.Close();

            bm.SetusernamelistListAttr(usernameList);

            conn.Close();

            BoardModel.m_gotusers = true;

            return RedirectToAction("Board", "Board");
        }

        int getUserId( string username)
        {
            int userid = 0;
            if (username != "None")
            {
                MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

                conn.Open();

                MySqlCommand getuserid = conn.CreateCommand();
                getuserid.CommandText = "SELECT user_id FROM users where username= @username";
                getuserid.Parameters.AddWithValue("@username", username);

                MySqlDataReader reader = getuserid.ExecuteReader();

                while (reader.Read())
                {
                    userid = Convert.ToInt32(reader[0]);
                }
            }

            return userid;
        }

        void getUsername(int userid)
        {
            if (userid != 0)
            {
                MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

                conn.Open();

                MySqlCommand getusername = conn.CreateCommand();
                getusername.CommandText = "SELECT username FROM users where user_id= @userId";
                getusername.Parameters.AddWithValue("@userId", userid);

                MySqlDataReader reader = getusername.ExecuteReader();

                while (reader.Read())
                {
                    TaskPropertiesModel.getAssignee = Convert.ToString(reader[0]);

                }
            }
            else
            {
                TaskPropertiesModel.getAssignee = "None";
            }
        }

        public IActionResult CreateTask(TaskPropertiesModel tpm)
        {
            
            if (ModelState.IsValid)
            {
                int userid = getUserId(tpm.Assignee);

                string txtcmd2 = " ";

                if (userid == 0)
                {
                    userid = -1;
                }

                    txtcmd2 = $"Insert into tasks(taskname, taskdescription, board_id, location, task_points, user_id)" + $"values ( @taskname,@taskdescription,@board_id, @location, @Task_point, @user_id)";
                MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

                conn.Open();

               
                MySqlCommand cmd = new MySqlCommand(txtcmd2, conn);
               
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@taskname", tpm.Task_name);
                cmd.Parameters.AddWithValue("@taskdescription", tpm.Task_Description);
                cmd.Parameters.AddWithValue("@board_id", BoardItems.m_Boardid);
                cmd.Parameters.AddWithValue("@Task_point", tpm.Task_points);
                cmd.Parameters.AddWithValue("@user_id", userid);
                

                cmd.Parameters.AddWithValue("@location", "Backlog");

                cmd.Prepare();
                cmd.ExecuteReader();
                conn.Close();
                BoardModel.m_gotTask = false;

                return RedirectToAction("ProductBacklog", "Board");
            }
            return View();
        }

        public IActionResult collect_task_info (TaskPropertiesModel tpm, int taskid)
        {
            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

            conn.Open();

            int userid = 0;

            MySqlCommand getInfo = conn.CreateCommand();
            getInfo.CommandText = "SELECT taskname, taskdescription, user_id, task_points  FROM tasks where task_id= @taskID";
            getInfo.Parameters.AddWithValue("@taskID", taskid);

            MySqlDataReader reader = getInfo.ExecuteReader();

            while (reader.Read())
            {
                    tpm.SetgetTaskname(Convert.ToString(reader[0]));
                    tpm.SetTaskDescript(Convert.ToString(reader[1]));
                    userid = Convert.ToInt32(reader[2]);
                    tpm.SetTaskPoints(Convert.ToInt16(reader[3]));
            }
         
            reader.Close();

            getUsername(userid);

            conn.Close();

            BoardItems.m_Taskid = taskid;

            BoardModel.m_gotTask = false;

            return RedirectToAction("EditTask");
        }


        public IActionResult EditTask(TaskPropertiesModel tpm)
        {
            if (ModelState.IsValid)
            {
                int userid = getUserId(tpm.Assignee);

                string txtcmd2 = " ";


                  txtcmd2 = $"Update tasks SET taskname ='" + tpm.Task_name + "', taskdescription ='" + tpm.Task_Description + "', board_id ='" + BoardItems.m_Boardid + "', task_points ='" + tpm.Task_points + "', user_id ='" + userid + "' where task_id ='" + BoardItems.m_Taskid + "'";


               
                MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

                conn.Open();

                MySqlCommand cmd = new MySqlCommand(txtcmd2, conn);

                cmd.Prepare();
                cmd.ExecuteReader();
                conn.Close();
                BoardModel.m_gotTask = false;

                return RedirectToAction("ProductBacklog", "Board");
            }
            return View();
        }
    }
}
