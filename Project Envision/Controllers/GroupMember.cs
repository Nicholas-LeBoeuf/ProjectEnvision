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
    public class GroupMember : Controller
    {

        int getUserId(string username)
        {
            int userId = 0;
            if (username != "None")
            {
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

                MySqlCommand getUserId = connection.CreateCommand();
                getUserId.CommandText = "SELECT user_id FROM users where username= @username";
                getUserId.Parameters.AddWithValue("@username", username);

                MySqlDataReader reader = getUserId.ExecuteReader();

                while (reader.Read())
                {
                    userId = Convert.ToInt32(reader[0]);
                }
            }

            return userId;
        }

        bool validateUser(string username)
        {
                bool userdatatable = false;
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();
                string selectCommand = $"SELECT* FROM users where username = '" + username + "'"; // the command
                MySqlCommand command = new MySqlCommand(selectCommand, connection);

                MySqlDataReader dRead;

                using (dRead = command.ExecuteReader())
                {
                    if (dRead.Read())
                    {
                        userdatatable = true;
                    }
                    else
                    {
                        dRead.Close();
                    }
                }
                dRead.Close();
                
                if(userdatatable == true)
                { 
                    selectCommand = $"SELECT* FROM invitedboard where inviteduser = '" + username + "' AND board_id = '" + boardModel.m_BoardId + "'";
                    command = new MySqlCommand(selectCommand, connection);
                    MySqlDataReader sRead;
                    using (sRead = command.ExecuteReader())
                    {
                        if (sRead.Read())
                        {
                            ViewBag.Message = "Already in board";
                            sRead.Close();
                            return false;
            
                        }
                        else
                        {
                            sRead.Close();
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
        }

        public IActionResult inviteGroupMember(GroupMembers groupMembers)
        {
            bool validUsername = validateUser(groupMembers.username);

            if(ModelState.IsValid)
            { 
                if (validUsername == true)
                {
                    MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                    connection.Open();

                    string insertCommand = $"Insert into invitedboard(board_id, user_id, inviteduser)" + $"values (@board_id, @user_id, @inviteduser)";
                        MySqlCommand command = new MySqlCommand(insertCommand, connection);

                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@board_id", boardItems.m_BoardId);
                        command.Parameters.AddWithValue("@user_id", getUserId(groupMembers.username));
                        command.Parameters.AddWithValue("@inviteduser", groupMembers.username);
                        command.Prepare();
                        command.ExecuteReader();
                        connection.Close();

                        ViewBag.Message = "User added successfully";
                        return RedirectToAction("Teammates", "GroupMember");
                }
            }
            return RedirectToAction("Teammates", "GroupMember");
        }

        public IActionResult removeGroupMember(string username)
        {
                MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

                connection.Open();

                MySqlCommand removeMember = connection.CreateCommand();

                removeMember.CommandText = "Delete FROM invitedboard where board_id = @boardID AND inviteduser = @inviteduser";

                removeMember.Parameters.AddWithValue("@boardID", boardModel.m_BoardId);
                removeMember.Parameters.AddWithValue("@inviteduser", username);

                removeMember.Prepare();
                removeMember.ExecuteReader();
                connection.Close();

                if(ModelItems.m_Username != username)
                {
                    ViewBag.Message = "User removed successfully";
                    return RedirectToAction("Teammates", "GroupMember");
                }
                else
                {
                    boardItems.m_GotBoard = false;
                    return RedirectToAction("ChooseBoard", "Board");
                }
        }

        public IActionResult AddTeammate()
        {
            return View();
        }

        public IActionResult RemoveTeammate(GroupMembers groupMembers)
        {
            if(groupMembers.removeUsername != null)
            {
                removeGroupMember(groupMembers.removeUsername);
                ViewBag.Message = "User removed successfully";
                return RedirectToAction("Teammates", "GroupMember");
            }
            return View();
        }
        public IActionResult Teammates()
        {
            return View();
        }
    }
}
