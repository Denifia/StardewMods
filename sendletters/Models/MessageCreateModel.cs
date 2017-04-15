using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Models
{
    public class MessageCreateModel
    {
        public string FromPlayerId { get; set; }

        public string ToPlayerId { get; set; }

        public string Text { get; set; }
    }
}
