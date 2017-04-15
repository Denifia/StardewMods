using denifia.stardew.sendletters.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Domain
{
    public class LocalRepository : IRepository
    {
        private List<Player> _players;
        private IConfigurationService _configService;
        private readonly string _databaseFileName = "data.json";
        private FileInfo _database;

        public LocalRepository(IConfigurationService configService)
        {
            _configService = configService;

            var filePath = _configService.GetLocalPath();
            _database = new FileInfo(Path.Combine(filePath, _databaseFileName));

            LoadDatabase();
        }

        public IQueryable<Player> GetAllPlayers()
        {
            return _players.AsQueryable();
        }

        public IQueryable<Player> FindPlayers(Expression<Func<Player, bool>> predicate)
        {
            return _players.AsQueryable().Where(predicate);
        }

        public IQueryable<Message> FindMessagesForPlayer(string playerId, Expression<Func<Message, bool>> predicate)
        {
            var p = _players.FirstOrDefault(x => x.Id == playerId);
            if (p != null)
            {
                return p.Messages.AsQueryable().Where(predicate);
            }
            return new List<Message>().AsQueryable();
        }

        public void CreateMessageForPlayer(string playerId, Message message)
        {
            var p = _players.FirstOrDefault(x => x.Id == playerId);
            if (p != null)
            {
                p.Messages.Add(message);
                SaveDatabase();
            }
        }

        public void Create(Player player)
        {
            _players.Add(player);
            SaveDatabase();
        }

        public void Delete(Message message)
        {
            foreach (var player in _players)
            {
                player.Messages.RemoveAll(x => x.Id == message.Id);
            }
            SaveDatabase();
        }

        private void LoadDatabase()
        {
            if (!File.Exists(_database.FullName))
            {
                File.WriteAllText(_database.FullName, "[]");
            }

            var text = File.ReadAllText(_database.FullName);
            _players = SimpleJson.DeserializeObject<List<Player>>(text);

            if (_players == null)
            {
                _players = new List<Player>();
            }
        }

        private void SaveDatabase()
        {
            File.WriteAllText(_database.FullName, SimpleJson.SerializeObject(_players));
        }
    }
}
