using Denifia.Stardew.SendItems.Domain;
using Denifia.Stardew.SendItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IMessageService
    {
        void CreateMessage(CreateMessageModel model);
        void CheckForMessages(string playerId);
        int UnreadMessageCount(string playerId);
        Message GetFirstMessage(string playerId);
    }
}
