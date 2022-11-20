namespace DatingApp.Api.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public AppUser Sender { get; set; }
        public Guid SenderId { get; set; }
        public string SenderUserName { get; set; }
        public AppUser Recipient { get; set; }
        public Guid RecipientId { get; set; }
        public string RecipientUserName { get; set; }
        public string Content { get; set; }
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
        public DateTime? DateRead { get; set; }
        public bool DeletedBySender { get; set; }
        public bool DeletedByRecipient { get; set; }
    }
}