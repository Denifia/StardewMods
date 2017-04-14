using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace denifia.stardew.webapi.Models
{
    public class MessageCreateModel
    {
        public string ToPlayerId { get; set; }

        public string Message { get; set; }
    }
}
