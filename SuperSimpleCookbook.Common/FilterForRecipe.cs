using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleCookbook.Common
{
    public class FilterForRecipe
    {
        public int? Id { get; set; } = null;

        public decimal? MinPrice { get; set; } = null;

        public decimal? MaxPrice { get; set; } = null;

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public Guid? AuthorGuid { get; set; } = null;

        public string? SearchQuery { get; set; } = string.Empty;


    }
}
