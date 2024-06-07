using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Common
{
    public class CombineTemp
    {
        public FilterForAuthor? Filter { get; set; }

        public Paging? Paging { get; set; }

        public SortOrder? Sort { get; set; }
    }
}
