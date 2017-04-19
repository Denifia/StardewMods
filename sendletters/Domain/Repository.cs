using RestSharp;
using Denifia.Stardew.SendLetters.Common.Domain;
using StardewModdingAPI;
using System.IO;

namespace Denifia.Stardew.SendLetters.Domain
{
    public class Repository : JsonFileRepository
    {
        private readonly IMemoryCache _memoryCache;

        public Repository(IModHelper modHelper, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _dataDirectory = new DirectoryInfo(modHelper.DirectoryPath);
        }

        //internal override bool TryGetValueCache<T>(string key, out T value)
        //{
        //    return _memoryCache.TryGetValue(key, out value);
        //}

        //internal override void SaveToCache<T>(string key, T value)
        //{
        //    _memoryCache.Set(key, value);
        //}

        internal override string SerializeObject(object value)
        {
            return SimpleJson.SerializeObject(value);
        }

        internal override T DeserializeObject<T>(string value)
        {
            return SimpleJson.DeserializeObject<T>(value);
        }
    }
}
