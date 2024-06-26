using System.ComponentModel.DataAnnotations;

namespace SuperSimpleCookbook.Model
{
    public class AuthorUpdateDTO
    {
        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters = 30")]
        public string FirstName { get; set; }

        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters = 30")]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required, StringLength(1200, ErrorMessage = "Maximum allowed number of characters = 1200")]
        public string Bio { get; set; }

        public bool IsActive { get; set; }

        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters = 30")]
        public string Username { get; set; }

        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters = 30")]
        public string Password { get; set; }

        public int RoleId { get; set; }

    }
}
