namespace denifia.stardew.sendletters.Services
{
    public interface IConfigurationService
    {
        string GetLocalPath();
        bool InDebugMode();
        bool InLocalOnlyMode();
    }
}
