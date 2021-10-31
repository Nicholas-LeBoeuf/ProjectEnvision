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
    public class MainWindowController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Main()
        {
            return View();
        }

        void DeleteboardParts(int Boardid, string tableName)
        {

            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

            conn.Open();

            MySqlCommand DeleteBoardpart = conn.CreateCommand();

            DeleteBoardpart.CommandText = "Delete FROM " + tableName + " where user_id= @userID AND board_id = @boardId";
            
            DeleteBoardpart.Parameters.AddWithValue("@userID", ModelItems.m_userid);
            DeleteBoardpart.Parameters.AddWithValue("@boardId", Boardid);
          
            DeleteBoardpart.Prepare();
            DeleteBoardpart.ExecuteReader();
            conn.Close();
        }

        public IActionResult DeleteBoard (int Boardid)
        {
            BoardItems.m_gotBoard = false;

            DeleteboardParts(Boardid, "tasks");
            DeleteboardParts(Boardid, "createboard");
            
            return RedirectToAction("ChooseBoard");
        }

        public IActionResult GetBoarditems(ChooseBoardModel cbm)
        {
            BoardItems.m_gotBoard = true;
            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

            conn.Open();

            MySqlCommand getBoards = conn.CreateCommand();
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

            cbm.SetBoardlistListAttr(BoardsList);
            cbm.SetBoardidlistListAttr(BoardidsList);
            cbm.SetBoardDesclistListAttr(BoarddescList);

            conn.Close();
            return RedirectToAction("ChooseBoard");
        }

 

        public IActionResult CreateBoard(CreateboardModel Cb)
        {

            if (ModelState.IsValid)
            {

                MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

                conn.Open();

                string txtcmd = $"SELECT* FROM createboard where board_name = '" + Cb.board_name + "' AND user_id = '" + ModelItems.m_userid + "'";
                MySqlCommand textcmd = new MySqlCommand(txtcmd, conn);
                 MySqlDataReader tRead;

                    using (tRead = textcmd.ExecuteReader())
                    {
                        if (tRead.Read())
                        {
                            ViewBag.message = "You already have board with that name";
                            tRead.Close();
                            conn.Close();
                            return View("CreateBoard");
                        }
                    }

                    string txtcmd2 = $"Insert into createboard (board_name,user_id,username, board_description)" + $"values ( @board_name,@user_id,@username, @board_description) ";
                    MySqlCommand cmd = new MySqlCommand(txtcmd2, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@board_name", Cb.board_name);
                    cmd.Parameters.AddWithValue("@board_description", Cb.board_Description);
                    cmd.Parameters.AddWithValue("@user_id", ModelItems.m_userid);
                    cmd.Parameters.AddWithValue("@username", ModelItems.m_username);

                    cmd.Prepare();
                    cmd.ExecuteReader();
                    conn.Close();
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
    }
}
