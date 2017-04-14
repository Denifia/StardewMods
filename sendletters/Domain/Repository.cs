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
            var player = new Player
            {
                Id = config.YourUniqueId,
                Name = config.YourDisplayName
            };

            if (config.YourFriendsUniqueIds != null)
            {
                foreach (var friend in config.YourFriendsUniqueIds)
                {
                    player.Friends.Add(new Player { Id = friend });
                }
            }

            Players.Add(player);
        }

        internal void SetCurrentPlayer()
        {
            CurrentPlayer = Players[0];
        }
    }
}
