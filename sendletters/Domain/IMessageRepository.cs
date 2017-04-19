using Denifia.Stardew.SendLetters.Common.Domain;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Domain
{
    public interface IMessageRepository
    {
        IQueryable<Message> GetMessagesToPlayer(string id);
        IQueryable<Message> GetMessagesFromPlayer(string id);

        Task<IEnumerable<Message>> GetAllAsync();
        List<Message> GetAll();

        Task<Message> AddOrUpdateAsync(Message message);
        Message AddOrUpdate(Message message);

        Task DeleteAsync(Message message);
        void Delete(Message message);
    }
}
