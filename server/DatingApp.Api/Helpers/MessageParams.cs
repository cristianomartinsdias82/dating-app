namespace DatingApp.Api.Helpers
{
    public sealed record MessageParams : PaginationParameters
    {
        public Guid UserId {get; set;}
        public MessageContainers Container {get; init;} = MessageContainers.Unread;
    }
}