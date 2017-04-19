using Denifia.Stardew.SendLetters.Domain;
using System;
using System.Collections.Generic;

namespace Denifia.Stardew.SendLetters.Services
{
    public interface IConfigurationService
    {
        Uri GetApiUri();
        string GetLocalPath();
        bool InDebugMode();
        bool InLocalOnlyMode();
        List<SavedGame> GetSavedGames();
    }
}
