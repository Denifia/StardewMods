using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Services;
using StardewModdingAPI.Events;
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
        private int LastMessageIndex = -1;

        public Program(ModConfig config)
        {
            Repo.LoadConfig(config);

            PlayerService = new PlayerService(config.ApiUrl);
            MessageService = new MessageService(config.ApiUrl);
            MailboxService = new MailboxService();

            ModEvents.PlayerMessagesUpdated += PlayerMessagesUpdated;
            ModEvents.PlayerCreated += PlayerCreated;
            SaveEvents.AfterLoad += AfterSavedGameLoad;
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
                var currentIndex = messages.Count - 1;

                if (currentIndex > LastMessageIndex)
                {
                    MailboxService.ShowLetter(messages[currentIndex]);
                    LastMessageIndex = currentIndex;
                }
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
