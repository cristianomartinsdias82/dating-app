using System.ComponentModel.DataAnnotations;
using DatingApp.Api.Entities;

namespace DatingApp.Api.Helpers
{
    public record QueryParams
    (
        [Range(1, 999999)]
        int PageNumber,

        [Range(1, 50)]
        int PageSize,

        Gender? Gender,

        [Range(18, 999999)]
        int? MinAge,

        int? MaxAge,

        string SortColumn,

        string SortDirection
    );
}