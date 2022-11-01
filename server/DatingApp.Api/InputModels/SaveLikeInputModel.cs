using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.InputModels
{
    public sealed record SaveLikeInputModel([Required] Guid LikedUserId);
}