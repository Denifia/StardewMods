using denifia.stardew.sendletters.Domain;
using StardewValley;
using System;
using System.Linq;

namespace denifia.stardew.sendletters.Services
{
    public class PlayerService : IPlayerService
    {
        private Player _currentPlayer;

        private readonly IRepository _repository;
        private readonly IConfigurationService _configService;

        public PlayerService(IRepository repository, IConfigurationService configService)
        {
            _repository = repository;
            _configService = configService;
        }

        public Player GetCurrentPlayer()
        {
            return _currentPlayer;
        }

        public Player GetPlayerById(string id)
        {
            return _repository.FindPlayers(x => x.Id == id).FirstOrDefault();
        }

        public void LoadCurrentPlayer()
        {
            var name = Game1.player.name;
            var farmName = Game1.player.farmName;

            var matchingPlayers = _repository.FindPlayers(x => x.Name == name && x.FarmName == farmName);
            if (matchingPlayers.Any())
            {
                _currentPlayer = matchingPlayers.First();
                _configService.CurrentPlayerId = _currentPlayer.Id;
            }
            else
            {
                throw new Exception("couldn't find current player!");
            }
        }     
    }
}
