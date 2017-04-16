using denifia.stardew.common.Models;
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
            urlSegments.Add("playerId", playerId);
            _restService.GetRequest<List<Message>>("Messages/ToPlayer/{playerId}", urlSegments, RemoteMessagesRetreived);

            return base.FindMessagesForPlayer(playerId, predicate);
        }

        private void RemoteMessagesRetreived(List<Message> remoteMessages)
        {
            // Add remote messages to local db
            if (remoteMessages == null) return;
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
                    FromPlayerId = message.FromPlayerId,
                    Text = message.Text
                };

                // Not a local player
                var urlSegments = new Dictionary<string, string>();
                _restService.PostRequest("Messages", urlSegments, messageCreateModel, ModEvents.RaiseMessageSentEvent);
            }
        }

        public override void Delete(Message message)
        {
            base.Delete(message);
            // online delete message

            var urlSegments = new Dictionary<string, string>();
            urlSegments.Add("messageId", message.Id);
            _restService.DeleteRequest("Messages/{messageId}", urlSegments);
        }
    }
}

