using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Common
{
    public class SortOrder
    {
        public string OrderBy { get; set; } = "DateCreated";
        public string OrderDirection { get; set; } = "asc";
    }
}
