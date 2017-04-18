using Denifia.Stardew.SendLetters.Common.Domain;
using Denifia.Stardew.SendLetters.Services;
using RestSharp;
using StardewValley;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Domain
{
    public class LocalRepository : OldIRepository
    {
        internal Database _database;       
        internal IConfigurationService _configService;
        private readonly string _databaseFileName = "data.json";
        private FileInfo _databaseFile;

        public LocalRepository(IConfigurationService configService)
        {
            _configService = configService;

            var filePath = _configService.GetLocalPath();
            _databaseFile = new FileInfo(Path.Combine(filePath, _databaseFileName));

            LoadDatabase();
        }

        public virtual IQueryable<Player> GetAllPlayers()
        {
            return _database.Players.AsQueryable();
        }

        public virtual IQueryable<Player> FindPlayers(Expression<Func<Player, bool>> predicate)
        {
            return _database.Players.AsQueryable().Where(predicate);
        }

        public virtual IQueryable<Message> FindMessages(string playerId, Expression<Func<Message, bool>> predicate)
        {
            return _database.Messages.AsQueryable().Where(predicate);
        }

        public virtual void CreateMessage(Message message)
        {
            _database.Messages.Add(message);
            SaveDatabase();
        }

        public virtual void Create(Player player)
        {
            // Do nothing if local only
        }

        public virtual void Delete(Message message)
        {
            _database.Messages.RemoveAll(x => x.Id == message.Id);
            SaveDatabase();
        }

        internal void LoadDatabase()
        {
            if (!File.Exists(_databaseFile.FullName))
            {
                File.WriteAllText(_databaseFile.FullName, "{}");
            }

            var text = File.ReadAllText(_databaseFile.FullName);
            _database = SimpleJson.DeserializeObject<Database>(text);

            if (_database == null)
            {
                _database = new Database();
            }
            if (_database.Players == null)
            {
                _database.Players = new List<Player>();
            }
            if (_database.Messages == null)
            {
                _database.Messages = new List<Message>();
            }

            GetSavedGames();
            foreach (var save in saveGames)
            {
                if (!_database.Players.Any(x => x.Name == save.Name && x.FarmName == save.FarmName))
                {
                    _database.Players.Add(save);
                }
            }

            foreach (var player in _database.Players)
            {
                foreach (var p in _database.Players)
                {
                    if (!player.Friends.Any(x => x.Id == p.Id))
                    {
                        player.Friends.Add(new Friend()
                        {
                            Id = p.Id,
                            Name = p.Name,
                            FarmName = p.FarmName                            
                        });
                    }
                }
            }

            SaveDatabase();
        }

        internal void SaveDatabase()
        {
            File.WriteAllText(_databaseFile.FullName, SimpleJson.SerializeObject(_database));
        }

    }
}
