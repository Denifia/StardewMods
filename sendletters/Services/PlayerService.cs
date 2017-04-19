using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Domain;
using StardewValley;
using System;
using System.Linq;

namespace Denifia.Stardew.SendLetters.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository _repository;
        private readonly IConfigurationService _configurationService;

        private Player _currentPlayer;
        public Player CurrentPlayer { get
            {
                return LoadCurrentPlayer();
            }
        }

        public PlayerService(IRepository repository, IConfigurationService configurationService)
        {
            _repository = repository;
            _configurationService = configurationService;
        }

        public Player GetPlayerById(string id)
        {
            return _repository.GetAll<Player>().FirstOrDefault(x => x.Id == id);
        }

        public void LoadLocalPlayers()
        {
            var savedGames = _configurationService.GetSavedGames();
            foreach (var save in savedGames)
            {
                if (!_repository.GetAll<Player>().Any(x => x.Name == save.Name && x.FarmName == save.FarmName))
                {
                    var player = new Player(save.Name, save.FarmName);
                    _repository.AddOrUpdate(player);
                }
            }

            var players = _repository.GetAll<Player>().ToList();
            foreach (var player in players)
            {
                var update = false;
                foreach (var p in players)
                {
                    if (!player.Friends.Any(x => x.Id == p.Id))
                    {
                        player.Friends.Add(new Friend()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            FarmName = p.FarmName
                        });
                        update = true;
                    }   
                }
                if (update)
                {
                    _repository.AddOrUpdate(player);
                }
            }
        }

        private Player LoadCurrentPlayer()
        {
            var name = Game1.player.name;
            var farmName = Game1.player.farmName;
            var matchingPlayers = _repository.GetAll<Player>().Where(x => x.Name == name && x.FarmName == farmName).ToList();
            if (matchingPlayers.Any())
            {
                return matchingPlayers.First();
            }
            else
            {
                throw new Exception("couldn't find current player!");
            }
        }     
    }
}
