using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using LiteDB;
using Denifia.Stardew.SendItemsApi.Domain;
using Denifia.Stardew.SendItemsApi.Models;

namespace Denifia.Stardew.SendItemsApi.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        private readonly ITableStorageRepository _repository;

        public MailController(ITableStorageRepository repository)
        {
            _repository = repository;
        }

        // GET api/mail/{toFarmerId}/{mailId}
        [HttpGet("{toFarmerId}/{mailId}")]
        public async Task<Mail> Get(string toFarmerId, string mailId)
        {
            return await _repository.Retrieve<Mail>(toFarmerId, mailId);
        }

        // GET api/mail/to/{farmerId}
        [HttpGet("to/{farmerId}")]
        public async Task<List<Mail>> GetMailToFarmer(string farmerId)
        {
            return await Task.Run(() =>
            {
                return Repository.Instance.Fetch<Mail>(x => x.ToFarmerId == farmerId);
            });
        }

        // GET api/mail/from/{farmerId}
        [HttpGet("from/{farmerId}")]
        public async Task<List<Mail>> GetMailFromFarmer(string farmerId)
        {
            return await Task.Run(() =>
            {
                return Repository.Instance.Fetch<Mail>(x => x.FromFarmerId == farmerId);
            });
        }

        // GET api/mail/count
        [HttpGet("count")]
        public async Task<int> GetMailCount()
        {
            return await _repository.Count<Mail>();
        }

        // PUT api/mail/{toFarmerId}/{mailId}
        [HttpPut("{toFarmerId}/{mailId}")]
        public async Task<bool> Put(string toFarmerId, string mailId, [FromBody]CreateMailModel model)
        {
            var mail = new Mail(mailId, toFarmerId)
            {
                Text = model.Text,
                FromFarmerId = model.FromFarmerId,
                ClientCreatedDate = model.CreatedDate,
                ServerCreatedDate = DateTime.Now.ToUniversalTime()
            };
            return await _repository.InsertOrReplace(mail);
        }

        // DELETE api/mail/{toFarmerId}/{mailId}
        [HttpDelete("{toFarmerId}/{mailId}")]
        public async Task<bool> Delete(string toFarmerId, string mailId)
        {
            var mail = new Mail(mailId, toFarmerId);
            return await _repository.Delete(mail);
        }
    }
}
