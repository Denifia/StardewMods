using Denifia.Stardew.SendItems.Events;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;

namespace Denifia.Stardew.SendItems.Services
{
    public class MailScheduleService
    {
        private readonly IMod _mod;
        private readonly IConfigurationService _configService;

        public MailScheduleService(IMod mod, IConfigurationService configService)
        {
            _mod = mod;
            _configService = configService;

            TimeEvents.AfterDayStarted += AfterDayStarted;
            TimeEvents.TimeOfDayChanged += TimeOfDayChanged;
        }

        private void AfterDayStarted(object sender, EventArgs e)
        {
            // Deliver mail each night
            ModEvents.RaiseOnMailDelivery(this, EventArgs.Empty);
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
                ModEvents.RaiseOnMailDelivery(this, EventArgs.Empty);
            }
        }
    }
}
