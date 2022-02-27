using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Project_Envision.Models;
using Project_Envision.Models.Board;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Controllers
{
    public class BoardSettingsController : Controller
    {
        public IActionResult Index()
        {
            getBoardParts();
            bool firsttime = createBoardSettings(boardItems.m_BoardId);

            if(boardItems.m_GotBoardSettingsReturn == true)
            {
                boardItems.m_GotBoardSettingsReturn = false;

              return RedirectToAction("board", "board");
            }

            else
            {
                return View("Views/Settings/BoardSettings.cshtml");
            }
        }

        public bool createBoardSettings(int board_id)
        {

            MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

            databaseConnection.Open();
            string selectCommand = $"SELECT* FROM boardsettings where board_id = '" + boardModel.m_BoardId + "' AND user_id = '" + ModelItems.m_UserId + "'";
            MySqlCommand command = new MySqlCommand(selectCommand, databaseConnection);
            MySqlDataReader sRead;

            using (sRead = command.ExecuteReader())
            {
                if (sRead.Read())
                {
                    sRead.Close();
                    databaseConnection.Close();
                    getBoardSettingParts();
                    return false;
                }
            }

            string insetcommand = $"Insert into boardsettings (user_id,board_id,text_color,card_color,background_image)" + $"values (@user_id,@board_id,@text_color, @card_color, @background_image) ";
            MySqlCommand command2 = new MySqlCommand(insetcommand, databaseConnection);
            command2.CommandType = CommandType.Text;

            command2.Parameters.AddWithValue("@board_id", board_id);
            command2.Parameters.AddWithValue("@text_color", "#ffffff");
            command2.Parameters.AddWithValue("@user_id", ModelItems.m_UserId);
            command2.Parameters.AddWithValue("@card_color", "#0000000");
            command2.Parameters.AddWithValue("@background_image", "Computer-Background.jpg");

            command2.Prepare();
            command2.ExecuteReader();
            databaseConnection.Close();
            getBoardSettingParts();
            return true;
        }

        [HttpPost]
        public IActionResult textColor(BoardSettings boardSettings)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string insertCommand = $"Update boardsettings set text_color ='" + boardSettings.textColor + "' where board_id = '" + boardModel.m_BoardId + "' AND user_id = '" + ModelItems.m_UserId + "'";

                MySqlCommand command = new MySqlCommand(insertCommand, databaseConnection);

                command.Prepare();
                command.ExecuteReader();
                databaseConnection.Close();

                boardItems.m_GotBoard = false;
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        
        }

        [HttpPost]
        public IActionResult cardColor(BoardSettings boardSettings)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string insertCommand = $"Update boardsettings set card_color ='" + boardSettings.cardColor + "'where board_id = '" + boardModel.m_BoardId + "' AND user_id = '" + ModelItems.m_UserId + "'";

                MySqlCommand command = new MySqlCommand(insertCommand, databaseConnection);

                command.Prepare();
                command.ExecuteReader();
                databaseConnection.Close();

                boardItems.m_GotBoard = false;
                return RedirectToAction("index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult backgroundImage(BoardSettings boardSettings)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string insertCommand = $"Update boardsettings set background_image ='" + boardSettings.backgroundImage + "'where board_id = '" + boardModel.m_BoardId + "' AND user_id = '" + ModelItems.m_UserId + "'";

                MySqlCommand command = new MySqlCommand(insertCommand, databaseConnection);

                command.Prepare();
                command.ExecuteReader();
                databaseConnection.Close();

                boardItems.m_GotBoard = false;
                return RedirectToAction("index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult getBoardParts()
        {

            MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

            databaseConnection.Open();

            MySqlCommand getBoards = databaseConnection.CreateCommand();
            getBoards.CommandText = "SELECT board_Name, board_description FROM createboard where board_id= @boardID";
            getBoards.Parameters.AddWithValue("@boardID", boardItems.m_BoardId);

            MySqlDataReader reader = getBoards.ExecuteReader();

            while (reader.Read())
            {
                BoardSettingsItems.m_BoardDescription = Convert.ToString(reader[1]);
                BoardSettingsItems.m_BoardName = Convert.ToString(reader[0]);
            }
            reader.Close();
            databaseConnection.Close();
            return RedirectToAction("index");
        }

        public IActionResult getBoardSettingParts()
        {
            MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

            databaseConnection.Open();

            MySqlCommand getBoards = databaseConnection.CreateCommand();
            getBoards.CommandText = "SELECT text_color, card_color,background_image FROM boardsettings where board_id= @boardID";
            getBoards.Parameters.AddWithValue("@boardID", boardItems.m_BoardId);

            MySqlDataReader reader = getBoards.ExecuteReader();
            
            string hexadec = "";
            
            while (reader.Read())
            {
                BoardSettingsItems.m_TextColor = Convert.ToString(reader[0]);
                hexadec = Convert.ToString(reader[1]);
                BoardSettingsItems.m_CardColorHex = Convert.ToString(reader[1]);
                BoardSettingsItems.m_BoardBackground = Convert.ToString(reader[2]);
            }

            string rgba = hexadecimalToRGBA(hexadec);

            reader.Close();

            BoardSettingsItems.m_CardColorRGBA = rgba;
            databaseConnection.Close();
            boardItems.m_GotBoardSettings = true;
            return RedirectToAction("index");
        }

        public string hexadecimalToRGBA(string hexadec)
        {
            string rgba = "rgba(";

            int r = Convert.ToInt32(hexadec.Substring(1, 2), 16);
            int g = Convert.ToInt32(hexadec.Substring(3, 2), 16);
            int b = Convert.ToInt32(hexadec.Substring(5, 2), 16);
            rgba += Convert.ToString(r) + ",";
            rgba += Convert.ToString(g) + ",";
            rgba += Convert.ToString(b) + ",";
            rgba += "0.5)";

            return rgba;
        }

        [HttpPost]
        public IActionResult boardName(BoardSettings editBoard)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string selectCommand = $"SELECT* FROM createboard where board_Name = '" + editBoard.boardName + "' AND user_id = '" + ModelItems.m_UserId + "'";
                MySqlCommand command = new MySqlCommand(selectCommand, databaseConnection);
                MySqlDataReader sRead;

                using (sRead = command.ExecuteReader())
                {
                    if (sRead.Read())
                    {
                        ViewBag.message = "You already have board with that name";
                        sRead.Close();
                        databaseConnection.Close();
                        return RedirectToAction("index");
                    }
                }

                string insertCommand = $"Update createboard set board_name ='" + editBoard.boardName + "' where board_id ='" + boardItems.m_BoardId + "'";
                command = new MySqlCommand(insertCommand, databaseConnection);
                command.Prepare();
                command.ExecuteReader();
                databaseConnection.Close();

                boardItems.m_GotBoard = false;
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult editBoardDescription(BoardSettings editBoard)
        {
            if (ModelState.IsValid)
            {
                MySqlConnection databaseConnection = new MySqlConnection(Database_connection.m_Connection);

                databaseConnection.Open();

                string insertCommand = $"Update createboard set board_description ='" + editBoard.boardDescription + "' where board_id ='" + boardItems.m_BoardId + "'";

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
