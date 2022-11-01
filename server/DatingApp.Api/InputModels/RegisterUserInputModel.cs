using System.ComponentModel.DataAnnotations;

namespace DatingApp.Api.InputModels
{
    public sealed record RegisterUserInputModel
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string UserName { get; init; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; init; }

        [Required]
        [Range(typeof(bool), "true", "true")]
        public bool TermsAccepted { get; init; }

        [Required]
        public string KnownAs { get; init; }

        [Required]
        public DateTime Dob { get;init; }

        [Required]
        public string City { get; init; }

        [Required]
        public string Country { get; init; }

        [Required]
        public string Gender { get; init; }
    }
}