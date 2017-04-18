using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Denifia.Stardew.SendLetters.Common.Domain;
using System;
using Newtonsoft.Json;

namespace Denifia.Stardew.SendLetters.webapi.Domain
{
    public class Repository : JsonFileRepository
    {
        private readonly IMemoryCache _memoryCache;

        public Repository(IHostingEnvironment env, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _dataDirectory = new DirectoryInfo(env.ContentRootPath);
        }

        internal override bool TryGetValueCache<T>(string key, out T value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        internal override void SaveToCache<T>(string key, T value)
        {
            _memoryCache.Set(key, value);
        }

        internal override string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        internal override T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
