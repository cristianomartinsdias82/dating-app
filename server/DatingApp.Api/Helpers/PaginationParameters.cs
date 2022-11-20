using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Helpers
{
    public record PaginationParameters
    {
        [Range(1, 999999)]
        public int PageNumber {get;init;}

        [Range(1, 50)]
        public int PageSize {get;init;}
    }
}