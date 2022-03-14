using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class BoardSettingsItems
    {
        public static string m_BoardName;
        public static string m_BoardDescription;
        public static string m_BoardBackground;
        public static string m_TextColor;
        public static string m_CardColorRGBA;
        public static string m_CardColorHex;

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

        public string cardColorRGBA
        {
            get => m_CardColorRGBA;
            set => m_CardColorRGBA = value;
        }

        public string cardColorHex
        {
            get => m_CardColorHex;
            set => m_CardColorHex = value;
        }
    }
}
