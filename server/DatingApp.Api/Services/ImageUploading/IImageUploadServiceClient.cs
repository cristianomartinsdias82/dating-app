using CloudinaryDotNet.Actions;
using DatingApp.Api.Dtos;

namespace DatingApp.Api.Services.ImageUploading
{
    public interface IImageUploadServiceClient
    {
         Task<ImageUploadResult> UploadAsync(
            MemberDto member,
            IFormFile file,
            CancellationToken cancellationToken = default);

        Task<DeletionResult> DeleteAsync(
            string publicId,
            CancellationToken cancellationToken = default);
    }
}