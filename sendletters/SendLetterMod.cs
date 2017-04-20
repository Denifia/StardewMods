using Denifia.Stardew.SendLetters.Domain;
using Denifia.Stardew.SendLetters.Services;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using xTile.Dimensions;

namespace Denifia.Stardew.SendLetters
{
    public class SendLetterMod
    {
        private readonly IMod _mod;
        private readonly IModHelper _modHelper;
        private readonly IConfigurationService _configService;
        private readonly IPlayerService _playerService;
        private readonly IMessageService _messageService;
        private readonly IMailboxService _mailboxService;

        private bool SavedGameLoaded = false;

        public SendLetterMod(IMod mod,
            IModHelper modHelper,
            IConfigurationService configService,
            IPlayerService playerService,
            IMessageService messageService,
            IMailboxService mailboxService)
        {
            _mod = mod;
            _modHelper = modHelper;
            _configService = configService;
            _playerService = playerService;
            _messageService = messageService;
            _mailboxService = mailboxService;

            ModEvents.PlayerMessagesUpdated += PlayerMessagesUpdated;
            ModEvents.PlayerCreated += PlayerCreated;
            ModEvents.MessageSent += MessageSent;
            SaveEvents.AfterLoad += AfterSavedGameLoad;

            RegisterCommands();
        }
        
        private void RegisterCommands()
        {
            _modHelper.ConsoleCommands
                .Add("sendletters_addfriend", "Adds a friend to your currently loaded game.\n\nUsage: sentletters_addfriend \n- name: the name of your friend.\n- id: the id of your friend.", HandleCommand)
                .Add("sendletters_me", "Shows you the command that your friends need to type to add you as a friend.\n\nUsage: sentletters_me", HandleCommand);
        }
        
        private void HandleCommand(string command, string[] args)
        {
            if (!SavedGameLoaded)
            {
                _mod.Monitor.Log("Please load up a saved game first, then try again.", LogLevel.Warn);
                return;
            }

            switch (command)
            {
                case "sendletters_me":
                    _mod.Monitor.Log("Command for others to add the currently loaded farmer as a friend is...", LogLevel.Info);
                    _mod.Monitor.Log($"sendletters_addfriend -Name {_playerService.CurrentPlayer.Name} -FarmName {_playerService.CurrentPlayer.FarmName} -Id {_playerService.CurrentPlayer.Id}", LogLevel.Info);
                    _mod.Monitor.Log("Feel free to change your <Name> if you want but the <Id> needs to stay as it is.", LogLevel.Info);
                    break;
                case "sendletters_addfriend":
                    if (args.Length == 6 && args[0] == "-Name" && args[2] == "-FarmName" && args[4] == "-Id")
                    {
                        var name = args[1];
                        var farmName = args[3];
                        var id = args[5];
                        _playerService.AddFriendToCurrentPlayer(name, farmName, id);
                        _mod.Monitor.Log($"{name} ({farmName} Farm) was added!", LogLevel.Info);
                    }
                    else
                    {
                        LogArgumentsInvalid(command);
                    }
                    break;
                default:
                    throw new NotImplementedException($"SendLetters received unknown command '{command}'.");
            }
        }

        private void PlayerCreated(Player player)
        {
        }

        private void MessageSent(object sender, EventArgs e)
        {
        }

        private void PlayerMessagesUpdated(object sender, EventArgs e)
        {
            var messageCount = _messageService.UnreadMessageCount(_playerService.CurrentPlayer.Id);
            if (messageCount > 0)
            {
                _mailboxService.PostLetters(messageCount);
            }
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            SavedGameLoaded = true;
            _playerService.LoadLocalPlayers();

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
                _messageService.CheckForMessages(_playerService.CurrentPlayer.Id);
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

        /****
        ** Logging
        ****/
        private void LogUsageError(string error, string command)
        {
            _mod.Monitor.Log($"{error} Type 'help {command}' for usage.", LogLevel.Error);
        }

        private void LogArgumentsInvalid(string command)
        {
            LogUsageError("The arguments are invalid.", command);
        }
    }
}
