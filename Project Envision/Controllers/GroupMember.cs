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
                string selectCommand = $"SELECT* FROM users where username = '" + username + "'";
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
                        boardModel.m_GotUsers = false;
                        boardModel.m_MemberReturn = true;

                    ViewBag.message = "User added successfully";
                        return RedirectToAction("Teammates", "GroupMember");
                }
            }
            return RedirectToAction("Teammates", "GroupMember");
        }

        public IActionResult removeGroupMember(string username, string tableName)
        {
            int userID = 0;
            userID = getUserId(username);

            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand removeMember = connection.CreateCommand();

            if (tableName == "invitedboard")
            {
                removeMember.CommandText = "Delete FROM " + tableName + " where board_id = @boardID AND inviteduser = @inviteduser";
                removeMember.Parameters.AddWithValue("@boardID", boardModel.m_BoardId);
                removeMember.Parameters.AddWithValue("@inviteduser", username);

            }
            else
            {
                removeMember.CommandText = "Delete FROM " + tableName + " where board_id = @boardID and user_id = @user_Id";
                removeMember.Parameters.AddWithValue("@boardID", boardModel.m_BoardId);
                removeMember.Parameters.AddWithValue("@user_Id", userID);
            }

            removeMember.Prepare();
            removeMember.ExecuteReader();
            connection.Close();

            boardModel.m_GotUsers = false;
            boardModel.m_MemberReturn = true;

            if (ModelItems.m_Username != username)
            {
                ViewBag.message = "User removed successfully";

                    boardModel.m_GotTask = false;
                    return RedirectToAction("Teammates", "GroupMember");
            }
            else
            {
                boardItems.m_GotBoard = false;
                return RedirectToAction("ChooseBoard", "Board");
            }
        }

        void removeAssignee(string username)
        {
            string updateCommand = $"Update tasks set user_id = 0 where board_id ='" + boardModel.m_BoardId + "' and user_id = " + getUserId(username);

            MySqlConnection connection = new MySqlConnection(Database_connection.m_Connection);

            connection.Open();

            MySqlCommand command = new MySqlCommand(updateCommand, connection);

            command.Prepare();
            command.ExecuteReader();
            connection.Close();

        }

        public IActionResult AddTeammate()
        {
            return View("Teammates");
        }

        public IActionResult RemoveTeammate(GroupMembers groupMembers)
        {
            if (groupMembers.removeUsername != null)
            {
                removeAssignee(groupMembers.removeUsername);
                removeGroupMember(groupMembers.removeUsername, "invitedboard");
                ViewBag.message = "User removed sucessfully";
                
                return RedirectToAction("Teammates", "GroupMember");
            }

            return View("Teammates");
        }

        public IActionResult Teammates()
        {

            if (boardModel.m_GotUsers == false)
            {
                return RedirectToAction("getUsernames", "Task");

            }
            return View();
        }
    }
}
