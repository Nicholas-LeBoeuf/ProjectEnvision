using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models.Board
{
    public class BoardSettingsItems
    {
        public static string m_BoardName;
        public static string m_BoardDescription;
        public static string m_BoardBackground;
        public static string m_TextColor;
        public static string m_CardColor;

        public string boardName
        {
            get => m_BoardName;
            set => m_BoardName = value;
        }

        public string boardDescription
        {
            get => m_BoardDescription;
            set => m_BoardDescription = value;
        }

        public string boardBackground
        {
            get => m_BoardBackground;
            set => m_BoardBackground = value;
        }

        public string textColor
        {
            get => m_TextColor;
            set => m_TextColor = value;
        }

        public string cardColor
        {
            get => m_CardColor;
            set => m_CardColor = value;
        }
    }
}
