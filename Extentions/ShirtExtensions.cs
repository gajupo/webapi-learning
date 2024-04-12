using webapi_learning.Models;

namespace webapi_learning.Extentions
{
    public static class ShirtExtensions
    {
        public static PagedShirts ToPagedResult(this List<Shirt> shirts, int pageNumber, int pageSize, int totalCount) {
            var response = new PagedShirts()
            {
                TotalCount= totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = shirts
            };

            return response;
        }
    }
}
