using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class BoardModel : BoardItems
    {
        public static List<string> m_Tasklist;
        public static List<string> m_TaskLocationlist;
        public static List<int> m_TaskIdlist;
        public static bool m_gotTask;

        public void SetTasklistListAttr(List<string> Tasklist)
        {
            m_Tasklist = Tasklist;
        }

        public void SetTaskLocationlistListAttr(List<string> TaskLocationlist)
        {
            m_TaskLocationlist = TaskLocationlist;
        }

        public void SetTaskIdlistListAttr(List<int> TaskIdlist)
        {
            m_TaskIdlist = TaskIdlist;
        }

        public bool gotTask
        {
            get => m_gotTask;
            set => m_gotTask = value;
        }

    }
}
