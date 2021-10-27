using System.ComponentModel.DataAnnotations;
namespace Project_Envision.Models
{
    public class CreateboardModel
    {
        [Required(ErrorMessage = "Required field. *")]
        public string board_name { get; set; }

        public string board_Description { get; set; }
    }
}