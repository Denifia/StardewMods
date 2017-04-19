using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Domain;
using Denifia.Stardew.SendLetters.Models;
using System;
using System.Linq;

namespace Denifia.Stardew.SendLetters.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPlayerService _playerService;

        public MessageService(IMessageRepository messageRepository, IPlayerService playerService)
        {
            _messageRepository = messageRepository;
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

            _messageRepository.AddOrUpdate(message);
        }

        public int UnreadMessageCount(string playerId)
        {
            return _messageRepository.GetMessagesToPlayer(playerId).Count();
        }

        public Message GetFirstMessage(string playerId)
        {
            return _messageRepository.GetMessagesToPlayer(playerId).FirstOrDefault();
        }

        public void CheckForMessages(string playerId)
        {
            ModEvents.RaisePlayerMessagesUpdatedEvent();
        }

        private void MessageRead(Message message)
        {
            _messageRepository.Delete(message);
        }
    }
}
