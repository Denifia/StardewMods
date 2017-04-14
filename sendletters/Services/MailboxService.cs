using denifia.stardew.sendletters.Domain;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Services
{
    public class MailboxService
    {
        public void ShowLetter(Message message)
        {
            Game1.activeClickableMenu = (IClickableMenu)new LetterViewerMenu(message.Text, "Player Mail");
            ModEvents.RaiseMessageReadEvent(message);
        }
    }
}
