namespace SuperSimpleCookbook.Model
{
    public class PagedList<T> where T : class
    {
        public  List<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int Pagenumber { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

    }
}
