using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class boardModel : boardItems
    {
        public static List<string> m_TaskList;
        public static List<string> m_TaskDescriptList;
        public static List<string> m_TaskLocationList;
        public static List<int> m_TaskIdList;
        public static List<string> m_AssigneeList;
        public static List<int> m_TaskPointsList;
        
        public static bool m_GotTask;
        public static bool m_ReturnToBoard;
        public static bool m_MemberReturn;

        public bool memberReturn
        {
            get => m_MemberReturn;
            set => m_MemberReturn = value;
        }

        public bool returnToBoard
        {
            get => m_ReturnToBoard;
            set => m_ReturnToBoard = value;
        }

        public void setAsigneeListAttr(List<string> assigneeList)
        {
            m_AssigneeList = assigneeList;
        }

        public void setTaskPointsListAttr(List<int> taskPointslist)
        {
            m_TaskPointsList = taskPointslist;
        }

        public void setTaskListAttr(List<string> taskList)
        {
            m_TaskList = taskList;
        }

        public void setTaskLocationListAttr(List<string> taskLocationList)
        {
            m_TaskLocationList = taskLocationList;
        }

        public void setTaskIdListAttr(List<int> taskIdList)
        {
            m_TaskIdList = taskIdList;
        }

        public bool gotTask
        {
            get => m_GotTask;
            set => m_GotTask = value;
        }

        public void setTaskDescriptListAttr(List<string> taskDescriptList)
        {
            m_TaskDescriptList = taskDescriptList;
        }

        public static List<int> m_SprintIdList;
        public static bool m_GotSprint;

        public void setSprintIdListAttr(List<int> sprintIdList)
        {
            m_SprintIdList = sprintIdList;
        }

        public bool gotSprint
        {
            get => m_GotSprint;
            set => m_GotSprint = value;
        }

        public static List<string> m_UsernameList;
        public static string m_CreatorUsername;
        public static bool m_GotUsers;

        public void setUsernameListAttr(List<string> usernameList)
        {
            m_UsernameList = usernameList;
        }

        public void setCreatorUsername(string username)
        {
            m_CreatorUsername = username;
        }

        public bool gotUsers
        {
            get => m_GotUsers;
            set => m_GotUsers = value;
        }

    }
}