using denifia.stardew.sendletters.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Domain
{
    public class Repository 
    {

        public List<Player> Players { get; set; }
        //public Player CurrentPlayer { get; set; }

        //private bool _debug { get; set; }
        //public bool InDebugMode { get
        //    {
        //        return _debug;
        //    }
        //}
        private IConfigurationService _configService;

        public Repository(IConfigurationService configService)
        {
            _configService = configService;

            Players = new List<Player>();
        }

        public IQueryable<Player> GetAllPlayers()
        {
            throw new NotImplementedException();
        }

        //public void LoadConfig(ModConfig config)
        //{
        //    _debug = config.Debug;
        //    var players = config.Players;
        //    Players.AddRange(players);
        //}

        //internal void SetCurrentPlayer(Player player)
        //{
        //    CurrentPlayer = player;
        //}
    }
}
