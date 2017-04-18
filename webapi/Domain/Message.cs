using System;

namespace denifia.stardew.sendletters.webapi.Domain
{
    public class Message : Entity
    {
        public string FromPlayerId { get; set; }
        public string ToPlayerId { get; set; }
        public string Text { get; set; }
    }
}
