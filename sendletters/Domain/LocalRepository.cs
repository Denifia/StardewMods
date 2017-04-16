using denifia.stardew.sendletters.Services;
using RestSharp;
using StardewValley;
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
        internal List<Player> _players;
        internal IConfigurationService _configService;
        private readonly string _databaseFileName = "data.json";
        private FileInfo _database;

        public LocalRepository(IConfigurationService configService)
        {
            _configService = configService;

            var filePath = _configService.GetLocalPath();
            _database = new FileInfo(Path.Combine(filePath, _databaseFileName));

            LoadDatabase();
        }

        public virtual IQueryable<Player> GetAllPlayers()
        {
            return _players.AsQueryable();
        }

        public virtual IQueryable<Player> FindPlayers(Expression<Func<Player, bool>> predicate)
        {
            return _players.AsQueryable().Where(predicate);
        }

        public virtual IQueryable<Message> FindMessagesForPlayer(string playerId, Expression<Func<Message, bool>> predicate)
        {
            var p = _players.FirstOrDefault(x => x.Id == playerId);
            if (p != null)
            {
                return p.Messages.AsQueryable().Where(predicate);
            }
            return new List<Message>().AsQueryable();
        }

        public virtual void CreateMessageForPlayer(string playerId, Message message)
        {
            var p = _players.FirstOrDefault(x => x.Id == playerId);
            if (p != null)
            {
                // Player is local
                p.Messages.Add(message);
                SaveDatabase();
            }
        }

        public virtual void Create(Player player)
        {
            // Do nothing if local only
        }

        public virtual void Delete(Message message)
        {
            foreach (var player in _players)
            {
                player.Messages.RemoveAll(x => x.Id == message.Id);
            }
            SaveDatabase();
        }

        internal void LoadDatabase()
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

            GetSavedGames();
            foreach (var save in saveGames)
            {
                if (!_players.Any(x => x.Name == save.Name && x.FarmName == save.FarmName))
                {
                    _players.Add(save);
                }
            }

            foreach (var player in _players)
            {
                foreach (var p in _players)
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
            var strat = new PocoJsonSerializerStrategy();
            File.WriteAllText(_database.FullName, SimpleJson.SerializeObject(_players, strat));
        }

        private List<Player> saveGames = new List<Player>();
        private void GetSavedGames()
        {
            string str = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley"), "Saves"));
            if (Directory.Exists(str))
            {
                string[] directories = Directory.GetDirectories(str);
                if (directories.Length != 0)
                {
                    foreach (string path2 in directories)
                    {
                        try
                        {
                            FileInfo file = new FileInfo(Path.Combine(str, path2, "SaveGameInfo"));
                            if (file.Exists)
                            {
                                var fileContents = File.ReadAllText(file.FullName);

                                var farmerNodeStart = fileContents.IndexOf("<Farmer");
                                var farmerNodeEnd = fileContents.IndexOf("</Farmer>");
                                var farmerNode = fileContents.Substring(farmerNodeStart, farmerNodeEnd - farmerNodeStart);
                                var playerNameNodeStart = farmerNode.IndexOf("<name>") + 6;
                                var playerNameNodeEnd = farmerNode.IndexOf("</name>");
                                var playerName = farmerNode.Substring(playerNameNodeStart, playerNameNodeEnd - playerNameNodeStart);

                                var farmNameNodeStart = fileContents.IndexOf("<farmName>") + 10;
                                var farmNameNodeEnd = fileContents.IndexOf("</farmName>");
                                var farmName = fileContents.Substring(farmNameNodeStart, farmNameNodeEnd - farmNameNodeStart);

                                var farmer = new Player(playerName, farmName);
                                saveGames.Add(farmer);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            //this.saveGames.Sort();
        }
    }
}
