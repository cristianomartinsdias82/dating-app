using System.Text.Json;

namespace DatingApp.Api.Extensions
{

    public static class HttpExtensions
    {
        public static void AddPaginationHeader(
            this HttpResponse response,
            int itemCount,
            int pageCount,
            int pageSize,
            int pageNumber)
        {
            response.Headers.Add("Pagination", JsonSerializer.Serialize(new { itemCount, pageCount, pageSize, pageNumber }));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}