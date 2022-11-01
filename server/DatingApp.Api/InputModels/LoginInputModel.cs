using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.InputModels
{
    public sealed record LoginInputModel([Required] string UserName, [Required] string Password);
}