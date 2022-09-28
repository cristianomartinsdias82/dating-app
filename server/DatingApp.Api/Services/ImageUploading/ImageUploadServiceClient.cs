using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Api.Dtos;
using DatingApp.Api.Settings;

namespace DatingApp.Api.Services.ImageUploading
{
    public sealed class CloudinaryImageUploadServiceClient : IImageUploadServiceClient
    {
        private readonly ILogger<CloudinaryImageUploadServiceClient> _logger;
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageUploadServiceClient(
            ILogger<CloudinaryImageUploadServiceClient> logger,
            ImageUploadExternalProviderSettings providerSettings)
        {
            _logger = logger;
            
            _cloudinary = new Cloudinary(new Account
            {
                Cloud = providerSettings.ApiCloudName,
                ApiKey = providerSettings.ApiKey,
                ApiSecret = providerSettings.ApiSecret
            });
            _cloudinary.Api.Secure = true;
        }

        public async Task<ImageUploadResult> UploadAsync(
            MemberDto member,
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            if ((file?.Length ?? 0) == 0)
                throw new ArgumentException("A file must be informed!");

            var uploadResult = default(ImageUploadResult);

            try
            {
                using var fs = file.OpenReadStream();

                uploadResult = await _cloudinary.UploadAsync(
                    new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, fs),
                        Transformation = new Transformation()
                            .Height(500)
                            .Width(500)
                            .Crop("fill")
                            .Gravity("face")
                    },
                    cancellationToken);

                if (uploadResult?.Error is not null)
                    _logger.LogError(
                        "Error when attempting to upload image to image upload service provider: {Message}",
                        uploadResult.Error.Message);
            }
            catch (Exception) {}

            return uploadResult;
        }

        public async Task<DeletionResult> DeleteAsync(
            string publicId,
            CancellationToken cancellationToken = default)
        {
            var deletionResult = default(DeletionResult);

            try
            {
                deletionResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));
            }
            catch (Exception){ }

            return deletionResult;
        }
    }
}