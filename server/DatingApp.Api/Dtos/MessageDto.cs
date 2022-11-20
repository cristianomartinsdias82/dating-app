namespace DatingApp.Api.Dtos
{
    public sealed record MessageDto
    {
        public Guid Id { get; init; }
        public Guid SenderId { get; init; }
        public string SenderUserName { get; init; }
        public string SenderKnownAs { get; init; }
        public string SenderPhotoUrl { get; init; }
        public Guid RecipientId { get; init; }
        public string RecipientUserName { get; init; }
        public string RecipientKnownAs { get; init; }
        public string RecipientPhotoUrl { get; init; }
        public string Content { get; init; }
        public DateTime DateSent { get; init; }
        public DateTime? DateRead { get; init; }
    }
}