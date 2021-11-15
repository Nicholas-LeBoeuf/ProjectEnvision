using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class ModelItems
    { 
        public static int m_UserId;
        public static string m_Username;
        public static string m_Email;
        public static string code;

        public string username
        {
            get => m_Username;
            set => m_Username = value;
        }
        public string email
        {
            get => m_Email;
            set => m_Email = value;
        }
        public int id
        {
            get => m_UserId;
            set => m_UserId = value;
        }
        public string randCode
        {
            get => code;
            set => code = value;
        }

    }
}
