
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    public class BuggyController : DatingAppController
    {
        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
            => NotFound();

        [Authorize]
        [HttpGet("secret")]
        public ActionResult<string> GetUnauthorized()
            => "secret";

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
            => ((string)null).ToString();

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
            => BadRequest();
    }
}