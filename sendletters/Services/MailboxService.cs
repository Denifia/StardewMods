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
    public class MailboxService : IMailboxService
    {
        public void ShowLetter(Message message)
        {
            Game1.activeClickableMenu = (IClickableMenu)new LetterViewerMenu(message.Text, "Player Mail");
            if (Game1.mailbox.Any())
            {
                Game1.mailbox.Dequeue();
            }
            ModEvents.RaiseMessageReadEvent(message);
        }

        public void PostLetters(int count)
        {
            while (Game1.mailbox.Any() && Game1.mailbox.Peek() == "playerMail")
            {
                Game1.mailbox.Dequeue();
            }

            for (int i = 0; i < count; i++)
            {
                Game1.mailbox.Enqueue("playerMail");
            }
        }

        
    }
}
