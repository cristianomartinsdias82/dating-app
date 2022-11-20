using System.ComponentModel.DataAnnotations;
using DatingApp.Api.Entities;

namespace DatingApp.Api.Helpers
{
    public sealed record QueryParams : PaginationParameters
    {
        public Gender? Gender {get;init;}

        [Range(18, 999999)]
        public int? MinAge {get;init;}

        public int? MaxAge {get;init;}

        public string SortColumn {get;init;}

        public string SortDirection {get;init;} = "asc";
    }
}