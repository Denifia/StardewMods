using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Domain
{
    public interface IRepository
    {
        IQueryable<Player> FindPlayers(Expression<Func<Player, bool>> predicate);
        IQueryable<Player> GetAllPlayers();
        void Create(Player player);

        IQueryable<Message> FindMessagesForPlayer(string playerId, Expression<Func<Message, bool>> predicate);
        void CreateMessageForPlayer(string playerId, Message message);
        void Delete(Message message);

        //void Update(TEntity entity);
        //void Delete(TEntity entity);
        //void Insert(TEntity entity);
        //void Save();
    }
}
