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
            SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }

        private void TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            var timeToCheck = false;
            if (e.NewInt % 10 == 0 && (e.NewInt >= 800 && e.NewInt <= 1800))
            {
                // Check mail on every hour in game between 8am and 6pm
                timeToCheck = true;
            }        

            if (timeToCheck || _configService.InDebugMode())
            {
                //_messageService.CheckForMessages(_playerService.CurrentPlayer.Id);
                // TODO: Riase event to do stuff
            }
        }
    }
}
