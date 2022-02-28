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

        public void creatorUsername(int boardId)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand getTasks = connection.CreateCommand();

            getTasks.CommandText = "SELECT username FROM Createboard where board_id= @boardID";
            getTasks.Parameters.AddWithValue("@boardID", boardId);

            MySqlDataReader sreader = getTasks.ExecuteReader();
            while (sreader.Read())
            {
                boardModel.m_CreatorUsername = (Convert.ToString(sreader[0]));
            }
            sreader.Close();
            connection.Close();
        }

        public IActionResult board(int boardId)
        {
            if(boardModel.m_DifferentBoard == true)
            {
                boardModel.m_DifferentBoard = false;
                boardModel.m_GotSprint = false;
                boardModel.m_GotUsers = false;
                DragNDropModel.returnBoard = true;
            }

            if (boardId != 0)
            {
                boardItems.m_BoardId = boardId;
            }

            if(boardModel.m_GotSprint == false)
            {
                return RedirectToAction("GetSprint", "Sprint");
            }

            if (boardModel.m_GotTask == false)
            {
                return RedirectToAction("GetTask", "Task");
            }

            if (boardModel.m_GotUsers == false)
            {
                return RedirectToAction("getUsernames", "Task");

            }

            if(boardModel.m_ReturnToBoard == true)
            {
                boardModel.m_ReturnToBoard = false;
                return RedirectToAction("Sprint", "Sprint");
            }

            else
            {
                return View("Board");
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
            creatorUsername(boardId);
            if (boardModel.m_CreatorUsername == ModelItems.m_Username)
            { 
            deleteBoardParts(boardId, "tasks");
            deleteBoardParts(boardId, "createboard");
            deleteBoardParts(boardId, "burndownchart");
            deleteBoardParts(boardId, "sprint");
            }
            else
            {
                boardModel.m_BoardId = boardId;
                return RedirectToAction("removeGroupMember", "GroupMember", new { username = ModelItems.m_Username});
            }
            return RedirectToAction("ChooseBoard");
        }

        public IActionResult getBoardItems(ChooseBoardModel chooseBoardModel)
        {
            boardItems.m_GotBoard = true;
            chooseBoardModel.createdBoardNum = 0;

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
                chooseBoardModel.createdBoardNum++;
                boardsList.Add(Convert.ToString(reader[0]));
                boardIdsList.Add(Convert.ToInt32(reader[1]));
                boardDescList.Add(Convert.ToString(reader[2]));
            }
            reader.Close();
        
            List<int> boardIdInviteBoard = new List<int>();

            getBoards.CommandText = "SELECT board_id FROM invitedboard where user_id= @userID";
            MySqlDataReader sReader = getBoards.ExecuteReader();
            while (sReader.Read())
            {
                boardIdInviteBoard.Add(Convert.ToInt32(sReader[0]));
            }

            sReader.Close();

            for (int j = 0; j < boardIdInviteBoard.Count(); j++)
            {
                getBoards.CommandText = "SELECT board_Name, board_id, board_description FROM createboard where board_id= @board_Id";

                    getBoards.Parameters.AddWithValue("@board_Id", boardIdInviteBoard[j]);
 
                MySqlDataReader sReader2 = getBoards.ExecuteReader();

                while (sReader2.Read())
                {
                    boardsList.Add(Convert.ToString(sReader2[0]));
                    boardIdsList.Add(Convert.ToInt32(sReader2[1]));
                    boardDescList.Add(Convert.ToString(sReader2[2]));
                }
                sReader2.Close();
                getBoards.Parameters.Clear();
            }

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
            boardModel.m_GotUsers = false;

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

        public IActionResult BurndownChart(Burndown bur)
        {
            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select task_points from burndownchart";
            
            MySqlDataReader reader = cmd.ExecuteReader();
            List<int> points = new List<int>();


            while (reader.Read())
                {
                    points.Add(Convert.ToInt32(reader[0]));
                }
            reader.Close();
            connection.Close();

            bur.setburndowntaskpoints(points);

            return View();
        }

        

    }
}
