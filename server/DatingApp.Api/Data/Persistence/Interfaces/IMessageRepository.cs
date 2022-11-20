using DatingApp.Api.Dtos;
using DatingApp.Api.Entities;
using DatingApp.Api.Helpers;

namespace DatingApp.Api.Data.Persistence
{
    public interface IMessageRepository
    {
        Task<PagedList<MessageDto>> GetPaginatedAsync(
            MessageParams messageParams,
            CancellationToken cancellationToken = default);

        Task<Message> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<MessageDto>> GetMessageThreadAsync(
            Guid currentUserId,
            Guid recipientUserId,
            CancellationToken cancellationToken = default
        );

        void Add(Message message);

        void Update(Message message);

        void Delete(Message message);

        Task<bool> SaveAllAsync(CancellationToken cancellationToken = default);
    }
}