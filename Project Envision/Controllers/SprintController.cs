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
    public class SprintController : Controller
    {
        public IActionResult sprint()
        {
            return View();
        }

        public IActionResult sprintUpdate(int sprintId)
        {
            updateSprint(sprintId, DragNDropModel.taskId);
            
            return RedirectToAction("GetTask", "Task");
        }

        public IActionResult setCurrentSprint(int sprintId)
        {
            GetSprintProperties.currentSprint_Id = sprintId;

            for (int i = 0; i < GetSprintProperties.getSprint_IdList.Count(); i++)
            {
                if (GetSprintProperties.getSprint_IdList[i] == sprintId)
                {
                    string updateCommand = $"Update sprint set Currentsprint ='" + 1 + "' where Sprint_id ='" + sprintId + "'";

                    MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand(updateCommand, connection);

                    command.Prepare();
                    command.ExecuteReader();
                    connection.Close();
                    GetSprintProperties.currentSprint_Id = sprintId;
                    GetSprintProperties.getCurrent_SprintList[i] = true;
                }
            }
            currentSprintCount();

            return RedirectToAction("ProductBacklog", "Board");
        }

        public void currentSprintCount()
        {
            for (int i = 0; i < GetSprintProperties.getSprint_IdList.Count(); i++)
            {
                if(GetSprintProperties.getCurrent_SprintList[i] == true)
                {
                    GetSprintProperties.currentSprintCount++;
                }
            }
        }

        public IActionResult currentSprintCompleted()
        {
            for (int i = 0; i < GetSprintProperties.getSprint_IdList.Count(); i++)
            {
                if (GetSprintProperties.getCurrent_SprintList[i] == true)
                {
                    string updateCommand = $"Update sprint set Sprint_completed ='" + 1 + "', Currentsprint ='" + 0 + "'where Sprint_id ='" + GetSprintProperties.getSprint_IdList[i] + "'";

                    MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                    connection.Open();

                    MySqlCommand command = new MySqlCommand(updateCommand, connection);

                    command.Prepare();
                    command.ExecuteReader();
                    connection.Close();

                    GetSprintProperties.getCurrent_SprintList[i] = false;
                    GetSprintProperties.getSprint_CompletedList[i] = true;
                    GetSprintProperties.currentSprintCount--;
                    GetSprintProperties.currentSprint_Id = 0;
                }
            }
            return RedirectToAction("ProductBacklog", "Board");
        }

        public void getValidSprints()
        {

            DateTime currentDate = DateTime.Now;

            for (int i = 0; i< GetSprintProperties.getSprint_IdList.Count(); i++)
            {
                DateTime start = Convert.ToDateTime(GetSprintProperties.getSprint_StartList[i]);
                DateTime end = Convert.ToDateTime(GetSprintProperties.getSprint_EndList[i]);

                int check = DateTime.Compare(currentDate, start);

                if (check == 0 || check > 0)
                {
                    check = DateTime.Compare(currentDate, end);

                    if (check == 0 || check < 0)
                    {
                        if (GetSprintProperties.getSprint_CompletedList[i] == false)
                        {
                            GetSprintProperties.getValidCurrent_SprintList[i] = true;
                        }
                    }
                }
            }
            currentSprintCount();
        }

    public IActionResult setSprintTaskId(int taskId)
        {
            DragNDropModel.taskId = taskId;

            return RedirectToAction("ProductBacklog", "Board");
        }

       private void updateSprint(int sprintId, int taskId)
        {
            string updateCommand = $"Update tasks set Sprint_id ='" + sprintId + "' where task_id ='" + taskId + "'";

            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand command = new MySqlCommand(updateCommand, connection);

            command.Prepare();
            command.ExecuteReader();
            connection.Close();
            boardModel.m_GotTask = false;
        }

        public IActionResult deleteSprint(int sprintId)
        {
            deleteSprintParts(sprintId, "tasks");
            deleteSprintParts(sprintId, "sprint");

            getValidSprints();

            boardModel.m_GotSprint = false;

            if (boardModel.m_GotSprint == false)
            {
                return RedirectToAction("getSprint");
            }

            return View();
        }

        void deleteSprintParts(int sprintId, string tableName)
        {

            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand deleteSprintpart = connection.CreateCommand();

            deleteSprintpart.CommandText = "Delete FROM " + tableName + " where board_id = @boardID AND sprint_id = @sprintId";

            deleteSprintpart.Parameters.AddWithValue("@boardID", boardModel.m_BoardId);
            deleteSprintpart.Parameters.AddWithValue("@sprintId", sprintId);

            deleteSprintpart.Prepare();
            deleteSprintpart.ExecuteReader();
            connection.Close();

        }

        public IActionResult getSprint(GetSprintProperties getSprintProperties)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand getSprint = connection.CreateCommand();
            getSprint.CommandText = "SELECT Sprintname, Sprint_id, Sprintdescription, Start_time, End_time, Sprint_completed, Currentsprint FROM sprint where board_id= @boardID";
            getSprint.Parameters.AddWithValue("@boardID", boardModel.m_BoardId);

            MySqlDataReader reader = getSprint.ExecuteReader();

            List<string> sprintnameList = new List<string>();
            List<string> sprintDescriptList = new List<string>();
            List<string> startList = new List<string>();
            List<string> endList = new List<string>();
            List<int> sprint_Id = new List<int>();
            List<bool> sprint_completed = new List<bool>();
            List<bool> currentSprint = new List<bool>();
            List<bool> validSprint = new List<bool>();
            
            while (reader.Read())
            {
                sprintnameList.Add(Convert.ToString(reader[0]));
                sprint_Id.Add(Convert.ToInt32(reader[1]));
                sprintDescriptList.Add(Convert.ToString(reader[2]));
                startList.Add(Convert.ToDateTime(reader[3]).ToString("yyyy-MM-dd"));
                endList.Add(Convert.ToDateTime(reader[4]).ToString("yyyy-MM-dd"));
                sprint_completed.Add(Convert.ToBoolean(reader[5]));
                currentSprint.Add(Convert.ToBoolean(reader[6]));
                validSprint.Add(false);
                if(Convert.ToBoolean(reader[6]) == true)
                {
                    GetSprintProperties.currentSprint_Id = Convert.ToInt32(reader[1]);
                }
            }
            reader.Close();

            connection.Close();

            getSprintProperties.setGetSprintIdList(sprint_Id);
            getSprintProperties.setGetSprintNameList(sprintnameList);
            getSprintProperties.setSprintDescriptList(sprintDescriptList);
            getSprintProperties.setSprintStartList(startList);
            getSprintProperties.setSprintEndList(endList);
            getSprintProperties.setCurrentSprintList(currentSprint);
            getSprintProperties.setSprintCompletedList(sprint_completed);
            getSprintProperties.setValidCurrentSprintList(validSprint);

            boardModel.m_GotSprint = true;

            getValidSprints();
            
            if (boardModel.m_GotUsers == false)
            {
                return RedirectToAction("Board", "Board");
            }

            boardModel.m_ReturnToBoard = true;

                return RedirectToAction("Board", "Board");
        }

        public IActionResult collect_Sprint_Info(GetSprintProperties sprintProperties, int sprintId)
        {
            if (ModelState.IsValid)
            {
                sprintProperties.setGetSprintId(sprintId);
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

                MySqlCommand getInfo = connection.CreateCommand();
                getInfo.CommandText = "SELECT Sprintname, Sprintdescription, Start_time, End_time  FROM sprint where Sprint_id= @sprint_id";
                getInfo.Parameters.AddWithValue("@sprint_id", sprintId);

                MySqlDataReader reader = getInfo.ExecuteReader();

                while (reader.Read())
                {
                    sprintProperties.setGetSprintName(Convert.ToString(reader[0]));
                    sprintProperties.setSprintDescript(Convert.ToString(reader[1]));
                    sprintProperties.setSprintStart(Convert.ToDateTime(reader[2]).ToString("yyyy-MM-dd"));
                    sprintProperties.setSprintEnd(Convert.ToDateTime(reader[3]).ToString("yyyy-MM-dd"));
                }

                reader.Close();

                connection.Close();

                getValidSprints();

                return RedirectToAction("editSprint","Sprint");
            }
            return View("Sprint");
        }

        public IActionResult createSprint(SprintPropertiesModel sprintPropertiesModel)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);
                connection.Open();

                string selectCommand = $"SELECT Sprint_id FROM sprint where Sprintname='" + sprintPropertiesModel.sprint_Name + "' and board_id ='"+ boardModel.m_BoardId +"'";
                
                MySqlCommand dataCommand = new MySqlCommand(selectCommand, connection);
                MySqlDataReader dRead;
                
                using (dRead = dataCommand.ExecuteReader())
                {
                    if (dRead.Read())
                    {
                        connection.Close();
                        ViewBag.message = "You already have a sprint named that";
                        return View("Sprint");

                    }
                }
                int length = sprintPropertiesModel.sprint_Name.Length - 1;

                string sprintName = sprintPropertiesModel.sprint_Name.Substring(0, 1).ToUpper() + sprintPropertiesModel.sprint_Name.Substring(1,length);
                
                string insertCommand = " ";

                insertCommand = $"Insert into sprint(Sprintname, Sprintdescription, Start_time, End_time, board_id, Sprint_completed, Currentsprint)" + $"values ( @Sprintname,@Sprintdescription,@Start_time,@End_time,@board_id,@Sprint_completed,@Currentsprint)";

                MySqlCommand command = new MySqlCommand(insertCommand, connection);

                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Sprintname", sprintName);
                command.Parameters.AddWithValue("@Sprintdescription", sprintPropertiesModel.sprint_Description);
                command.Parameters.AddWithValue("@Start_time", sprintPropertiesModel.start_Time.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@End_time", sprintPropertiesModel.end_Time.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@board_id", boardItems.m_BoardId);
                command.Parameters.AddWithValue("@Sprint_completed", false);
                command.Parameters.AddWithValue("@Currentsprint", false);

                command.Prepare();
                command.ExecuteReader();
                connection.Close();
                boardModel.m_GotSprint = false;
                boardModel.m_ReturnToBoard = true;
                getValidSprints();

                return RedirectToAction("board", "board");
            }
            return View();
        }

        public IActionResult editSprint(SprintPropertiesModel sprintPropertiesModel)
        {
            if (ModelState.IsValid)
            {
                string updateCommand = $"Update sprint set Sprintname ='" + sprintPropertiesModel.sprint_Name + "', Sprintdescription ='" + sprintPropertiesModel.sprint_Description + "', Start_time ='" + sprintPropertiesModel.start_Time.ToString("yyyy-MM-dd") + "', End_time ='" + sprintPropertiesModel.end_Time.ToString("yyyy-MM-dd") + "' where Sprint_id ='" + GetSprintProperties.getSprint_Id + "'";

                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

                MySqlCommand command = new MySqlCommand(updateCommand, connection);

                command.Prepare();
                command.ExecuteReader();
                connection.Close();
                
                boardModel.m_GotSprint = false;
                boardModel.m_ReturnToBoard = true;

                getValidSprints();

                return RedirectToAction("board", "board");
            }
            return View();
        }
    }
}

