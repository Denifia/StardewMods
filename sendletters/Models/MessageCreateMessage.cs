using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using denifia.stardew.sendletters.Domain;
using denifia.stardew.sendletters.common.Domain;

namespace denifia.stardew.sendletters.Models
{
    public class MessageCreateMessage
    {
        public string FromPlayerId { get; set; }
        public string ToPlayerId { get; set; }
        public string Text { get; set; }
        public Message Message { get; internal set; }
    }
}
