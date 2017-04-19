using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace Denifia.Stardew.SendLetters.Common.Domain
{
    public abstract class JsonFileRepository : IRepository
    {
        internal DirectoryInfo _dataDirectory;

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : Entity
        {
            return await Task.Run(() => GetAll<T>());
        }

        public async Task<T> AddOrUpdateAsync<T>(T entity) where T : Entity
        {
            return await Task.Run(() => AddOrUpdate(entity));
        }

        public T AddOrUpdate<T>(T entity) where T : Entity
        {
            var entities = GetAll<T>();
            var existing = entities.FirstOrDefault(x => x.Id == entity.Id);
            if (existing == null)
            {
                entities.Add(entity);
            }
            else
            {
                var index = entities.IndexOf(existing);
                entities[index] = entity;           
            }
            SaveEntity(entities);
            return entity;
        }

        public async Task DeleteAsync<T>(T entity) where T : Entity
        {
            await Task.Run(() => Delete(entity));
        }

        public void Delete<T>(T entity) where T : Entity
        {
            var entities = GetAll<T>();
            var existing = entities.FirstOrDefault(x => x.Id == entity.Id);
            if (existing != null)
            {
                var index = entities.IndexOf(existing);
                entities.RemoveAt(index);
            }
            SaveEntity(entities);
        }

        public List<T> GetAll<T>() where T : Entity
        {
            var key = GetKeyForType<T>();
            if (!TryGetValueCache(key, out List<T> entities))
            {
                entities = LoadEntity<T>();
                if (entities == null)
                {
                    entities = new List<T>();
                    SaveEntity(entities);
                }
                else
                {
                    SaveToCache(key, entities);
                }
            }
            return entities;
        }

        internal List<T> LoadEntity<T>() where T : Entity
        {
            var databaseFile = new FileInfo(GetFileNameForType<T>());
            if (!databaseFile.Exists)
            {
                SaveEntity(new List<T>());
            }
            return DeserializeObject<List<T>>(File.ReadAllText(databaseFile.FullName));
        }

        internal async Task SaveEntityAsync<T>(List<T> entity) where T : Entity
        {
            await Task.Run(() => SaveEntity<T>(entity));
        }

        internal void SaveEntity<T>(List<T> entity) where T : Entity
        {
            SaveToCache(GetKeyForType<T>(), entity);
            File.WriteAllText(GetFileNameForType<T>(), SerializeObject(entity));
        }

        internal string GetFileNameForType<T>() where T : Entity
        {
            return Path.Combine(_dataDirectory.FullName, GetKeyForType<T>());
        }

        internal string GetKeyForType<T>() where T : Entity
        {
            return string.Format("data.{0}.json", typeof(T).Name.ToLower());
        }

        internal virtual bool TryGetValueCache<T>(string key, out T value) where T : new()
        {
            value = default(T);
            return false;
        }

        internal virtual void SaveToCache<T>(string key, T value) where T : new()
        {
            
        }

        internal abstract string SerializeObject(object value);

        internal abstract T DeserializeObject<T>(string value) where T : new();
    }
}
