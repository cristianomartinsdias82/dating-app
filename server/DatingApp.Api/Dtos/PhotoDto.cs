namespace DatingApp.Api.Dtos
{
    public record PhotoDto
    (
        Guid Id,
        string Url,
        bool IsMain
    );
}