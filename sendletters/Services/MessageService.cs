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
                Text = model.Text,
                FromPlayerId = model.FromPlayerId
            };

            _repository.CreateMessageForPlayer(model.ToPlayerId, message);
        }

        public int UnreadMessageCount(string playerId)
        {
            return _repository.FindMessagesForPlayer(playerId, x => true).Count();
        }

        public Message GetFirstMessage(string playerId)
        {
            return _repository.FindMessagesForPlayer(playerId, x => true).FirstOrDefault();
        }

        public void CheckForMessages(string playerId)
        {
            ModEvents.RaisePlayerMessagesUpdatedEvent();
            //var player = _playerService.GetPlayerById(playerId);

            //if (player != null)
            //{
            //    var messages = _repository.FindMessagesForPlayer(player, x => true);
                
            //}
        }


        //public MessageService(Uri api) : base(api)
        //{
        //    ModEvents.MessageRead += MessageRead;
        //}

        private void MessageRead(Message message)
        {
            _repository.Delete(message);
            //RequestDeleteMessage(message);
        }

        public void RequestOverridePlayerMessages()
        {
            //if (Repo.CurrentPlayer == null) return;

            //var urlSegments = new Dictionary<string, string>();
            //urlSegments.Add("id", Repo.CurrentPlayer.Id);
            //GetRequest<List<Message>>("messages/{id}", urlSegments, OverridePlayerMessages);
        }

        internal void OverridePlayerMessages(List<Message> messages)
        {
            //Repo.CurrentPlayer.Messages = messages;
            //ModEvents.RaisePlayerMessagesUpdatedEvent();
        }

        public void RequestSendMessage(Player friend, string message)
        {
            //if (Repo.CurrentPlayer == null) return;

            //var messageCreateModel = new MessageCreateModel
            //{
            //    ToPlayerId = friend.Id,
            //    Message = message
            //};

            //var urlSegments = new Dictionary<string, string>();
            //urlSegments.Add("id", Repo.CurrentPlayer.Id);
            //PostRequest("messages/{id}", urlSegments, messageCreateModel, ModEvents.RaiseMessageSentEvent);
        }

        public void RequestDeleteMessage(Message message)
        {
            //if (Repo.CurrentPlayer == null || message == null) return;

            //var urlSegments = new Dictionary<string, string>();
            //urlSegments.Add("playerId", Repo.CurrentPlayer.Id);
            //urlSegments.Add("id", message.Id);
            //DeleteRequest("messages/{playerId}/{id}", urlSegments);
        }

        
    }
}
