using Autofac;
using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Menus;
using denifia.stardew.sendletters.Services;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;
using xTile.ObjectModel;
using xTile.Tiles;

namespace denifia.stardew.sendletters
{
    public class Program
    {
        //private PlayerService PlayerService { get; set; }
        //private MessageService MessageService { get; set; }
        //private MailboxService MailboxService { get; set; }

        private IModHelper _modHelper;
        private IConfigurationService _configService;
        private IRepository _repository;
        private IPlayerService _playerService;

        public Program(IModHelper modHelper, 
            IRepository repository, 
            IConfigurationService configService,
            IPlayerService playerService)
        {
            _modHelper = modHelper;
            _repository = repository;
            _configService = configService;
            _playerService = playerService;

            //Repo.LoadConfig(config);

            //PlayerService = new PlayerService(config.ApiUrl);
            //MessageService = new MessageService(config.ApiUrl);
            //MailboxService = new MailboxService();

            //ModEvents.PlayerMessagesUpdated += PlayerMessagesUpdated;
            //ModEvents.PlayerCreated += PlayerCreated;
            //ModEvents.MessageSent += MessageSent;
            //ModEvents.CheckMailbox += CheckMailbox;
            //SaveEvents.AfterLoad += AfterSavedGameLoad;
            //ControlEvents.KeyPressed += ControlEvents_KeyPressed;
        }

        internal void Init()
        {
            SaveEvents.AfterLoad += AfterSavedGameLoad;
        }

        private void CheckMailbox(object sender, EventArgs e)
        {
            //if (!Repo.CurrentPlayer.Messages.Any()) return;
            //MailboxService.ShowLetter(Repo.CurrentPlayer.Messages.First());
        }

        private void MessageSent(object sender, EventArgs e)
        {
            
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            //int index = -1;
            //switch (e.KeyPressed)
            //{
            //    case Microsoft.Xna.Framework.Input.Keys.NumPad1:
            //        index = 0;
            //        break;
            //    case Microsoft.Xna.Framework.Input.Keys.NumPad2:
            //        index = 1;
            //        break;
            //    case Microsoft.Xna.Framework.Input.Keys.NumPad3:
            //        index = 2;
            //        break;
            //    case Microsoft.Xna.Framework.Input.Keys.L:
            //        //Game1.activeClickableMenu = new ComposeLetterMenu("ni");
            //        Game1.mailbox.Enqueue("test");
            //        break;
            //    default:
            //        break;
            //}

            //if (index > -1)
            //{
            //    var item = Game1.player.CurrentItem;
            //    if (item.canBeGivenAsGift())
            //    {
            //        var message = "Hey there!^I thought you might like this... Take care!  ^   -{0} %item object {1} {2} %%";
            //        MessageService.RequestSendMessage(Repo.CurrentPlayer.Friends[index], string.Format(message, Repo.CurrentPlayer.Name, item.parentSheetIndex, item.getStack()));
            //        Game1.player.removeItemsFromInventory(item.parentSheetIndex, item.getStack());
            //    }
            //}
        }

        private void PlayerCreated(Player player)
        {
            //Repo.SetCurrentPlayer(player);
        }

        private void PlayerMessagesUpdated(object sender, EventArgs e)
        {
            //var messages = Repo.CurrentPlayer.Messages;
            //if (messages != null && messages.Any())
            //{
            //    MailboxService.PostLetters(messages.Count);
            //}
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            _playerService.LoadOrCreatePlayer();
            // Find out if we need to set a new player as current
            //PlayerService.CreatePlayer();

            //LocationEvents.CurrentLocationChanged += LocationEvents_CurrentLocationChanged;
            //TimeEvents.TimeOfDayChanged += TimeOfDayChanged;
            //SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }

        private void LocationEvents_CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            if (e.NewLocation.name == "Farm")
            {
                ControlEvents.MouseChanged += ControlEvents_MouseChanged;
            } else
            {
                ControlEvents.MouseChanged -= ControlEvents_MouseChanged;
            }
        }

        private void ControlEvents_MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            if (e.NewState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Location tileLocation = new Location() { X = (int)Game1.currentCursorTile.X, Y = (int)Game1.currentCursorTile.Y };
                Vector2 key = new Vector2((float)tileLocation.X, (float)tileLocation.Y);

                if (tileLocation.X == 68 && (tileLocation.Y >= 15 && tileLocation.Y <= 16))
                {
                    ModEvents.RaiseCheckMailboxEvent();
                }
            }
        }

        private void TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            //MessageService.RequestOverridePlayerMessages();
        }
    }
}
