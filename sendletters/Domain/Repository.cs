using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using denifia.stardew.sendletters.common.Domain;
using RestSharp;

namespace denifia.stardew.sendletters.Domain
{
    public class Repository : denifia.stardew.sendletters.common.Domain.IRepository
    {
        private DirectoryInfo _dataDirectory;

        public Repository()
        {
            //_memoryCache = memoryCache;
            //_dataDirectory = new DirectoryInfo(env.ContentRootPath);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : Entity
        {
            return await Task.Run(() => GetAll<T>());
        }

        public async Task<T> AddAsync<T>(T entity) where T : Entity
        {
            var entities = await Task.Run(() => GetAll<T>());
            entities.Add(entity);
            await SaveEntityAsync(entities);
            return entity;
        }

        public async Task DeleteAsync<T>(T entity) where T : Entity
        {
            var entities = await Task.Run(() => GetAll<T>());
            entities.Remove(entity);
            await SaveEntityAsync(entities);
        }

        private IList<T> GetAll<T>() where T : Entity
        {
            var entities = LoadEntity<T>();
            if (entities == null)
            {
                entities = new List<T>();
                SaveEntity(entities);
            }
            return entities;
        }

        private IList<T> LoadEntity<T>() where T : Entity
        {
            var databaseFile = new FileInfo(GetFileNameForType<T>());
            if (!databaseFile.Exists)
            {
                SaveEntity(new List<T>());
            }
            return SimpleJson.DeserializeObject<IList<T>>(File.ReadAllText(databaseFile.FullName));
        }

        private async Task SaveEntityAsync<T>(IList<T> entity) where T : Entity
        {
            await Task.Run(() => SaveEntity<T>(entity));
        }

        private void SaveEntity<T>(IList<T> entity) where T : Entity
        {
            File.WriteAllText(GetFileNameForType<T>(), SimpleJson.SerializeObject(entity));
        }

        private string GetFileNameForType<T>() where T : Entity
        {
            return Path.Combine(_dataDirectory.FullName, GetKeyForType<T>());
        }

        private string GetKeyForType<T>() where T : Entity
        {
            return string.Format("data.{0}.json", typeof(T).Name.ToLower());
        }
    }
}
