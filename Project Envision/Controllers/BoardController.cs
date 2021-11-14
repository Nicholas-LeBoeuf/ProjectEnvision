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

        public IActionResult Main()
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
                return RedirectToAction("GetTask", "Task");
            }
            if (BoardModel.m_gotusers == false)
            {
                return RedirectToAction("Getusernames", "Task");

            }
            else
            {
                return View("Board", "Board");
            }
        }


        void DeleteboardParts(int Boardid, string tableName)
        {

            MySqlConnection databaseConnection= new MySqlConnection(Database_connection.m_connection);

            databaseConnection.Open();

            MySqlCommand DeleteBoardpart = databaseConnection.CreateCommand();

            DeleteBoardpart.CommandText = "Delete FROM " + tableName +  " where user_id= @userID AND board_id = @boardId";
            
            DeleteBoardpart.Parameters.AddWithValue("@userID", ModelItems.m_userid);
            DeleteBoardpart.Parameters.AddWithValue("@boardId", Boardid);
          
            DeleteBoardpart.Prepare();
            DeleteBoardpart.ExecuteReader();
            databaseConnection.Close();
        }

        public IActionResult DeleteBoard (int Boardid)
        {
            BoardItems.m_gotBoard = false;

            DeleteboardParts(Boardid, "tasks");
            DeleteboardParts(Boardid, "createboard");
            
            return RedirectToAction("ChooseBoard");
        }

        public IActionResult GetBoarditems(ChooseBoardModel ChooseBoardModel)
        {
            BoardItems.m_gotBoard = true;
            MySqlConnection databaseConnection= new MySqlConnection(Database_connection.m_connection);

            databaseConnection.Open();

            MySqlCommand getBoards = databaseConnection.CreateCommand();
            getBoards.CommandText = "SELECT board_name, board_id, board_description FROM createboard where user_id= @userID";
            getBoards.Parameters.AddWithValue("@userID", ModelItems.m_userid);

            MySqlDataReader reader = getBoards.ExecuteReader();

            List<string> BoardsList = new List<string>();
            List<int> BoardidsList = new List<int>();
            List<string> BoarddescList = new List<string>();
            
            while (reader.Read())
            {
                BoardsList.Add(Convert.ToString(reader[0]));
                BoardidsList.Add(Convert.ToInt32(reader[1]));
                BoarddescList.Add(Convert.ToString(reader[2]));
            }
            reader.Close();

            ChooseBoardModel.SetBoardlistListAttr(BoardsList);
            ChooseBoardModel.SetBoardidlistListAttr(BoardidsList);
            ChooseBoardModel.SetBoardDesclistListAttr(BoarddescList);

            databaseConnection.Close();
            return RedirectToAction("ChooseBoard");
        }

 

        public IActionResult createBoard(CreateboardModel createBoardModel)
        {

            if (ModelState.IsValid)
            {

                MySqlConnection databaseConnection= new MySqlConnection(Database_connection.m_connection);

                databaseConnection.Open();

                string txtcmd = $"SELECT* FROM createboard where board_name = '" + createBoardModel.board_name + "' AND user_id = '" + ModelItems.m_userid + "'";
                MySqlCommand textcmd = new MySqlCommand(txtcmd, databaseConnection);
                 MySqlDataReader tRead;

                    using (tRead = textcmd.ExecuteReader())
                    {
                        if (tRead.Read())
                        {
                            ViewBag.message = "You already have board with that name";
                            tRead.Close();
                            databaseConnection.Close();
                            return View("CreateBoard");
                        }
                    }

                    string insetcommand = $"Insert into createboard (board_name,user_id,username, board_description)" + $"values ( @board_name,@user_id,@username, @board_description) ";
                    MySqlCommand command = new MySqlCommand(insetcommand, databaseConnection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@board_name", createBoardModel.board_name);
                    command.Parameters.AddWithValue("@board_description", createBoardModel.board_Description);
                    command.Parameters.AddWithValue("@user_id", ModelItems.m_userid);
                    command.Parameters.AddWithValue("@username", ModelItems.m_username);

                    command.Prepare();
                    command.ExecuteReader();
                    databaseConnection.Close();
                BoardItems.m_gotBoard = false;
                return RedirectToAction("ChooseBoard");
            }
            
            return View("CreateBoard");
            }
        
        public IActionResult ChooseBoard(ChooseBoardModel cbm)
        {
            if(BoardItems.m_gotBoard == false)
            {
                return RedirectToAction("GetBoarditems");
            }
            return View();
        }

        public IActionResult ProductBacklog(TaskPropertiesModel tpm, BoardModel bm)
        {
            if (BoardModel.m_gotTask == false)
            {
                return RedirectToAction("GetTask", "Task");
            }
            if (BoardModel.m_gotusers == false)
            {
                return RedirectToAction("Getusernames", "Task");

            }
            else
            {
                return View();
            }
        }

        public IActionResult BurndownChart()
        {
            return View();
        }
    }
}
