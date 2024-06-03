using System.ComponentModel.DataAnnotations;

namespace SuperSimpleCookbook.Model
{
    public class Author : IEntity
    {
        [Key]
        public int Id { get; set; }

        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Required, StringLength(30, ErrorMessage ="Maximum allowed number of characters = 30")]
        public string FirstName { get; set; }

        [Required, StringLength(30, ErrorMessage = "Maximum allowed number of characters = 30")]
        public string LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }


        [Required, StringLength(1200, ErrorMessage = "Maximum allowed number of characters = 1200")]
        public string Bio { get; set; }
       
        public bool IsActive { get; set; } = true;

        public DateOnly DateCreated { get; set; } = new DateOnly();

        public DateOnly DateUpdated { get; set; } = new DateOnly();

    }
}
