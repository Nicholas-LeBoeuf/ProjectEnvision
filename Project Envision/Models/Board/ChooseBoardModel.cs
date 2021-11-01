using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class ChooseBoardModel : BoardItems
    {
        public static List<int> m_Boardidlist;
        public static List<string> m_Boardlist;
        public static List<string> m_BoardDesclist;

        public void SetBoardidlistListAttr(List<int> Boardidlist)
        {
            m_Boardidlist = Boardidlist;
        }

        public void SetBoardlistListAttr(List<string> Boardlist)
        {
            m_Boardlist = Boardlist;
        }

        public void SetBoardDesclistListAttr(List<string> BoardDesclist)
        {
            m_BoardDesclist = BoardDesclist;
        }

    }
}
