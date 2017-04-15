using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Domain
{
    public class Repository
    {
        private static Repository instance;

        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        private bool _debug { get; set; }
        public bool InDebugMode { get
            {
                return _debug;
            }
        }
        
        private Repository()
        {
            Players = new List<Player>();
        }

        public static Repository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Repository();
                }
                return instance;
            }
        }

        public void LoadConfig(ModConfig config)
        {
            _debug = config.Debug;
            var players = config.Players;
            Players.AddRange(players);
        }

        internal void SetCurrentPlayer()
        {
            CurrentPlayer = Players[0];
        }
    }
}
