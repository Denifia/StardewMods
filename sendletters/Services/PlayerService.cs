using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Models;
using RestSharp;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public class PlayerService : IPlayerService
    {
        //public PlayerService(Uri api) : base(api)
        //{ 
        //}

        private const int _randonStringLength = 5;

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            private set { _currentPlayer = value; }
        }

        private readonly IRepository _repository;

        public PlayerService(IRepository repository)
        {
            _repository = repository;
        }

        public void LoadOrCreatePlayer()
        {
            var name = Game1.player.name;
            var farmName = Game1.player.farmName;

            var matchingPlayers = _repository.FindPlayers(x => x.Name == name && x.FarmName == farmName);
            if (matchingPlayers.Any())
            {
                _currentPlayer = matchingPlayers.First();
            } else
            {
                _currentPlayer = CreatePlayer(name, farmName);
            }
        }

        private Player CreatePlayer(string name, string farmName)
        {
            var player = new Player(name, farmName, RandomString(_randonStringLength));
            _repository.Create(player);
            return player;
        }

        private static string RandomString(int length)
        {
            string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random rng = new Random();

            char[] chars = new char[length];
            int setLength = allowedChars.Length;

            for (int i = 0; i < length; ++i)
            {
                chars[i] = allowedChars[rng.Next(setLength)];
            }

            return new string(chars, 0, length);
        }

        //public void CreatePlayer()
        //{
        //    if (!Repo.Players.Any()) return;

        //    var playerName = Game1.player.Name;
        //    var farmName = Game1.player.farmName;

        //    var possiblePlayers = Repo.Players.Where(x => !x.Games.Any() || x.Games.Any(g => g.PlayerName == playerName && g.FarmName == farmName)).ToList();
        //    if (!possiblePlayers.Any()) return;
        //    var player = possiblePlayers[0];

        //    var createrPlayerModel = new PlayerCreaterModel
        //    {
        //        Name = player.Name
        //    };

        //    var urlSegments = new Dictionary<string, string>();
        //    urlSegments.Add("id", player.Id);
        //    PutRequest<Player>("players/{id}", urlSegments, createrPlayerModel, ModEvents.RaisePlayerCreatedEvent, player);
        //}
    }
}
