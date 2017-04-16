using denifia.stardew.common.Domain;
using denifia.stardew.sendletters.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public interface IMailboxService
    {
        void PostLetters(int count);
        void ShowLetter(Message message);
        void ShowFriendSelecter();
    }
}
