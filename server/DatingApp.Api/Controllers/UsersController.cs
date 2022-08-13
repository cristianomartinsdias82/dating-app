using System.Net.Mime;
using DatingApp.Api.Data;
using DatingApp.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsersController : ControllerBase
    {
        private readonly DatingAppDbContext _dbContext;

        public UsersController(DatingAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AppUser>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async ValueTask<IActionResult> Get(CancellationToken cancellationToken)
            => Ok(await _dbContext.Users.ToListAsync(cancellationToken));

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