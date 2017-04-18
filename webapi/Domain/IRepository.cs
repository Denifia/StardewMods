using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.webapi.Domain
{
    public interface IRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : Entity;
        Task<T> AddAsync<T>(T entity) where T : Entity;
        Task DeleteAsync<T>(T entity) where T : Entity;
    }
}
