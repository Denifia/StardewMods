using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using LiteDB;
using Denifia.Stardew.SendItems.Api.Domain;
using Denifia.Stardew.SendItems.Api.Models;

namespace Denifia.Stardew.SendLetters.webapi.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        private const string connectionString = "data.db";

        // GET api/mail/{mailId}
        [HttpGet("{mailId}")]
        public async Task<Mail> Get(Guid mailId)
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(connectionString))
                {
                    return db.Query<Mail>().Where(x => x.Id == mailId).FirstOrDefault();
                }
            });
        }

        // GET api/mail/to/{farmerId}
        [HttpGet("to/{farmerId}")]
        public async Task<List<Mail>> GetMailToFarmer(string farmerId)
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(connectionString))
                {
                    return db.Query<Mail>().Where(x => x.ToFarmerId == farmerId).ToList();
                }
            });
        }

        // GET api/mail/from/{farmerId}
        [HttpGet("from/{farmerId}")]
        public async Task<List<Mail>> GetMailFromFarmer(string farmerId)
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(connectionString))
                {
                    return db.Query<Mail>().Where(x => x.FromFarmerId == farmerId).ToList();
                }
            });
        }

        // POST api/mail
        [HttpPost]
        public async Task<Guid> Post([FromBody]CreateMailModel model)
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(connectionString))
                {
                    var mail = new Mail()
                    {
                        Id = Guid.NewGuid(),
                        Text = model.Text,
                        ToFarmerId = model.ToFarmerId,
                        FromFarmerId = model.FromFarmerId,
                        CreatedDate = DateTime.Now
                    };

                    db.Insert(mail);

                    return mail.Id;
                }
            });
        }

        // DELETE api/mail/{mailId}
        [HttpDelete("{mailId}")]
        public async Task<bool> Delete(Guid mailId)
        {
            return await Task.Run(() =>
            {
                using (var db = new LiteRepository(connectionString))
                {
                    var deletedCount = db.Delete<Mail>(x => x.Id == mailId);
                    return deletedCount > 0;
                }
            });
        }
    }
}
