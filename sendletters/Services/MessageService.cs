using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Services
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

        public void CreateMessage(CreateMessageModel model)
        {
            var message = new Message
            {
                Id = Guid.NewGuid().ToString(),
                ToPlayerId = model.ToPlayerId,
                FromPlayerId = model.FromPlayerId,
                Text = model.Text,
                CreatedDate = DateTime.Now
            };

            _repository.AddOrUpdate(message);
        }

        public int UnreadMessageCount(string playerId)
        {
            return _repository.GetAll<Message>().Where(x => x.ToPlayerId == playerId).Count();
        }

        public Message GetFirstMessage(string playerId)
        {
            return _repository.GetAll<Message>().Where(x => x.ToPlayerId == playerId).FirstOrDefault();
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
