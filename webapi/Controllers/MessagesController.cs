using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Denifia.Stardew.SendLetters.Common.Models;
using Denifia.Stardew.SendLetters.Common.Domain;

namespace Denifia.Stardew.SendLetters.webapi.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IRepository _repository;

        public MessagesController(IRepository repository)
        {
            _repository = repository;
        }

        // GET api/Messages/{playerid}
        [HttpGet("{messageId}")]
        public async Task<Message> Get(string messageId)
        {
            var messages = await _repository.GetAllAsync<Message>();
            return messages.FirstOrDefault(x => x.Id == messageId);
        }

        // GET api/Messages/toplayer/{playerid}
        [Route("~/api/Messages/ToPlayer/{playerId}")]
        [HttpGet("{playerId}")]
        public async Task<List<Message>> ToPlayer(string playerId)
        {
            var messages = await _repository.GetAllAsync<Message>();
            return messages.Where(x => x.ToPlayerId == playerId).ToList();
        }

        // GET api/Messages/fromplayer/{playerid}
        [Route("~/api/Messages/FromPlayer/{playerId}")]
        [HttpGet("{playerId}")]
        public async Task<List<Message>> FromPlayer(string playerId)
        {
            var messages = await _repository.GetAllAsync<Message>();
            return messages.Where(x => x.FromPlayerId == playerId).ToList();
        }

        // POST api/Messages
        [HttpPost]
        public async Task<string> Post([FromBody]MessageCreateModel model)
        {
            var message = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                Text = model.Text,
                ToPlayerId = model.ToPlayerId,
                FromPlayerId = model.FromPlayerId,
                CreatedDate = DateTime.Now
            };

            var newMessage = await _repository.AddOrUpdateAsync(message);
            return newMessage.Id;
        }

        // DELETE api/Messages/{messageId}
        [HttpDelete("{messageId}")]
        public async Task Delete(string messageId)
        {
            var messages = await _repository.GetAllAsync<Message>();
            var messageToDelete = messages.FirstOrDefault(x => x.Id == messageId);
            if (messageToDelete != null)
            {
                await _repository.DeleteAsync(messageToDelete);
            }
        }
    }
}
