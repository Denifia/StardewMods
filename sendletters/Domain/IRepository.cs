using denifia.stardew.common.Domain;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace denifia.stardew.sendletters.Domain
{
    public interface IRepository
    {
        IQueryable<Player> FindPlayers(Expression<Func<Player, bool>> predicate);
        IQueryable<Message> FindMessages(string playerId, Expression<Func<Message, bool>> predicate);
        void CreateMessage(Message message);
        void Delete(Message message);
    }
}
