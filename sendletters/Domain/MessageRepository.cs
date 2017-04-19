using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Common.Models;
using Denifia.Stardew.SendLetters.Services;
using Pathoschild.Stardew.Common;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Domain
{

    public class MessageRepository : IMessageRepository
    {
        private readonly IRepository _repository;
        private readonly IRestService _restService;
        private readonly IConfigurationService _configurationService;

        public MessageRepository(IRepository repository, 
            IRestService restService,
            IConfigurationService configurationService)
        {
            _repository = repository;
            _restService = restService;
            _configurationService = configurationService;
        }
       
        public IQueryable<Message> GetMessagesToPlayer(string id)
        {
            if (!_configurationService.InLocalOnlyMode())
            {
                // Get online messages
                var urlSegments = new Dictionary<string, string> { { "playerId", id } };
                _restService.GetRequest<List<Message>>("Messages/ToPlayer/{playerId}", urlSegments, RemoteMessagesRetreived);
            }
            return GetAll().AsQueryable();
        }

        public IQueryable<Message> GetMessagesFromPlayer(string id)
        {
            if (!_configurationService.InLocalOnlyMode())
            {
                // Get online messages
                var urlSegments = new Dictionary<string, string> { { "playerId", id } };
                _restService.GetRequest<List<Message>>("Messages/FromPlayer/{playerId}", urlSegments, RemoteMessagesRetreived);
            }
            return GetAll().AsQueryable();
        }

        private void RemoteMessagesRetreived(List<Message> remoteMessages)
        {
            // Add remote messages to local db
            if (remoteMessages == null) return;
            foreach (var message in remoteMessages)
            {
                AddOrUpdate(message);
            }
        }

        public Message AddOrUpdate(Message message)
        {
            var savedMessage = _repository.AddOrUpdate(message);

            if (!_configurationService.InLocalOnlyMode())
            {
                if (!_repository.GetAll<Player>().Any(x => x.Id == message.ToPlayerId))
                {
                    // Message destined for remote player
                    var messageCreateModel = new MessageCreateModel
                    {
                        ToPlayerId = message.ToPlayerId,
                        FromPlayerId = message.FromPlayerId,
                        Text = message.Text
                    };

                    var urlSegments = new Dictionary<string, string>();
                    _restService.PostRequest("Messages", urlSegments, messageCreateModel, ModEvents.RaiseMessageSentEvent);
                }
            }

            CommonHelper.ShowInfoMessage("Letter sent!");

            return savedMessage;
        }

        public async Task<Message> AddOrUpdateAsync(Message message)
        {
            return await _repository.AddOrUpdateAsync(message);
        }

        public void Delete(Message message)
        {
            _repository.Delete(message);

            if (!_configurationService.InLocalOnlyMode())
            {
                var urlSegments = new Dictionary<string, string> { { "messageId", message.Id } };
                _restService.DeleteRequest("Messages/{messageId}", urlSegments);
            }
        }

        public async Task DeleteAsync(Message message)
        {
            await _repository.DeleteAsync(message);
        }

        public List<Message> GetAll()
        {
            return _repository.GetAll<Message>();
        }

        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _repository.GetAllAsync<Message>();
        }
    }
}
