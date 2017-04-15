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
        private readonly IMessageService _messageService;
        private readonly IMailboxService _mailboxService;

        public Program(IModHelper modHelper, 
            IRepository repository, 
            IConfigurationService configService,
            IPlayerService playerService,
            IMessageService messageService,
            IMailboxService mailboxService)
        {
            _modHelper = modHelper;
            _repository = repository;
            _configService = configService;
            _playerService = playerService;
            _messageService = messageService;
            _mailboxService = mailboxService;

            //Repo.LoadConfig(config);

            //PlayerService = new PlayerService(config.ApiUrl);
            //MessageService = new MessageService(config.ApiUrl);
            //MailboxService = new MailboxService();

            ModEvents.PlayerMessagesUpdated += PlayerMessagesUpdated;
            //ModEvents.PlayerCreated += PlayerCreated;
            //ModEvents.MessageSent += MessageSent;
            ModEvents.CheckMailbox += CheckMailbox;
            //SaveEvents.AfterLoad += AfterSavedGameLoad;
            ControlEvents.KeyPressed += ControlEvents_KeyPressed;
        }

        internal void Init()
        {
            SaveEvents.AfterLoad += AfterSavedGameLoad;
        }

        private void CheckMailbox(object sender, EventArgs e)
        {
            //if (!Repo.CurrentPlayer.Messages.Any()) return;
            var message = _messageService.GetFirstMessage(_playerService.GetCurrentPlayer().Id);
            if (message != null)
            {
                _mailboxService.ShowLetter(message);
            }
            
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
                case Microsoft.Xna.Framework.Input.Keys.L:
                    //Game1.activeClickableMenu = new ComposeLetterMenu("ni");
                    Game1.mailbox.Enqueue("test");
                    break;
                default:
                    break;
            }

            if (index > -1)
            {
                var item = Game1.player.CurrentItem;
                if (item.canBeGivenAsGift())
                {
                    var currentPlayer = _playerService.GetCurrentPlayer();
                    var messageFormat = "Hey there!^I thought you might like this... Take care!  ^   -{0} %item object {1} {2} %%";
                    var messageText = string.Format(messageFormat, currentPlayer.Name, item.parentSheetIndex, item.getStack());
                    var newMessage = new Models.MessageCreateModel
                    {
                        FromPlayerId = currentPlayer.Id,
                        ToPlayerId = currentPlayer.Friends[index].Id,
                        Text = messageText
                    };

                    _messageService.CreateMessage(newMessage);
                    Game1.player.removeItemsFromInventory(item.parentSheetIndex, item.getStack());

                    //MessageService.RequestSendMessage(Repo.CurrentPlayer.Friends[index], string.Format(message, Repo.CurrentPlayer.Name, item.parentSheetIndex, item.getStack()));
                }
            }
        }

        private void PlayerCreated(Player player)
        {
            //Repo.SetCurrentPlayer(player);
        }

        private void PlayerMessagesUpdated(object sender, EventArgs e)
        {
            var messageCount = _messageService.UnreadMessageCount(_playerService.GetCurrentPlayer().Id);
            if (messageCount > 0)
            {
                _mailboxService.PostLetters(messageCount);
            }
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            _playerService.LoadOrCreatePlayer();
            // Find out if we need to set a new player as current
            //PlayerService.CreatePlayer();

            LocationEvents.CurrentLocationChanged += CurrentLocationChanged;
            TimeEvents.TimeOfDayChanged += TimeOfDayChanged;
            //SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }

        private void CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            if (e.NewLocation.name == "Farm")
            {
                ControlEvents.MouseChanged += ControlEvents_MouseChanged;
            } else
            {
                ControlEvents.MouseChanged -= ControlEvents_MouseChanged;
            }
        }

        private void TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            if (_configService.InDebugMode())
            {
                
            }
            else
            {
                if (e.NewInt % 10 == 0 && (e.NewInt >= 800 && e.NewInt <= 1600))
                {
                    // Check mail on every hour in game between 8am and 6pm
                }
            }

            //_messageService.CreateMessage(new Models.MessageCreateModel
            //{
            //    FromPlayerId = _playerService.GetCurrentPlayer().Id,
            //    ToPlayerId = _playerService.GetCurrentPlayer().Id,
            //    Text = "hi"
            //});

            _messageService.CheckForMessages(_playerService.GetCurrentPlayer().Id);
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

        
    }
}
