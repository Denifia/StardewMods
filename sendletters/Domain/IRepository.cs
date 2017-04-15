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

        IQueryable<Message> FindMessagesForPlayer(Player player, Expression<Func<Message, bool>> predicate);
        void CreateMessageForPlayer(Player player, Message message);

        //void Update(TEntity entity);
        //void Delete(TEntity entity);
        //void Insert(TEntity entity);
        //void Save();
    }
}
