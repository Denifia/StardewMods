using denifia.stardew.sendletters.Models;
using denifia.stardew.sendletters.Services;
using RestSharp;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Domain
{
    public class LocalAndRemoteRepository : LocalRepository
    {
        internal IRestService _restService;

        public LocalAndRemoteRepository(IConfigurationService configService, 
            IRestService restService)
            : base(configService)
        {
            _restService = restService;
        }

        public override IQueryable<Message> FindMessagesForPlayer(string playerId, Expression<Func<Message, bool>> predicate)
        {
            // Get online messages
            var urlSegments = new Dictionary<string, string>();
            urlSegments.Add("id", playerId);
            _restService.GetRequest<List<Message>>("messages/{id}", urlSegments, RemoteMessagesRetreived);

            return base.FindMessagesForPlayer(playerId, predicate);
        }

        private void RemoteMessagesRetreived(List<Message> remoteMessages)
        {
            // Add remote messages to local db
            foreach (var message in remoteMessages)
            {
                base.CreateMessageForPlayer(_configService.CurrentPlayerId, message);
            }
        }

        public override void CreateMessageForPlayer(string playerId, Message message)
        {
            base.CreateMessageForPlayer(playerId, message);
            // online create message

            if (!_players.Any(x => x.Id == playerId))
            {
                var messageCreateModel = new MessageCreateModel
                {
                    ToPlayerId = playerId,
                    Message = message
                };

                // Not a local player
                var urlSegments = new Dictionary<string, string>();
                urlSegments.Add("id", _configService.CurrentPlayerId);
                _restService.PostRequest("messages/{id}", urlSegments, messageCreateModel, ModEvents.RaiseMessageSentEvent);
            }
        }

        public override void Create(Player player)
        {
            base.Create(player);

            // online create player

            var createrPlayerModel = new PlayerCreaterModel
            {
                Name = player.Name
            };

            var urlSegments = new Dictionary<string, string>();
            urlSegments.Add("id", player.Id);
            _restService.PutRequest<Player>("players/{id}", urlSegments, createrPlayerModel, ModEvents.RaisePlayerCreatedEvent, player);
        }

        public override void Delete(Message message)
        {
            base.Delete(message);
            // online delete message

            var urlSegments = new Dictionary<string, string>();
            urlSegments.Add("playerId", _configService.CurrentPlayerId);
            urlSegments.Add("id", message.Id);
            _restService.DeleteRequest("messages/{playerId}/{id}", urlSegments);
        }
    }
}

