using Autofac;
using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.Menus;
using denifia.stardew.sendletters.Services;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
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
        private readonly IModHelper _modHelper;
        private readonly IConfigurationService _configService;
        private readonly IRepository _repository;
        private readonly IPlayerService _playerService;
        private readonly IMessageService _messageService;
        private readonly IMailboxService _mailboxService;

        private string _currentPlayerId
        {
            get
            {
                return _playerService.GetCurrentPlayer().Id;
            }
        }

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

            ModEvents.PlayerMessagesUpdated += PlayerMessagesUpdated;
            ModEvents.PlayerCreated += PlayerCreated;
            ModEvents.MessageSent += MessageSent;
        }
        
        internal void Init()
        {
            SaveEvents.AfterLoad += AfterSavedGameLoad;
        }

        private void PlayerCreated(Player player)
        {
        }

        private void MessageSent(object sender, EventArgs e)
        {
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
            _playerService.LoadCurrentPlayer();

            LocationEvents.CurrentLocationChanged += CurrentLocationChanged;
            TimeEvents.TimeOfDayChanged += TimeOfDayChanged;
            SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }

        private void CurrentLocationChanged(object sender, EventArgsCurrentLocationChanged e)
        {
            if (e.NewLocation.name == "Farm")
            {
                // Only watch for mouse events while at the farm, for performance
                ControlEvents.MouseChanged += MouseChanged;
            }
            else
            {
                ControlEvents.MouseChanged -= MouseChanged;
            }
        }

        private void TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            var timeToCheck = false;
            if (_configService.InDebugMode())
            {
                timeToCheck = true;
            }
            else
            {
                if (e.NewInt % 10 == 0 && (e.NewInt >= 800 && e.NewInt <= 1600))
                {
                    // Check mail on every hour in game between 8am and 6pm
                    timeToCheck = true;
                }
            }          
            if (timeToCheck)
            {
                _messageService.CheckForMessages(_currentPlayerId);
            }
        }

        private void MouseChanged(object sender, EventArgsMouseStateChanged e)
        {
            if (e.NewState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                // Check if the click is on the mailbox tile or the one above it
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
