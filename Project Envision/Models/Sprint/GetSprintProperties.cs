using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class GetSprintProperties
    {
        public static int currentSprint_Id { get; set; }
        public void setCurrentSprint_Id(int currentId)
        {
            currentSprint_Id = currentId;
        }

        public static int getSprint_Id { get; set; }
        public void setGetSprintId(int getSprintId)
        {
            getSprint_Id = getSprintId;
        }

        public static string getSprint_Name { get; set; }
        public void setGetSprintName(string getSprintName)
        {
            getSprint_Name = getSprintName;
        }

        public static string getSprint_Description { get; set; }
        public void setSprintDescript(string getSprintDescript)
        {
            getSprint_Description = getSprintDescript;
        }

        public static string getSprint_Start { get; set; }
        public void setSprintStart(string getSprintStart)
        {
            getSprint_Start = getSprintStart;
        }

        public static string getSprint_End { get; set; }
        public void setSprintEnd(string getSprintEnd)
        {
            getSprint_End = getSprintEnd;
        }

        public static List<int> getSprint_IdList { get; set; }
        public void setGetSprintIdList(List<int> getSprintIdList)
        {
            getSprint_IdList = getSprintIdList;
        }

        public static List<string> getSprint_NameList { get; set; }
        public void setGetSprintNameList(List<string> getSprintNameList)
        {
            getSprint_NameList = getSprintNameList;
        }

        public static List<string> getSprint_DescriptionList { get; set; }
        public void setSprintDescriptList(List<string> getSprintDescriptList)
        {
            getSprint_DescriptionList = getSprintDescriptList;
        }

        public static List<string> getSprint_StartList { get; set; }
        public void setSprintStartList(List<string> getSprintStartList)
        {
            getSprint_StartList = getSprintStartList;
        }

        public static List<string> getSprint_EndList { get; set; }
        public void setSprintEndList(List<string> getSprintEndList)
        {
            getSprint_EndList = getSprintEndList;
        }

    }
}
