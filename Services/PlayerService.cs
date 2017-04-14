using denifia.stardew.sendletters.Models;
using RestSharp;
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
            var player = Repo.Players[0];

            var createrPlayerModel = new CreaterPlayerModel
            {
                Name = player.Name
            };

            var urlSegments = new Dictionary<string, string>();
            urlSegments.Add("id", player.Id);
            PutRequest("players/{id}", urlSegments, createrPlayerModel, ModEvents.RaisePlayerCreatedEvent);
        }
    }
}
