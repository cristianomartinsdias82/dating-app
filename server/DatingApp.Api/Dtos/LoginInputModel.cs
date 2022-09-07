using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.Dtos
{
    public class LoginInputModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}