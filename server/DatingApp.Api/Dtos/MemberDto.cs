namespace DatingApp.Api.Dtos
{
    public record MemberDto
    {
        public Guid Id { get; init; }
        public string UserName { get; init; }
        public string PhotoUrl { get; set; }
        public int Age { get; init; }
        public string KnownAs { get; init; }
        public DateTime Dob { get; init; }
        public string Introduction { get; init; }
        public string Gender { get; init; }
        public string Country { get; init; }
        public string City { get; init; }
        public string Interests { get; init; }
        public string LookingFor { get; init; }
        public DateTimeOffset LastActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public IEnumerable<PhotoDto> Photos { get; init; }
    }
}