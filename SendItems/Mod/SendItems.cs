using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Linq;
using xTile.Dimensions;
using Denifia.Stardew.SendItems.Services;

namespace Denifia.Stardew.SendItems
{
    public class SendItems
    {
        private readonly IMod _mod;
        private readonly IConfigurationService _configService;
        private readonly ICommandService _commandService;
        private readonly IFarmerService _farmerService;
        private readonly IPostboxService _postboxService;

        public SendItems(
            IMod mod,
            IConfigurationService configService,
            ICommandService commandService,
            IFarmerService farmerService,
            IPostboxService postboxService)
        {
            _mod = mod;
            _commandService = commandService;
            _configService = configService;
            _farmerService = farmerService;
            _postboxService = postboxService;

            SaveEvents.AfterLoad += AfterSavedGameLoad;

            _commandService.RegisterCommands();
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            //_playerService.LoadLocalPlayers(); // TODO : Do i still need to replace this?

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
                if (e.NewInt % 10 == 0 && (e.NewInt >= 800 && e.NewInt <= 1800))
                {
                    // Check mail on every hour in game between 8am and 6pm
                    timeToCheck = true;
                }
            }          
            if (timeToCheck)
            {
                //_messageService.CheckForMessages(_playerService.CurrentPlayer.Id);
                // TODO: Riase event to do stuff
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
                    //ModEvents.RaiseCheckMailboxEvent(); // TODO: Relace this with new event
                }
            }
        }
    }
}
