using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using Project_Envision.Models.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Controllers
{
    public class BoardSettingsController : Controller
    {
        public IActionResult Index()
        {
            return View("Views/Settings/BoardSettings.cshtml");
        }
        public IActionResult textColor(BoardSettings boardSettings)
        {
            string color = boardSettings.textColor;
            return RedirectToAction("Index");
        }

        public IActionResult cardColor(BoardSettings boardSettings)
        {
            string color = boardSettings.cardColor;
            return RedirectToAction("Index");
        }

        public IActionResult getBoardParts(int board_id)
        {

            MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

            databaseConnection.Open();

            MySqlCommand getBoards = databaseConnection.CreateCommand();
            getBoards.CommandText = "SELECT board_Name, board_description FROM createboard where board_id= @boardID";
            getBoards.Parameters.AddWithValue("@boardID", board_id);

            MySqlDataReader reader = getBoards.ExecuteReader();

            while (reader.Read())
            {
                BoardSettingsItems.m_BoardDescription = Convert.ToString(reader[0]);
                BoardSettingsItems.m_BoardName = Convert.ToString(reader[1]);
            }
            reader.Close();

            databaseConnection.Close();
            return RedirectToAction("ChooseBoard");
        }

        public IActionResult boardName(BoardSettings boardSettings)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string selectCommand = $"SELECT* FROM createboard where board_Name = '" + boardSettings.boardName + "' AND user_id = '" + ModelItems.m_UserId + "'";
                MySqlCommand command = new MySqlCommand(selectCommand, databaseConnection);
                MySqlDataReader sRead;

                using (sRead = command.ExecuteReader())
                {
                    if (sRead.Read())
                    {
                        ViewBag.message = "You already have board with that name";
                        sRead.Close();
                        databaseConnection.Close();
                        return View("Board Settings");
                    }
                }

                string insertCommand = $"Update createboard set board_name ='" + boardSettings.boardName + "' where board_id ='" + boardItems.m_BoardId + "'";
                command = new MySqlCommand(insertCommand, databaseConnection);
                command.Prepare();
                command.ExecuteReader();
                databaseConnection.Close();

                boardItems.m_GotBoard = false;
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }

        public IActionResult editBoardDescription(CreateboardModel editBoard)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string insertCommand = $"Update createboard set board_description ='" + editBoard.board_Description + "' where board_id ='" + boardItems.m_BoardId + "'";

                MySqlCommand command = new MySqlCommand(insertCommand, databaseConnection);

                command.Prepare();
                command.ExecuteReader();
                databaseConnection.Close();

                boardItems.m_GotBoard = false;
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }

    }
}
