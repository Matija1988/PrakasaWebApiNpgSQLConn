using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Model.Model
{
    public class ServiceResponse<T>
    {
        public T Items { get; set; }
        public bool Success { get; set; }   
        public string? Message { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public bool HasNextPage { get; set; }

         public bool HasPreviousPage { get; set; }
    }
}
