using Denifia.Stardew.SendLetters.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Domain
{

    public class PlayerRepository : IPlayerRepository
    {
        private readonly IRepository _repository;

        public PlayerRepository(IRepository repository)
        {
            _repository = repository;
        }

        public Player AddOrUpdate(Player player)
        {
            return _repository.AddOrUpdate(player);
        }

        public async Task<Player> AddOrUpdateAsync(Player player)
        {
            return await _repository.AddOrUpdateAsync(player);
        }

        public void Delete(Player player)
        {
            _repository.Delete(player);
        }

        public async Task DeleteAsync(Player player)
        {
            await _repository.DeleteAsync(player);
        }

        public List<Player> GetAll()
        {
            return _repository.GetAll<Player>();
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            return await _repository.GetAllAsync<Player>();
        }
    }
}
