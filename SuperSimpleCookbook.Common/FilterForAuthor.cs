using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Common
{
    public class FilterForAuthor
    {
        public Guid? Uuid { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; } 

        public DateTime? DateOfBirth { get; set; } 
      
        public DateTime? DateCreated { get; set; } 
    }
}
