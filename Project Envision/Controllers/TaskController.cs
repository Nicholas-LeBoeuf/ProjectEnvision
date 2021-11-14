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

        public IActionResult deleteTask(int taskId)
        {
            deleteTaskParts(taskId, "tasks");

            boardModel.m_GotTask = false;
            
            if (boardModel.m_GotTask == false)
            {
                return RedirectToAction("getTask");
            }

            return View();
        }

        void deleteTaskParts(int taskId, string tableName)
        {

            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand deleteTaskpart = connection.CreateCommand();

            deleteTaskpart.CommandText = "Delete FROM " + tableName + " where board_id = @boardID AND task_id = @taskId";

            deleteTaskpart.Parameters.AddWithValue("@boardID", boardModel.m_BoardId);
            deleteTaskpart.Parameters.AddWithValue("@taskId", taskId);

            deleteTaskpart.Prepare();
            deleteTaskpart.ExecuteReader();
            connection.Close();
        }

        public IActionResult getTask(boardModel boardModel)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand getTasks = connection.CreateCommand();
            getTasks.CommandText = "SELECT taskname, location, task_id, taskdescription, task_Points, user_id FROM tasks where board_id= @boardID";
            getTasks.Parameters.AddWithValue("@boardID", boardModel.boardid);

            MySqlDataReader reader = getTasks.ExecuteReader();

            List<string> taskList = new List<string>();
            List<string> taskLocationList = new List<string>();
            List<string> taskDescriptList = new List<string>();
            List<string> assigneeList = new List<string>();
            List<int> task_Id = new List<int>();
            List<int> task_Points = new List<int>();
            List<int> user_Id = new List<int>();

            while (reader.Read())
            {
                taskList.Add(Convert.ToString(reader[0]));
                taskLocationList.Add(Convert.ToString(reader[1]));
                task_Id.Add(Convert.ToInt32(reader[2]));
                taskDescriptList.Add(Convert.ToString(reader[3]));
                task_Points.Add(Convert.ToInt32(reader[4]));
                getUsername(Convert.ToInt32(reader[5]));
                assigneeList.Add(TaskPropertiesModel.getAssignee);

            }
            reader.Close();

            boardModel.setTaskListAttr(taskList);
            boardModel.setTaskLocationListAttr(taskLocationList);
            boardModel.setTaskIdListAttr(task_Id);
            boardModel.setTaskDescriptListAttr(taskDescriptList);
            boardModel.setTaskPointsListAttr(task_Points);
            boardModel.setAsigneeListAttr(assigneeList);

            connection.Close();
            
            boardModel.m_GotTask = true;
            
            if(DragNDropModel.returnBoard == true)
            {
                DragNDropModel.returnBoard = false;
                return RedirectToAction("Board","Board");
            }

            else 
            {
                return RedirectToAction("ProductBacklog", "Board");
            }
            
        }

        public IActionResult getUsernames(boardModel boardModel)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand getTasks = connection.CreateCommand();
            getTasks.CommandText = "SELECT username FROM Createboard where board_id= @boardID";
            getTasks.Parameters.AddWithValue("@boardID", boardModel.boardid);

            MySqlDataReader reader = getTasks.ExecuteReader();

            List<string> usernameList = new List<string>();

            while (reader.Read())
            {
                usernameList.Add(Convert.ToString(reader[0]));
            }
            reader.Close();

            boardModel.setUsernameListAttr(usernameList);

            connection.Close();

            boardModel.m_GotUsers = true;

            return RedirectToAction("Board", "Board");
        }

        int getUserId( string username)
        {
            int userId = 0;
            if (username != "None")
            {
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

                MySqlCommand getUserId = connection.CreateCommand();
                getUserId.CommandText = "SELECT user_id FROM users where username= @username";
                getUserId.Parameters.AddWithValue("@username", username);

                MySqlDataReader reader = getUserId.ExecuteReader();

                while (reader.Read())
                {
                    userId = Convert.ToInt32(reader[0]);
                }
            }

            return userId;
        }

        void getUsername(int userId)
        {
            if (userId != 0)
            {
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

                MySqlCommand getUsername = connection.CreateCommand();
                getUsername.CommandText = "SELECT username FROM users where user_id= @userId";
                getUsername.Parameters.AddWithValue("@userId", userId);

                MySqlDataReader reader = getUsername.ExecuteReader();

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

        public IActionResult createTask(TaskPropertiesModel taskPropertiesModel)
        {
            
            if (ModelState.IsValid)
            {
                int userId = getUserId(taskPropertiesModel.assignee);

                string insertCommand = " ";

                if (userId == 0)
                {
                    userId = -1;
                }

                insertCommand = $"Insert into tasks(taskname, taskdescription, board_id, location, task_Points, user_id)" + $"values ( @taskname,@taskdescription,@board_id, @location, @Task_point, @user_id)";
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

               
                MySqlCommand command = new MySqlCommand(insertCommand, connection);
               
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@taskname", taskPropertiesModel.task_Name);
                command.Parameters.AddWithValue("@taskdescription", taskPropertiesModel.task_Description);
                command.Parameters.AddWithValue("@board_id", boardItems.m_BoardId);
                command.Parameters.AddWithValue("@Task_point", taskPropertiesModel.task_Points);
                command.Parameters.AddWithValue("@user_id", userId);
                

                command.Parameters.AddWithValue("@location", "Backlog");

                command.Prepare();
                command.ExecuteReader();
                connection.Close();
                boardModel.m_GotTask = false;

                return RedirectToAction("ProductBacklog", "Board");
            }
            return View();
        }

        public IActionResult collect_Task_Info (TaskPropertiesModel taskPropertiesModel, int taskId)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            int userId = 0;

            MySqlCommand getInfo = connection.CreateCommand();
            getInfo.CommandText = "SELECT taskname, taskdescription, user_id, task_Points  FROM tasks where task_id= @taskId";
            getInfo.Parameters.AddWithValue("@taskId", taskId);

            MySqlDataReader reader = getInfo.ExecuteReader();

            while (reader.Read())
            {
                    taskPropertiesModel.setGetTaskName(Convert.ToString(reader[0]));
                    taskPropertiesModel.setTaskDescript(Convert.ToString(reader[1]));
                    userId = Convert.ToInt32(reader[2]);
                    taskPropertiesModel.setTaskPoints(Convert.ToInt16(reader[3]));
            }
         
            reader.Close();

            getUsername(userId);

            connection.Close();

            boardItems.m_TaskId = taskId;

            boardModel.m_GotTask = false;

            return RedirectToAction("editTask");
        }


        public IActionResult editTask(TaskPropertiesModel taskPropertiesModel)
        {
            if (ModelState.IsValid)
            {
                int userId = getUserId(taskPropertiesModel.assignee);

                string insertCommand = " ";


                  insertCommand = $"Update tasks set taskname ='" + taskPropertiesModel.task_Name + "', taskdescription ='" + taskPropertiesModel.task_Description + "', board_id ='" + boardItems.m_BoardId + "', task_Points ='" + taskPropertiesModel.task_Points + "', user_id ='" + userId + "' where task_id ='" + boardItems.m_TaskId + "'";


               
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

                MySqlCommand command = new MySqlCommand(insertCommand, connection);

                command.Prepare();
                command.ExecuteReader();
                connection.Close();
                boardModel.m_GotTask = false;

                return RedirectToAction("ProductBacklog", "Board");
            }
            return View();
        }
    }
}
