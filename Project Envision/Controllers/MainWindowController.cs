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

        public IActionResult DeleteBoard (int Boardid)
        {
            BoardItems.m_gotBoard = false;


            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

            conn.Open();

            MySqlCommand DeleteBoard = conn.CreateCommand();
            DeleteBoard.CommandText = "Delete FROM createboard where user_id= @userID AND board_id = @boardId";
            DeleteBoard.Parameters.AddWithValue("@userID", ModelItems.m_userid);
            DeleteBoard.Parameters.AddWithValue("@boardId", Boardid);
            DeleteBoard.Prepare();
            DeleteBoard.ExecuteReader();
            conn.Close();
            return RedirectToAction("ChooseBoard");
        }

        public IActionResult GetBoarditems(ChooseBoardModel cbm)
        {
            BoardItems.m_gotBoard = true;
            MySqlConnection conn = new MySqlConnection(Database_connection.m_connection);

            conn.Open();

            MySqlCommand getBoards = conn.CreateCommand();
            getBoards.CommandText = "SELECT board_name, board_id FROM createboard where user_id= @userID";
            getBoards.Parameters.AddWithValue("@userID", ModelItems.m_userid);

            MySqlDataReader reader = getBoards.ExecuteReader();

            List<string> BoardsList = new List<string>();
            List<int> BoardidsList = new List<int>();

            while (reader.Read())
            {
                BoardsList.Add(Convert.ToString(reader[0]));
                BoardidsList.Add(Convert.ToInt32(reader[1]));
            }
            reader.Close();

            cbm.SetBoardlistListAttr(BoardsList);
            cbm.SetBoardidlistListAttr(BoardidsList);


            MySqlCommand getBoarddesc = conn.CreateCommand();
            getBoarddesc.CommandText = "SELECT board_description FROM createboard where user_id= @userID";
            getBoarddesc.Parameters.AddWithValue("@userID", ModelItems.m_userid);

            MySqlDataReader dreader = getBoarddesc.ExecuteReader();

            List<string> BoarddescList = new List<string>();

            while (dreader.Read())
            {
                BoarddescList.Add(Convert.ToString(dreader[0]));
            }
            dreader.Close();

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
                    MySqlCommand cmd3 = new MySqlCommand(txtcmd2, conn);
                    cmd3.CommandType = CommandType.Text;
                    cmd3.Parameters.AddWithValue("@board_name", Cb.board_name);
                    cmd3.Parameters.AddWithValue("@board_description", Cb.board_Description);
                    cmd3.Parameters.AddWithValue("@user_id", ModelItems.m_userid);
                    cmd3.Parameters.AddWithValue("@username", ModelItems.m_username);

                    cmd3.Prepare();
                    cmd3.ExecuteReader();
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
