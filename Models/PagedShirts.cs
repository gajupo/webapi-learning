namespace webapi_learning.Models
{
    public class PagedShirts
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<Shirt>? Items { get; set; }
    }
}
