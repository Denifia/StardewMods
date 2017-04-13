using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters
{
    public class Player
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<Message> Messages { get; set; }

        public Player()
        {
            Messages = new List<Message>();
        }
    }
}
