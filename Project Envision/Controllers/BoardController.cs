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
    public class boardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Main()
        {
            return View();
        }

        public IActionResult board(int boardId)
        {
            if (boardId != 0)
            {
                boardItems.m_BoardId = boardId;
            }

            if (boardModel.m_GotTask == false)
            {
                return RedirectToAction("GetTask", "Task");
            }
            if (boardModel.m_GotUsers == false)
            {
                return RedirectToAction("getUsernames", "Task");

            }
            else
            {
                return View("Board", "Board");
            }
        }


        void deleteBoardParts(int boardId, string tableName)
        {

            MySqlConnection databaseConnection= new MySqlConnection(Database_connection.m_Connection);

            databaseConnection.Open();

            MySqlCommand deleteBoardpart = databaseConnection.CreateCommand();

            if(tableName == "sprint")
            { 
                deleteBoardpart.CommandText = "Delete FROM " + tableName + " where board_id = @boardId";
                deleteBoardpart.Parameters.AddWithValue("@boardId", boardId);
            }
            else
            { 
                deleteBoardpart.CommandText = "Delete FROM " + tableName +  " where user_id= @userID AND board_id = @boardId";
            
                deleteBoardpart.Parameters.AddWithValue("@userID", ModelItems.m_UserId);
                deleteBoardpart.Parameters.AddWithValue("@boardId", boardId);
          }

            deleteBoardpart.Prepare();
            deleteBoardpart.ExecuteReader();
            databaseConnection.Close();
        }

        public IActionResult deleteBoard (int boardId)
        {
            boardItems.m_GotBoard = false;

            deleteBoardParts(boardId, "tasks");
            deleteBoardParts(boardId, "createboard");
            deleteBoardParts(boardId, "burndownchart");
            deleteBoardParts(boardId, "sprint");

            return RedirectToAction("ChooseBoard");
        }

        public IActionResult getBoardItems(ChooseBoardModel chooseBoardModel)
        {
            boardItems.m_GotBoard = true;
            MySqlConnection databaseConnection= new MySqlConnection(Database_connection.m_Connection);

            databaseConnection.Open();

            MySqlCommand getBoards = databaseConnection.CreateCommand();
            getBoards.CommandText = "SELECT board_Name, board_id, board_description FROM createboard where user_id= @userID";
            getBoards.Parameters.AddWithValue("@userID", ModelItems.m_UserId);

            MySqlDataReader reader = getBoards.ExecuteReader();

            List<string> boardsList = new List<string>();
            List<int> boardIdsList = new List<int>();
            List<string> boardDescList = new List<string>();
            
            while (reader.Read())
            {
                boardsList.Add(Convert.ToString(reader[0]));
                boardIdsList.Add(Convert.ToInt32(reader[1]));
                boardDescList.Add(Convert.ToString(reader[2]));
            }
            reader.Close();

            chooseBoardModel.setBoardListAttr(boardsList);
            chooseBoardModel.setBoardIdListAttr(boardIdsList);
            chooseBoardModel.setBoardDescListAttr(boardDescList);

            databaseConnection.Close();
            return RedirectToAction("ChooseBoard");
        }

 

        public IActionResult createBoard(CreateboardModel createBoardModel)
        {

            if (ModelState.IsValid)
            {

                MySqlConnection databaseConnection= new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string selectCommand = $"SELECT* FROM createboard where board_Name = '" + createBoardModel.board_Name + "' AND user_id = '" + ModelItems.m_UserId + "'";
                MySqlCommand command = new MySqlCommand(selectCommand, databaseConnection);
                 MySqlDataReader sRead;

                    using (sRead = command.ExecuteReader())
                    {
                        if (sRead.Read())
                        {
                            ViewBag.message = "You already have board with that name";
                            sRead.Close();
                            databaseConnection.Close();
                            return View("CreateBoard");
                        }
                    }

                    string insetcommand = $"Insert into createboard (board_Name,user_id,username, board_description)" + $"values ( @board_Name,@user_id,@username, @board_description) ";
                    MySqlCommand command2 = new MySqlCommand(insetcommand, databaseConnection);
                    command2.CommandType = CommandType.Text;
                    command2.Parameters.AddWithValue("@board_Name", createBoardModel.board_Name);
                    command2.Parameters.AddWithValue("@board_description", createBoardModel.board_Description);
                    command2.Parameters.AddWithValue("@user_id", ModelItems.m_UserId);
                    command2.Parameters.AddWithValue("@username", ModelItems.m_Username);

                    command2.Prepare();
                    command2.ExecuteReader();
                    databaseConnection.Close();
                boardItems.m_GotBoard = false;
                return RedirectToAction("ChooseBoard");
            }
            
            return View("CreateBoard");
            }
        
        public IActionResult ChooseBoard()
        {
            if(boardItems.m_GotBoard == false)
            {
                return RedirectToAction("getBoardItems");
            }
            return View();
        }

        public IActionResult ProductBacklog(boardModel boardModel)
        {
            if (boardModel.gotTask == false)
            {
                return RedirectToAction("GetTask", "Task");
            }
            if (boardModel.gotUsers == false)
            {
                return RedirectToAction("getUsernames", "Task");

            }
            else
            {
                return View();
            }
        }

        public IActionResult BurndownChart()
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            string selectCommand = $"SELECT task_points FROM burndownchart where board_id = '" + boardItems.m_BoardId +  "' AND user_id = '" + ModelItems.m_UserId + "'";



            return View();
        }
    }
}
