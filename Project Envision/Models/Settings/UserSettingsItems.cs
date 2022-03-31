using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class UserSettingsItems
    {
        public static string m_Username;
        public static string m_Password;
        public static string m_ConfirmPassword;

        public string Username
        {
            get => m_Username;
            set => m_Username = value;
        }

        public string Password
        {
            get => m_Username;
            set => m_Username = value;
        }

        public string ConfirmPassword
        {
            get => m_ConfirmPassword;
            set => m_ConfirmPassword = value;
        }

    }
}
