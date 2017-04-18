using System;

namespace Denifia.Stardew.SendLetters.Services
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
