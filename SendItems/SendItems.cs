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
        private readonly IPostboxInteractionService _postboxInteractionService;
        private readonly ILetterboxService _letterboxService;
        private readonly ILetterboxInteractionService _letterboxInteractionService;
        private readonly IMailDeliveryService _mailDeliveryService;

        public SendItems(
            IMod mod,
            IConfigurationService configService,
            ICommandService commandService,
            IFarmerService farmerService,
            IPostboxService postboxService,
            IPostboxInteractionService postboxInteractionService,
            ILetterboxService letterboxService,
            ILetterboxInteractionService letterboxInteractionService,
            IMailDeliveryService mailDeliveryService)
        {
            _mod = mod;
            _commandService = commandService;
            _configService = configService;
            _farmerService = farmerService;
            _postboxService = postboxService;
            _postboxInteractionService = postboxInteractionService;
            _letterboxService = letterboxService;
            _letterboxInteractionService = letterboxInteractionService;
            _mailDeliveryService = mailDeliveryService;

            SaveEvents.AfterLoad += AfterSavedGameLoad;

            _commandService.RegisterCommands();
        }

        private void AfterSavedGameLoad(object sender, EventArgs e)
        {
            _farmerService.LoadCurrentFarmerAsync();

            _letterboxInteractionService.Init();
            _postboxInteractionService.Init();
            _mailDeliveryService.Init();

            SaveEvents.AfterLoad -= AfterSavedGameLoad;
        }
    }
}
