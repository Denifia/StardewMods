using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace denifia.stardew.webapi.Domain
{
    public class Repository
    {
        private static Repository instance;

        public List<Player> Players { get; set; }

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

        
    }
}
