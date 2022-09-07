using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dtos
{
    public class RegisterUserInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string UserName { get; set; }
        
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}