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

        
        public int PageSize { get; set; }

        
        public int PageNumber { get; set; }
    }
}
