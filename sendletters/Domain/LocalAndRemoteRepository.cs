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
        public LocalAndRemoteRepository(IConfigurationService configService)
            : base(configService)
        {
        }

        public override IQueryable<Message> FindMessagesForPlayer(string playerId, Expression<Func<Message, bool>> predicate)
        {
            var localMessages = base.FindMessagesForPlayer(playerId, predicate);

            // online messages
            var remoteMessages = new List<Message>();

            var messages = new List<Message>();
            messages.AddRange(localMessages.ToList());
            messages.AddRange(remoteMessages.ToList());

            return messages.AsQueryable();
        }

        public override void CreateMessageForPlayer(string playerId, Message message)
        {
            base.CreateMessageForPlayer(playerId, message);
            // online create message
        }

        public override void Create(Player player)
        {
            base.Create(player);
            // online create player
        }

        public override void Delete(Message message)
        {
            base.Delete(message);
            // online delete message
        }
    }
}
