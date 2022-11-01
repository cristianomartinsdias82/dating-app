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

         Task<PagedList<MemberDto>> GetPagedLikerMembersForAsync(Guid id, QueryParams queryParams, CancellationToken cancellationToken = default); //Retrieves the users that liked the user whose id is {id}

         Task<PagedList<MemberDto>> GetPagedLikedByMembersForAsync(Guid id, QueryParams queryParams, CancellationToken cancellationToken = default); //Retrieves the users that were liked by user whose id is {id}

         Task<bool> SaveToggleLike(UserLike userLike, CancellationToken cancellationToken = default);

         Task<bool> MemberHasLikeAsync(Guid likerId, Guid likedId, CancellationToken cancellationToken = default);
    }
}