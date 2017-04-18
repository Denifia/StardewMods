using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Denifia.Stardew.SendLetters.Domain;
using Denifia.Stardew.SendLetters.common.Domain;

namespace Denifia.Stardew.SendLetters.Models
{
    public class MessageCreateMessage
    {
        public string FromPlayerId { get; set; }
        public string ToPlayerId { get; set; }
        public string Text { get; set; }
        public Message Message { get; internal set; }
    }
}
