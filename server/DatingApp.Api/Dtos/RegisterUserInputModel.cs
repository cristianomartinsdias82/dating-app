using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dtos
{
    public record RegisterUserInputModel
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string UserName { get; init; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; init; }
    }
}