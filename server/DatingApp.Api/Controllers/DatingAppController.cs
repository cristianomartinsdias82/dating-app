using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public abstract class DatingAppController : ControllerBase
    {
        
    }
}