using denifia.stardew.sendletters.common.Domain;
using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public class MessageService : IMessageService
    {
        private readonly IRepository _repository;
        private readonly IPlayerService _playerService;

        public MessageService(IRepository repository, IPlayerService playerService)
        {
            _repository = repository;
            _playerService = playerService;
            ModEvents.MessageRead += MessageRead;
        }

        public void CreateMessage(MessageCreateMessage model)
        {
            var message = new Message
            {
                Id = Guid.NewGuid().ToString(),
                ToPlayerId = model.ToPlayerId,
                FromPlayerId = model.FromPlayerId,
                Text = model.Text,
                CreatedDate = DateTime.Now
            };

            _repository.CreateMessage(message);
        }

        public int UnreadMessageCount(string playerId)
        {
            return _repository.FindMessages(playerId, x => x.ToPlayerId == playerId).Count();
        }

        public Message GetFirstMessage(string playerId)
        {
            return _repository.FindMessages(playerId, x => x.ToPlayerId == playerId).FirstOrDefault();
        }

        public void CheckForMessages(string playerId)
        {
            ModEvents.RaisePlayerMessagesUpdatedEvent();
        }

        private void MessageRead(Message message)
        {
            _repository.Delete(message);
        }
    }
}
