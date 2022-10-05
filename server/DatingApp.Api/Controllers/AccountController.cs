using System.Reflection.Metadata;
using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using DatingApp.Api.Services.AuthTokenIssuing;
using DatingApp.Api.Services.Hashing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers
{
    public class AccountController : DatingAppController
    {
        private readonly DatingAppDbContext _dbContext;
        private readonly IHashProvider _hashProvider;
        private readonly IAuthTokenIssuing _tokenIssuer;
        private readonly IMapper _mapper;

        public AccountController(
            DatingAppDbContext dbContext,
            IHashProvider hashProvider,
            IAuthTokenIssuing tokenIssuer,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _hashProvider = hashProvider;
            _tokenIssuer = tokenIssuer;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(
            RegisterUserInputModel model,
            CancellationToken cancellationToken)
        {
            if (await CheckUserNameForExistenceAsync(
                model.UserName,
                _dbContext,
                cancellationToken))
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "User name is already taken." });

            var salt = default(byte[]);
            var passwordHash = _hashProvider.ComputeUtf8EncodedHash(model.Password, out salt);

            var newUser = _mapper.Map<AppUser>(model);
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = salt;
   
            await _dbContext.Users.AddAsync(newUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return StatusCode(
                    StatusCodes.Status201Created,
                    new UserDto
                    {
                        UserName = newUser.UserName,
                        Token = _tokenIssuer.IssueToken(newUser),
                        KnownAs = model.KnownAs
                    });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(
            LoginInputModel model,
            CancellationToken cancellationToken)
        {
            var user = await _dbContext
                .Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName == model.UserName);

            if (user is null)
                return StatusCode(StatusCodes.Status404NotFound, new { message = "User not found." });

            var hash = _hashProvider.ComputeUtf8EncodedHash(model.Password, user.PasswordSalt);
            var base64_1 = Convert.ToBase64String(hash);
            var base64_2 = Convert.ToBase64String(user.PasswordHash);
            if (!base64_1.SequenceEqual(base64_2))
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Incorrect user and/or password." });

            return StatusCode(
                    StatusCodes.Status200OK,
                    new UserDto
                    {
                        UserName = user.UserName,
                        Token = _tokenIssuer.IssueToken(user),
                        KnownAs = user.KnownAs,
                        PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url
                    });
        }

        [HttpPost("check-username-availability/{userName}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckUserNameAvailability(
            string userName,
            CancellationToken cancellationToken)
        {
            var userNameAlreadyTaken = await CheckUserNameForExistenceAsync(
                userName,
                _dbContext,
                cancellationToken);

            return StatusCode(
                    StatusCodes.Status200OK,
                    !userNameAlreadyTaken);
        }

        private static async Task<bool> CheckUserNameForExistenceAsync(
            string userName,
            DatingAppDbContext dbContext,
            CancellationToken cancellationToken = default)
            => await dbContext.Users.AnyAsync(x => x.UserName.ToUpper() == userName.ToUpper(), cancellationToken);
    }
}