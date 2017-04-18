using System;

namespace Denifia.Stardew.SendLetters.common.Domain
{
    public class Message : Entity
    {
        public string FromPlayerId { get; set; }
        public string ToPlayerId { get; set; }
        public string Text { get; set; }
    }
}
