using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Controllers
{
    public class DragNDropController : Controller
    {

        void updateTask(int taskId, string location)
        {

            string txtcmd2 = $"Update tasks SET location ='" + location + "' where task_id ='" + taskId + "'";



            MySqlConnection connection = new MySqlConnection(Database_connection.m_connection);

            connection.Open();

            MySqlCommand comand = new MySqlCommand(txtcmd2, connection);

            comand.Prepare();
            comand.ExecuteReader();
            connection.Close();
            BoardModel.m_gotTask = false;

        }

        void completeTask(int taskId)
        {
            int points = 0;

            for (int i = 0; i < BoardModel.m_TaskPointslist.Count(); i++)
            {
                if (BoardModel.m_TaskIdlist[i] == taskId)
                {
                    points = BoardModel.m_TaskPointslist[i];
                }
            }



        }

        void updateCompleteTask(int taskId)
        {

        }

        public IActionResult setLocation(string location)
        {
            DragNDropModel.location = location;
            return RedirectToAction("dragNDropUpdate");
        }

        public IActionResult setTaskId(int taskId)
        {
            DragNDropModel.taskid = taskId;
            return RedirectToAction("dragNDropUpdate", new { taskId = taskId });
        }

        public IActionResult dragNDropUpdate()
        {
            DragNDropModel.returnboard = true;

            if (DragNDropModel.location == null)
            {
                return View("");
            }
            else
            {
                if (DragNDropModel.location == "Done")
                {
                    updateTask(DragNDropModel.taskid, DragNDropModel.location);
                    // completeTask(BoardModel.taskid);
                }

                else
                {
                    updateTask(DragNDropModel.taskid, DragNDropModel.location);
                    // updateCompleteTask(BoardModel.taskid);
                }
                return RedirectToAction("GetTask", "Task");
            }
        }

    }
}
