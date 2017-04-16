using System;

namespace denifia.stardew.webapi.Domain
{
    public class Message
    {
        public string Id { get; set; }
        public string FromPlayerId { get; set; }
        public string ToPlayerId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
