using System.Collections.Generic;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Domain
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllAsync();
        List<Player> GetAll();

        Task<Player> AddOrUpdateAsync(Player player);
        Player AddOrUpdate(Player player);

        Task DeleteAsync(Player player);
        void Delete(Player player);
    }
}
