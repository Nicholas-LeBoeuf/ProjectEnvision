using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class BoardItems
    {

        public static string m_Board_Name;
        public static int m_Boardid;
        public static bool m_gotBoard;

        public string Board_Name
        {
            get => m_Board_Name;
            set => m_Board_Name = value;
        }
        public int BoardId
        {
            get => m_Boardid;
            set => m_Boardid = value;
        }

        public bool gotBoard
        {
            get => m_gotBoard;
            set => m_gotBoard = value;
        }
    }
}
