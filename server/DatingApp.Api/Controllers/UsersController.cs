using System.Net.Mime;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers
{
    public class UsersController : DatingAppController
    {
        private readonly DatingAppDbContext _dbContext;

        public UsersController(DatingAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> Get(CancellationToken cancellationToken)
            => Ok(await _dbContext.Users.ToListAsync(cancellationToken));

        [Authorize]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppUser), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FindAsync(new object[]{ id }, cancellationToken);

            if (user is null)
                return NotFound();

            return Ok(user);
        }
    }
}