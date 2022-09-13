namespace DatingApp.Api.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public AppUser AppUser {get;set;}
        public Guid AppUserId {get;set;}
        // public string ThumbnailUrl { get; set; }
        public string Url { get; set; }
    }
}