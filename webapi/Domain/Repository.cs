using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;

namespace denifia.stardew.sendletters.webapi.Domain
{
    public class Repository : IRepository
    {
        private DirectoryInfo _dataDirectory;
        private readonly IMemoryCache _memoryCache;

        public Repository(IHostingEnvironment env, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _dataDirectory = new DirectoryInfo(env.ContentRootPath);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : new()
        {
            return await Task.Run(() => GetAll<T>());
        }

        public async Task<T> AddAsync<T>(T entity) where T : new()
        {
            var entities = await Task.Run(() => GetAll<T>());
            entities.Add(entity);
            await SaveEntityAsync(entities);
            return entity;
        }

        private IList<T> GetAll<T>() where T : new()
        {
            IList<T> entities = null;
            var key = GetKeyForType<T>();
            if (!_memoryCache.TryGetValue(key, out entities))
            {
                entities = LoadEntity<T>();
                _memoryCache.Set(key, entities);
            }

            if (entities == null)
            {
                entities = new List<T>();
                SaveEntity(entities);
            }

            return entities;
        }

        private IList<T> LoadEntity<T>() where T : new()
        {
            var databaseFile = new FileInfo(GetFileNameForType<T>());
            if (!databaseFile.Exists)
            {
                SaveEntity(new List<T>());
            }
            return JsonConvert.DeserializeObject<IList<T>>(File.ReadAllText(databaseFile.FullName));
        }

        private async Task SaveEntityAsync<T>(IList<T> entity) where T : new()
        {
            await Task.Run(() => SaveEntity<T>(entity));
        }

        private void SaveEntity<T>(IList<T> entity) where T : new()
        {
            _memoryCache.Set(typeof(T), entity);
            File.WriteAllText(GetFileNameForType<T>(), JsonConvert.SerializeObject(entity));
        }

        private string GetFileNameForType<T>() where T : new()
        {
            return Path.Combine(_dataDirectory.FullName, GetKeyForType<T>());
        }

        private string GetKeyForType<T>() where T : new()
        {
            return string.Format("data.{0}.json", typeof(T).Name);
        }
    }
}
