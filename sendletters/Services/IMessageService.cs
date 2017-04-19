using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Domain;
using Denifia.Stardew.SendLetters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Services
{
    public interface IMessageService
    {
        void CreateMessage(CreateMessageModel model);
        void CheckForMessages(string playerId);
        int UnreadMessageCount(string playerId);
        Message GetFirstMessage(string playerId);
    }
}
