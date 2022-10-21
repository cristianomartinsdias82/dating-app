using AutoMapper;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using DatingApp.Api.Helpers;
using DatingApp.Api.Extensions;

namespace DatingApp.Api.Data.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly DatingAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(
            DatingAppDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext
                    .Users
                    .Include(x => x.Photos)
                    .ToListAsync(cancellationToken);

        public async Task<AppUser> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext
                    .Users
                    .Include(x => x.Photos)
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task<AppUser> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        => await _dbContext
                    .Users
                    .Include(x => x.Photos)
                    .SingleOrDefaultAsync(x => x.UserName == userName, cancellationToken);

        public async Task<MemberDto> GetMemberByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext
                    .Users
                    //.Include(x => x.Photos)
                    .Where(x => x.Id == id)
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
                    
        public async Task<MemberDto> GetMemberByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        => await _dbContext
                    .Users
                    //.Include(x => x.Photos)
                    .Where(x => x.UserName == userName)
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<MemberDto>> GetAllMembersAsync(CancellationToken cancellationToken = default)
        => await _dbContext
                    .Users
                    //.Include(x => x.Photos)
                    .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

        public async Task<PagedList<MemberDto>> GetPagedMembersAsync(
            QueryParams queryParams,
            string currentlyLoggedInUserName,
            bool includeLoggedInUserInResults = false,
            CancellationToken cancellationToken = default)
        {
            if (!includeLoggedInUserInResults && string.IsNullOrWhiteSpace(currentlyLoggedInUserName))
                throw new ArgumentException($"Argument {nameof(currentlyLoggedInUserName)} must be informed.");

            var query = _dbContext
                            .Users
                            .AsQueryable();

            query = AddFilterExpressions(query, queryParams, currentlyLoggedInUserName, includeLoggedInUserInResults);
            query = AddOrderByExpressions(query, queryParams);
            
            return await PagedList<MemberDto>.CreateAsync(
                query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                     .AsNoTracking(),
                queryParams.PageNumber,
                queryParams.PageSize,
                cancellationToken);
        }

        private static IQueryable<AppUser> AddFilterExpressions(
            IQueryable<AppUser> query,
            QueryParams queryParams,
            string currentlyLoggedInUserName,
            bool includeLoggedInUserInResults = false)
        {
            if (!includeLoggedInUserInResults)
                query = query.Where(x => x.UserName != currentlyLoggedInUserName);

            if (queryParams.Gender.HasValue)
                query = query.Where(x => x.Gender == queryParams.Gender.Value);

            if (queryParams.MinAge.HasValue)
                query = query.Where(x => x.Dob.Date <= DateTime.Today.AddYears(-queryParams.MinAge.Value));

            if (queryParams.MaxAge.HasValue)
                query = query.Where(x => x.Dob.Date >= DateTime.Today.AddYears(-queryParams.MaxAge.Value - 1));

            return query;
        }

        private static IQueryable<AppUser> AddOrderByExpressions(IQueryable<AppUser> query, QueryParams queryParams)
        {
            if (string.IsNullOrWhiteSpace(queryParams.SortColumn))
                return query;
            
            bool isAscending = (queryParams.SortDirection?.ToUpperInvariant() ?? "ASC") == "ASC";

            switch (queryParams.SortColumn.ToUpperInvariant())
            {
                case "CITY":
                    query = query.OrderBy(x => x.City, isAscending).ThenBy(x => x.KnownAs);
                    break;
                case "COUNTRY":
                    query = query.OrderBy(x => x.Country, isAscending).ThenBy(x => x.KnownAs);
                    break;
                case "DOB":
                    query = query.OrderBy(x => x.Dob, !isAscending).ThenBy(x => x.KnownAs);
                    break;
                case "GENDER":
                    query = query.OrderBy(x => x.Gender, isAscending).ThenBy(x => x.KnownAs);
                    break;
                case "LASTACTIVE":
                    query = query.OrderBy(x => x.LastActive, isAscending).ThenBy(x => x.KnownAs);
                    break;
                case "NEWEST":
                    query = query.OrderBy(x => x.CreatedAt, isAscending).ThenBy(x => x.KnownAs);
                    break;
                case "KNOWNAS":
                default:
                    query = query.OrderBy(x => x.KnownAs, isAscending);
                    break;
            }

            return query;
        }

        public async Task<PagedList<MemberDto>> GetPagedMembersAsync(
            QueryParams queryParams,
            CancellationToken cancellationToken = default)
        {
            var usersQueryable = _dbContext
                        .Users
                        //.Include(x => x.Photos)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .AsNoTracking();
            
            return await PagedList<MemberDto>.CreateAsync(
                usersQueryable,
                queryParams.PageNumber,
                queryParams.PageSize);
        }

        public async Task<bool> SaveAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken) > 0;

        public void Update(AppUser user)
        => _dbContext.Entry(user).State = EntityState.Modified;
    }
}