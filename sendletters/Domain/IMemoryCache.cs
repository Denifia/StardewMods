namespace Denifia.Stardew.SendLetters.Domain
{
    public interface IMemoryCache
    {
        bool TryGetValue<T>(string key, out T value) where T : new();

        void Set<T>(string key, T value) where T : new();
    }
}