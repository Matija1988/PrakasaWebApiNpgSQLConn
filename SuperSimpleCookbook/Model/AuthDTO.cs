using System.ComponentModel.DataAnnotations;

namespace SuperSimpleCookbook.Model
{
    public class AuthDTO
    {
        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters = 30")]
        public string Username { get; set; }

        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters = 30")]
        public string Password { get; set; }
    }
}
