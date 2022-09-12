using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dtos
{
    public class RegisterUserInputModel
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}