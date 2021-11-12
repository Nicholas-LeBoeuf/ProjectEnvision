using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class BoardModel : BoardItems
    {
        public static List<string> m_Tasklist;
        public static List<string> m_Taskdescriptlist;
        public static List<string> m_TaskLocationlist;
        public static List<int> m_TaskIdlist;
        public static List<string> m_Assigneelist;
        public static List<int> m_TaskPointslist;
        public static bool m_gotTask;

        public void SetAsigneeListAttr(List<string> Assigneelist)
        {
            m_Assigneelist = Assigneelist;
        }

        public void SetTaskPointsListAttr(List<int> TaskPointslist)
        {
            m_TaskPointslist = TaskPointslist;
        }

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

        public static List<string> m_locationlist;

        public void SetTaskDescriptlistListAttr(List<string> Taskdescriptlist)
        {
            m_Taskdescriptlist = Taskdescriptlist;
        }

        public static List<string> m_sprintNamelist;
        public static List<int> m_sprintIdlist;
        public static bool m_gotSprint;
        
        public void SetsprintNamelistListAttr(List<string> sprintNamelist)
        {
            m_sprintNamelist = sprintNamelist;
        }

        public void SetsprintIdlistListAttr(List<int> sprintIdlist)
        {
            m_sprintIdlist = sprintIdlist;
        }

        public bool gotSprint
        {
            get => m_gotSprint;
            set => m_gotSprint = value;
        }

        public static List<string> m_usernamelist;
        public static bool m_gotusers;

        public void SetusernamelistListAttr(List<string> usernamelist)
        {
            m_usernamelist = usernamelist;
        }

        public bool gotusers
        {
            get => m_gotusers;
            set => m_gotusers = value;
        }



    }
}