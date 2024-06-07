using System.ComponentModel.DataAnnotations;

namespace SuperSimpleCookbook.Model
{
    public class AuthorReadDTO
    {
       
        public string FirstName { get; set; }
      
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Bio { get; set; }
    }
}
