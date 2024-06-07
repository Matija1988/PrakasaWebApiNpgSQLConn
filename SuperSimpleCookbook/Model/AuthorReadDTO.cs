using System.ComponentModel.DataAnnotations;

namespace SuperSimpleCookbook.Model
{
    public class AuthorReadDTO
    {
        public Guid Uuid { get; set; }
        public string FirstName { get; set; }
      
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Bio { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
