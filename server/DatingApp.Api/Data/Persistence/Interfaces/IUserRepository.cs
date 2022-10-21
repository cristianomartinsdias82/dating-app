using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using DatingApp.Api.Helpers;

namespace DatingApp.Api.Data.Persistence
{
    public interface IUserRepository
    {
         void Update(AppUser user);
         Task<bool> SaveAllAsync(CancellationToken cancellationToken = default);
         Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken cancellationToken = default);
         Task<AppUser> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
         Task<AppUser> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

         Task<IEnumerable<MemberDto>> GetAllMembersAsync(CancellationToken cancellationToken = default);
         Task<PagedList<MemberDto>> GetPagedMembersAsync(
            QueryParams queryParams,
            string currentlyLoggedInUserName,
            bool includeLoggedInUserInResults = false,
            CancellationToken cancellationToken = default);
            
         Task<MemberDto> GetMemberByIdAsync(Guid id, CancellationToken cancellationToken = default);
         Task<MemberDto> GetMemberByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    }
}