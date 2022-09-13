using DatingApp.Api.Data.Persistence;
using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [Authorize]
    public class UsersController : DatingAppController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        [HttpGet("username/{userName}")]
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
    }
}