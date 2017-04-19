using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Services
{
    public interface IMailboxService
    {
        void PostLetters(int count);
        void ShowLetter(Message message);
        void ShowFriendSelecter();
    }
}
