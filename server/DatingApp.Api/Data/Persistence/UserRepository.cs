using AutoMapper;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

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

        public async Task<bool> SaveAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken) > 0;

        public void Update(AppUser user)
        => _dbContext.Entry(user).State = EntityState.Modified;
    }
}