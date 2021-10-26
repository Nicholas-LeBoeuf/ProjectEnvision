using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class BoardItems
    {
        public static bool m_gotBoard;
        public static int m_Boardid;



        public bool gotBoard
        {
            get => m_gotBoard;
            set => m_gotBoard = value;
        }

        public int Boardid
        {
            get => m_Boardid;
            set => m_Boardid = value;
        }

    }
}
