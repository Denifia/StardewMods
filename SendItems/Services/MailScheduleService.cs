using Denifia.Stardew.SendItems.Events;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;

namespace Denifia.Stardew.SendItems.Services
{
    public interface IMailScheduleService
    {

    }

    public class MailScheduleService : IMailScheduleService
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
            // Deliver mail each morning
            ModEvents.RaiseOnMailCleanup(this, EventArgs.Empty);
        }

        private void TimeOfDayChanged(object sender, EventArgsIntChanged e)
        {
            if (_configService.InDebugMode())
            {
                ModEvents.RaiseOnMailCleanup(this, EventArgs.Empty);
            }
        }
    }
}
