using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class boardItems
    {
        public static bool m_GotBoard;
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

        public int boardid
        {
            get => m_BoardId;
            set => m_BoardId = value;
        }

    }
}
