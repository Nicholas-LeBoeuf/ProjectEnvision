using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Project_Envision.Models
{
    public class GroupMembers
    {
        public string username = "";

        public GroupMembers(string usernameinput)
        {
            username = usernameinput;
        }

        public string getUsername()
        {
            return username;
        }
    }
}
