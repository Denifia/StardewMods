using System.Collections.Generic;

namespace denifia.stardew.webapi.Domain
{
    public class Player
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FarmName { get; set; }

        public List<Message> Messages { get; set; }

        public Player()
        {
            Messages = new List<Message>();
        }
    }
}
