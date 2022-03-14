using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class boardItems
    {
        public static bool m_GotBoard;

        public static bool m_GotBoardSettingsReturn;
        public static bool m_GotBoardSettings;
        public static int m_BoardId;
        public static int m_TaskId;

        public int taskId
        {
            get => m_TaskId;
            set => m_TaskId = value;
        }

        public bool gotBoard
        {
            get => m_GotBoard;
            set => m_GotBoard = value;
        }

        public bool gotBoardSettings
        {
            get => m_GotBoardSettings;
            set => m_GotBoardSettings = value;
        }

        public bool gotBoardSettingsReturn
        {
            get => m_GotBoardSettingsReturn;
            set => m_GotBoardSettingsReturn = value;
        }

        public int boardid
        {
            get => m_BoardId;
            set => m_BoardId = value;
        }

    }
}
