using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using denifia.stardew.sendletters.webapi.Domain;
using System.Collections.Generic;
using denifia.stardew.sendletters.common.Models;

namespace denifia.stardew.sendletters.webapi.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        Repository repo = Repository.Instance;

        // GET api/Messages/{playerid}
        [HttpGet("{messageId}")]
        public Message Get(string messageId)
        {
            var message = repo.Messages.Where(x => x.Id == messageId).FirstOrDefault();
            return message;
        }

        // GET api/Messages/toplayer/{playerid}
        [Route("~/api/Messages/ToPlayer/{playerId}")]
        [HttpGet("{playerId}")]
        public List<Message> ToPlayer(string playerId)
        {
            var messages = repo.Messages.Where(x => x.ToPlayerId == playerId).ToList();
            return messages;
        }

        // GET api/Messages/fromplayer/{playerid}
        [Route("~/api/Messages/FromPlayer/{playerId}")]
        [HttpGet("{playerId}")]
        public List<Message> FromPlayer(string playerId)
        {
            var messages = repo.Messages.Where(x => x.FromPlayerId == playerId).ToList();
            return messages;
        }

        // POST api/Messages
        [HttpPost]
        public string Post([FromBody]MessageCreateModel model)
        {
            var message = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                Text = model.Text,
                ToPlayerId = model.ToPlayerId,
                FromPlayerId = model.FromPlayerId,
                CreatedDate = DateTime.Now
            };

            repo.Messages.Add(message);
            repo.SaveDatabase();
            return message.Id;
        }

        // DELETE api/Messages/{messageId}
        [HttpDelete("{messageId}")]
        public void Delete(string messageId)
        {
            repo.Messages.RemoveAll(x => x.Id == messageId);
            repo.SaveDatabase();
        }
    }
}
