using AutoMapper;
using DatingApp.Api.Data.Persistence;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Api.Extensions;
using DatingApp.Api.Services.ImageUploading;

namespace DatingApp.Api.Controllers
{
    [Authorize]
    public class UsersController : DatingAppController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IImageUploadServiceClient _imageUploadServiceClient;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserRepository userRepository,
            IMapper mapper,
            IImageUploadServiceClient imageUploadServiceClient,
            ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _imageUploadServiceClient = imageUploadServiceClient;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> Get(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllMembersAsync(cancellationToken);

            return Ok(users);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetMemberByIdAsync(id, cancellationToken);

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("username/{userName}", Name = "GetUser")]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> GetByUserName(string userName, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetMemberByUserNameAsync(userName, cancellationToken);

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(
            UpdateMemberInputModel model,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByUserNameAsync(
                User.GetUserName(),
                cancellationToken);

            _mapper.Map(model, user); //Transfer arguments from source (model) to destination(user)!

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync(cancellationToken))
                return NoContent();

            return BadRequest("Failed to update user profile information.");
        }

        [HttpPost("add-photo")]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByUserNameAsync(
                User.GetUserName(),
                cancellationToken);

            var imageUploadResult = await _imageUploadServiceClient.UploadAsync(
                _mapper.Map<MemberDto>(user),
                file,
                cancellationToken);

            if (imageUploadResult.Error is not null)
                return Problem(
                    "Error while attempting to upload the photo.",
                    statusCode:StatusCodes.Status500InternalServerError);

            var newPhoto = new Photo
            {
                CreatedAt = DateTimeOffset.UtcNow,
                PublicId = imageUploadResult.PublicId,
                Url = imageUploadResult.SecureUrl.AbsoluteUri,
            };

            user.Photos ??= new List<Photo>();

            if (!user.Photos.Any())
                newPhoto.IsMain = true;

            user.Photos.Add(newPhoto);

            if (!await _userRepository.SaveAllAsync(cancellationToken))
                return Problem(
                    "Error while attempting to upload the photo.",
                    statusCode:StatusCodes.Status500InternalServerError);

            return CreatedAtRoute(
                "GetUser",
                new {userName = user.UserName},
                _mapper.Map<PhotoDto>(newPhoto));
        }

        [HttpPut("set-main-photo/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetMainPhoto(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAsync(User.GetUserName(), cancellationToken);

            var currentMainPhoto = user.Photos?.FirstOrDefault(x => x.IsMain);
            if ((currentMainPhoto?.Id ?? id) == id)
                return NoContent();

            var newMainPhoto = user.Photos?.FirstOrDefault(x => x.Id == id);
            if (newMainPhoto is not null)
            {
                currentMainPhoto.IsMain = false;
                newMainPhoto.IsMain = true;

                await _userRepository.SaveAllAsync(cancellationToken);
            }

            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePhoto(Guid photoId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAsync(User.GetUserName(), cancellationToken);

            var photo = user.Photos?.FirstOrDefault(x => x.Id == photoId);
            if (photo is null)
                return Ok();

            if (photo.IsMain)
                return BadRequest("The main photo cannot be deleted.");

            user.Photos.Remove(photo);
            if (!await _userRepository.SaveAllAsync(cancellationToken))
                return Problem(
                        "Error while attempting do delete the photo.",
                        statusCode: StatusCodes.Status500InternalServerError);

            if (!string.IsNullOrWhiteSpace(photo.PublicId))
            {
                var deletionResult = await _imageUploadServiceClient.DeleteAsync(photo.PublicId, cancellationToken);
                if (deletionResult.Error is not null)
                {
                    _logger.LogError("The external photo upload provider is returning error while attempting to delete photo {id} and public id {publicId}: {@error}. Status code: {statusCode}. Result: {result}",
                    photoId,
                    photo.PublicId,
                    deletionResult.Error,
                    deletionResult.StatusCode,
                    deletionResult.Result);

                    //What else are we supposed to do? Send the photo some dead letter queue for further deletion attempts?

                    return Problem(
                            "Error while attempting do delete the photo.",
                            statusCode: StatusCodes.Status500InternalServerError);
                }
            }

            return Ok();
        }
    }
}