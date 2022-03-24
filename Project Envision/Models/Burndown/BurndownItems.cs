using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Envision.Models
{
    public class BurndownItems
    {
        public static List<int> m_SprintIds;
        public static List<string> m_SprintNames;
        public static List<int> m_SprintPoints;
        public static List<string> m_SprintDates;


        public List<int> SprintIds
        {
            get => m_SprintIds;
            set => m_SprintIds = value;
        }

        public List<string> SprintNames
        {
            get => m_SprintNames;
            set => m_SprintNames = value;
        }

        public List<int> SprintPoints
        {
            get => m_SprintPoints;
            set => m_SprintPoints = value;
        }

        public List<string> SprintDates
        {
            get => m_SprintDates;
            set => m_SprintDates = value;
        }
    }
}
