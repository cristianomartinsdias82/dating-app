using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using DatingApp.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data.Persistence
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DatingAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public MessageRepository(
            DatingAppDbContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public void Add(Message message)
        {
            _dbContext.Messages.Add(message);
        }

        public void Delete(Message message)
        {
            _dbContext.Messages.Remove(message);
        }

        public async Task<Message> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
            => await _dbContext.Messages.FindAsync(new object[] { id }, cancellationToken: cancellationToken);

        public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(
            Guid currentUserId,
            Guid recipientUserId,
            CancellationToken cancellationToken = default)
        {
            var messages = await _dbContext.Messages
                .Include(x => x.Sender).ThenInclude(x => x.Photos)
                .Include(x => x.Recipient).ThenInclude(x => x.Photos)
                .Where(x => (x.Recipient.Id == currentUserId && x.Sender.Id == recipientUserId && !x.DeletedByRecipient)
                            ||
                            (x.Sender.Id == currentUserId && x.Recipient.Id == recipientUserId && !x.DeletedBySender))
                .OrderBy(x => x.DateSent)
                .ToListAsync(cancellationToken);

            var unreadByRecipientMessages = messages
                .Where(x => x.Recipient.Id == currentUserId && x.DateRead == null);

            if (unreadByRecipientMessages?.Any() ?? false)
            {
                var dateRead = DateTime.UtcNow;

                //Mark all recipient messages as read
                unreadByRecipientMessages.ToList().ForEach(unreadMessage => unreadMessage.DateRead = dateRead);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public async Task<PagedList<MessageDto>> GetPaginatedAsync(
            MessageParams messageParams,
            CancellationToken cancellationToken = default)
        {
            var queryable = _dbContext
                            .Messages
                            .AsNoTracking()
                            .Include(x => x.Recipient)
                            .Include(x => x.Sender)
                            .OrderByDescending(x => x.DateSent)
                            .Where(x => !x.DeletedByRecipient && !x.DeletedBySender);

            queryable = messageParams.Container
            switch
            {
                /*Messages that current logged in user received */
                MessageContainers.Inbox => queryable.Where(u => u.Recipient.Id == messageParams.UserId && !u.DeletedByRecipient),
                
                /*Messages that current logged in user sent */
                MessageContainers.Outbox => queryable.Where(u => u.Sender.Id == messageParams.UserId && !u.DeletedBySender),
                
                /*Messages that current logged in user received and are not yet marked as read*/
                _ => queryable.Where(u => u.Recipient.Id == messageParams.UserId && u.DateRead == null && !u.DeletedByRecipient)
            };
            
            return await PagedList<MessageDto>.CreateAsync(
                queryable.ProjectTo<MessageDto>(_mapper.ConfigurationProvider),
                messageParams,
                cancellationToken);
        }

        public void Update(Message message)
        {
            if (message.DeletedByRecipient && message.DeletedBySender)
                Delete(message);
            else
                _dbContext.Entry(message).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}