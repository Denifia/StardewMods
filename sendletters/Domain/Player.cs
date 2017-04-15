using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Domain
{
    public class Player
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FarmName { get; set; }

        public List<Message> Messages { get; set; }

        public List<Player> Friends { get; set; }

        public List<Game> Games { get; set; }

        public Player(string name, string farmName, string random)
        {
            Name = name;
            FarmName = farmName;
            Id = string.Join(".", name, farmName, random).ToUpper();

            Messages = new List<Message>();
            Friends = new List<Player>();
        }
    }
}
