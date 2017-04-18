using Denifia.Stardew.SendLetters.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denifia.Stardew.SendLetters.Domain
{
    public class Database
    {
        public List<Player> Players { get; set; }
        public List<Message> Messages { get; set; }

        public Database()
        {
            Players = new List<Player>();
            Messages = new List<Message>();
        }
    }
}
