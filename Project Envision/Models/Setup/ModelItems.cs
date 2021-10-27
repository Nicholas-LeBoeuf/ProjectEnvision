using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class ModelItems
    { 
        public static int m_userid;
        public static string m_username;
        public static string m_email;
        public static string code;

        public string username
        {
            get => m_username;
            set => m_username = value;
        }
        public string email
        {
            get => m_email;
            set => m_email = value;
        }
        public int Id
        {
            get => m_userid;
            set => m_userid = value;
        }
        public string RandCode
        {
            get => code;
            set => code = value;
        }

    }
}
