using denifia.stardew.sendletters.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public class MessageService : RestBaseService
    {
        public MessageService(Uri api) : base(api)
        {
        }

        public void RequestOverridePlayerMessages()
        {
            if (Repo.CurrentPlayer == null) return;

            var urlSegments = new Dictionary<string, string>();
            urlSegments.Add("id", Repo.CurrentPlayer.Id);
            GetRequest<List<Message>>("messages/{id}", urlSegments, OverridePlayerMessages);
        }

        public void OverridePlayerMessages(List<Message> messages)
        {
            Repo.CurrentPlayer.Messages = messages;
            ModEvents.RaisePlayerMessagesUpdatedEvent();
        }
    }
}
