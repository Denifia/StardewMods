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
    public class PlayerService : RestBaseService
    {
        public PlayerService(Uri api) : base(api)
        { 
        }

        public void CreatePlayer()
        {
            if (!Repo.Players.Any()) return;

            var playerName = Game1.player.Name;
            var farmName = Game1.player.farmName;

            var possiblePlayers = Repo.Players.Where(x => !x.Games.Any() || x.Games.Any(g => g.PlayerName == playerName && g.FarmName == farmName)).ToList();
            if (!possiblePlayers.Any()) return;
            var player = possiblePlayers[0];

            var createrPlayerModel = new PlayerCreaterModel
            {
                Name = player.Name
            };

            var urlSegments = new Dictionary<string, string>();
            urlSegments.Add("id", player.Id);
            PutRequest<Player>("players/{id}", urlSegments, createrPlayerModel, ModEvents.RaisePlayerCreatedEvent, player);
        }
    }
}
