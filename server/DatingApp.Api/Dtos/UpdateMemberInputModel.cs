namespace DatingApp.Api.Dtos
{
    public record UpdateMemberInputModel
    {   
        public string Introduction { get; init; }
        
        public string Country { get; init; }
        
        public string City { get; init; }
        
        public string Interests { get; init; }
        
        public string LookingFor { get; init; }
    }
}