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

        private Player _currentPlayer;
        public Player CurrentPlayer { get
            {
                if (_currentPlayer == null)
                {
                    _currentPlayer = LoadCurrentPlayer();
                }
                return _currentPlayer;
            }
            set => _currentPlayer = value;
        }

        public PlayerService(IRepository repository)
        {
            _repository = repository;
        }

        public Player GetPlayerById(string id)
        {
            return _repository.GetAll<Player>().FirstOrDefault(x => x.Id == id);
        }

        private Player LoadCurrentPlayer()
        {
            var name = Game1.player.name;
            var farmName = Game1.player.farmName;
            var players = _repository.GetAll<Player>();
            var matchingPlayers = players.Where(x => x.Name == name && x.FarmName == farmName).ToList();
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
