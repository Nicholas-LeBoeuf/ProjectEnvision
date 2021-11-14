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
    public class DragNDropController : Controller
    {

        void updateTask(int taskId, string location)
        {

            string updateCommand = $"Update tasks set location ='" + location + "' where task_id ='" + taskId + "'";



            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand command = new MySqlCommand(updateCommand, connection);

            command.Prepare();
            command.ExecuteReader();
            connection.Close();
            boardModel.m_GotTask = false;

        }

        void updateCompleteTask(int taskId)
        {
            if(DragNDropModel.location == "Done" || DragNDropModel.currentLocation == "Done")
            { 
                string currentDate = DateTime.Now.ToString("MM-dd-yyyy");
                int points = 0;

                for (int i = 0; i < boardModel.m_TaskPointsList.Count(); i++)
                {
                    if (boardModel.m_TaskIdList[i] == taskId)
                    {
                        points = boardModel.m_TaskPointsList[i];
            
                        if (DragNDropModel.currentLocation == "Done")
                            points = -(points * 2);
                    }
                }

                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();
                string insetcommand = $"Insert into burndownchart(user_Id,task_Points,completedDate,board_Id, sprint_Id)" + $"values ( @user_Id,@task_Points,@completedDate, @board_Id, sprint_Id) ";
                MySqlCommand command = new MySqlCommand(insetcommand, databaseConnection);
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@user_Id", ModelItems.m_UserId);
                command.Parameters.AddWithValue("@task_Points", points);
                command.Parameters.AddWithValue("@completedDate", currentDate);
                command.Parameters.AddWithValue("@board_Id", boardModel.m_BoardId);
                command.Parameters.AddWithValue("@sprint_Id", 0);

                command.Prepare();
                command.ExecuteReader();
                databaseConnection.Close();

            }
        }

        

        public IActionResult setLocation(string location)
        {
            DragNDropModel.location = location;
            return RedirectToAction("dragNDropUpdate");
        }

        public IActionResult setCurrentLocation(string currentLocation)
        {
            DragNDropModel.currentLocation = currentLocation;
            return RedirectToAction("dragNDropUpdate", new {location = currentLocation});
        }

        public IActionResult setTaskId(int taskId)
        {
            DragNDropModel.taskId = taskId;
            return RedirectToAction("dragNDropUpdate", new { taskId = taskId });
        }

        public IActionResult dragNDropUpdate()
        {
            DragNDropModel.returnBoard = true;

            if (DragNDropModel.location == null || DragNDropModel.currentLocation == DragNDropModel.location)
            {
                return RedirectToAction("Board","Board");
            }
            else
            {
                    updateTask(DragNDropModel.taskId, DragNDropModel.location);
                    updateCompleteTask(DragNDropModel.taskId);
                    
            }
                return RedirectToAction("GetTask", "Task");
            }
        }

    }
