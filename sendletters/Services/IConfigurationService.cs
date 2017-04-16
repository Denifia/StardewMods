using System;

namespace denifia.stardew.sendletters.Services
{
    public interface IConfigurationService
    {
        string CurrentPlayerId { get; set; }
        Uri GetApiUri();
        string GetLocalPath();
        bool InDebugMode();
        bool InLocalOnlyMode();
    }
}
