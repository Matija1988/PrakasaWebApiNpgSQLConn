using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Common
{
    public class Paging
    {

        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid integer value: 1 or above ")]
        public int PageSize { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="Please enter valid integer value: 1 or above ")]
        public int PageNumber { get; set; }
    }
}
