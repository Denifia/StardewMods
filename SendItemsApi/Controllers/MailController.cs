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

        // GET api/mail/{mailId}
        [HttpGet("{mailId}")]
        public async Task<Mail> Get(Guid mailId)
        {
            return await Task.Run(() =>
            {
                return Repository.Instance.FirstOrDefault<Mail>(x => x.Id == mailId);
            });
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
            //return await Task.Run(() =>
            //{
            //    return Repository.Instance.Fetch<Mail>().Count;
            //});
        }

        // POST api/mail
        [HttpPost]
        public async Task<Guid> Post([FromBody]CreateMailModel model)
        {
            return await Task.Run(() =>
            {
                //var mail = new Mail()
                //{
                //    Id = Guid.NewGuid(),
                //    Text = model.Text,
                //    ToFarmerId = model.ToFarmerId,
                //    FromFarmerId = model.FromFarmerId,
                //    CreatedDate = DateTime.Now
                //};

                //Repository.Instance.Insert(mail);
                //return mail.Id;
                return Guid.NewGuid();
            });
        }

        // DELETE api/mail/{mailId}
        [HttpDelete("{mailId}")]
        public async Task<bool> Delete(Guid mailId)
        {
            return await Task.Run(() =>
            {
                var deletedCount = Repository.Instance.Delete<Mail>(x => x.Id == mailId);
                return deletedCount > 0;
            });
        }
    }
}
