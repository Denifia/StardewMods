using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Common.Domain
{
    public interface IRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : Entity;
        List<T> GetAll<T>() where T : Entity;

        Task<T> AddOrUpdateAsync<T>(T entity) where T : Entity;
        T AddOrUpdate<T>(T entity) where T : Entity;

        Task DeleteAsync<T>(T entity) where T : Entity;
        void Delete<T>(T entity) where T : Entity;
    }
}
