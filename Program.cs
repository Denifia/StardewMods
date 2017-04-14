using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Services;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters
{
    public class Program
    {
        private PlayerService PlayerService { get; set; }
        private MessageService MessageService { get; set; }
        private MailboxService MailboxService { get; set; }
        private Repository Repo = Repository.Instance;

        public Program(ModConfig config)
        {
            Repo.LoadConfig(config);

            PlayerService = new PlayerService(config.ApiUrl);
            MessageService = new MessageService(config.ApiUrl);
            MailboxService = new MailboxService();

            ModEvents.PlayerMessagesUpdated += PlayerMessagesUpdated;
            ModEvents.PlayerCreated += PlayerCreated;
            ModEvents.MessageSent += MessageSent;
            SaveEvents.AfterLoad += AfterSavedGameLoad;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;
        }

        private void MessageSent(object sender, EventArgs e)
        {
            
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            int index = -1;
            switch (e.KeyPressed)
            {
                case Microsoft.Xna.Framework.Input.Keys.NumPad1:
                    index = 0;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad2:
                    index = 1;
                    break;
                case Microsoft.Xna.Framework.Input.Keys.NumPad3:
                    index = 2;
                    break;
                default:
                    break;
            }

            if (index > -1)
            {
                var item = Game1.player.CurrentItem;
                if (item.canBeGivenAsGift())
                {
                    var message = "Hey there!^I thought you might like this... Take care!  ^   -{0} %item object {1} {2} %%";
                    MessageService.RequestSendMessage(Repo.CurrentPlayer.Friends[index], string.Format(message, Repo.CurrentPlayer.Name, item.parentSheetIndex, item.getStack()));
                    Game1.player.removeItemsFromInventory(item.parentSheetIndex, item.getStack());
                }
            }
        }

        private void PlayerCreated(object sender, EventArgs e)
        {
            Repo.SetCurrentPlayer();
        }

        private void PlayerMessagesUpdated(object sender, EventArgs e)
        {
            var messages = Repo.CurrentPlayer.Messages;
            if (messages != null && messages.Any())
            {
                MailboxService.ShowLetter(messages.First());
            }
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            // Find out if we need to set a new player as current
            PlayerService.CreatePlayer();

            TimeEvents.TimeOfDayChanged += TimeOfDayChanged;
            SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }

        private void TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            MessageService.RequestOverridePlayerMessages();
        }
    }
}
