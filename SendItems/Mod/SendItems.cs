using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Linq;
using xTile.Dimensions;
using Denifia.Stardew.SendItems.Services;
using Denifia.Stardew.SendItems.Events;

namespace Denifia.Stardew.SendItems
{
    public class SendItems
    {
        private readonly IMod _mod;
        private readonly IConfigurationService _configService;
        private readonly ICommandService _commandService;
        private readonly IFarmerService _farmerService;
        private readonly IPostboxService _postboxService;
        private readonly ILetterboxService _letterboxService;
        private readonly ILetterboxInteractionService _letterboxInteractionService;

        public SendItems(
            IMod mod,
            IConfigurationService configService,
            ICommandService commandService,
            IFarmerService farmerService,
            IPostboxService postboxService,
            ILetterboxService letterboxService,
            ILetterboxInteractionService letterboxInteractionService)
        {
            _mod = mod;
            _commandService = commandService;
            _configService = configService;
            _farmerService = farmerService;
            _postboxService = postboxService;
            _letterboxService = letterboxService;
            _letterboxInteractionService = letterboxInteractionService;

            SaveEvents.AfterLoad += AfterSavedGameLoad;
            TimeEvents.AfterDayStarted += AfterDayStarted;

            _commandService.RegisterCommands();
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            _letterboxInteractionService.Init();
            SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }

        private void AfterDayStarted(object sender, EventArgs e)
        {
            // Deliver mail each night
            SendItemsModEvents.RaiseOnMailDeliverySchedule(this, EventArgs.Empty);
        }

        private void TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            var timeToCheck = false;

            // Deliver mail at lunch time
            if (e.NewInt == 1200)
            {
                timeToCheck = true;
            }        

            if (timeToCheck || _configService.InDebugMode())
            {
                SendItemsModEvents.RaiseOnMailDeliverySchedule(this, EventArgs.Empty);
            }
        }
    }
}
