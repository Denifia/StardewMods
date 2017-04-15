using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using denifia.stardew.webapi.Domain;
using denifia.stardew.webapi.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace denifia.stardew.webapi.Controllers
{
    public class JsonContent : StringContent
    {
        public JsonContent(string content)
            : this(content, Encoding.UTF8)
        {
        }

        public JsonContent(string content, Encoding encoding)
            : base(content, encoding, "application/json")
        {
        }
    }

    [Route("api/[controller]")]
    public class MessagesController : Controller
    {

        Repository repo = Repository.Instance;

        // POST api/Messages/1
        [HttpPost("{playerId}")]
        public string Post(string playerId, [FromBody]MessageCreateModel message)
        {
            var response = "";
            var player = repo.Players.FirstOrDefault(x => x.Id == message.ToPlayerId);

            if (player != null)
            {
                var newMessage = new Message()
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = message.Message
                };
                player.Messages.Add(newMessage);

                repo.SaveDatabase();

                return newMessage.Id;
            }

            return response;
        }

        // GET api/Messages/1
        [HttpGet("{playerId}")]
        public List<Message> Get(string playerId)
        {
            var response = new HttpResponseMessage();
            var player = repo.Players.FirstOrDefault(x => x.Id == playerId);

            if (player != null)
            {
                return player.Messages;
            }

            return null;
        }

        // DELETE api/Messages/1
        [HttpDelete("{playerId}/{Id}")]
        public void Delete(string playerId, string Id)
        {
            var player = repo.Players.FirstOrDefault(x => x.Id == playerId);

            if (player != null)
            {
                player.Messages.RemoveAll(x => x.Id == Id);
                repo.SaveDatabase();
            }
        }
    }
}
