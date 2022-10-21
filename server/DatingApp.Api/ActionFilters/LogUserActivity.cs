using DatingApp.Api.Data.Persistence;
using DatingApp.Api.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.Api.ActionFilters
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            //Runs before ths action itself
            //...

            var resultContext = await next();

            //Runs after
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)
                return;

            await LogUserLastActiveAsync(context);
        }

        private async Task LogUserLastActiveAsync(ActionExecutingContext context)
        {
            var userId = context.HttpContext.User.GetUserId();
            if (userId is null)
                return;

            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await userRepository.GetByIdAsync(userId.Value);
            if (user is null)
                return;

            user.LastActive = DateTimeOffset.UtcNow;

            await userRepository.SaveAllAsync();
        }
    }
}