namespace Denifia.Stardew.SendLetters.Domain
{
    public class MemoryCache : IMemoryCache
    {
        public bool TryGetValue<T>(string key, out T value) where T : new()
        {
            value = default(T);
            return false;
        }

        public void Set<T>(string key, T value) where T : new()
        {

        }
    }
}