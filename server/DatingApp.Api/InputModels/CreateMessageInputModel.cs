
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.InputModels
{
    public sealed record CreateMessageInputModel(
            [Required] Guid RecipientId,
            [Required] string Content);
}