using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Common
{
    public class Paging
    {
        public int PageSize { get; set; } = 10;

        public int PageFirstIndex { get; set; }
        public int PageNumber { get; set; } = 1;
    }
}
