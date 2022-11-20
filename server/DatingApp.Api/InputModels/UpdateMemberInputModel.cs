namespace DatingApp.Api.InputModels
{
    public sealed record UpdateMemberInputModel
    (
        string Introduction,
        string Country,
        string City,
        string Interests,
        string LookingFor
    );
}