using System.ComponentModel.DataAnnotations;
namespace Project_Envision.Models
{
    public class CreateboardModel
    {    
        public int user_id  { get; set; }

        public int sprint_id { get; set; }

        public string board_name { get; set; }

        public string username { get; set; }
    }
}