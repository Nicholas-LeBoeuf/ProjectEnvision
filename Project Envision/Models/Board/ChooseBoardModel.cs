using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class ChooseBoardModel : boardItems
    {
        public static List<int> m_BoardIdList;
        public static List<string> m_BoardList;
        public static List<string> m_BoardDescList;
        public static int m_CreatedBoardNum;

        public int createdBoardNum
        {
            get => m_CreatedBoardNum;
            set => m_CreatedBoardNum = value;
        }

        public void setBoardIdListAttr(List<int> boardIdList)
        {
            m_BoardIdList = boardIdList;
        }

        public void setBoardListAttr(List<string> boardList)
        {
            m_BoardList = boardList;
        }

        public void setBoardDescListAttr(List<string> boardDescList)
        {
            m_BoardDescList = boardDescList;
        }

    }
}
