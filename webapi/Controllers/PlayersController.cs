using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using denifia.stardew.webapi.Domain;
using denifia.stardew.webapi.Models;

namespace denifia.stardew.webapi.Controllers
{
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {

        Repository repo = Repository.Instance;

        // PUT api/home/5
        [HttpPut("{id}")]
        public HttpResponseMessage Put(string id, [FromBody]PlayerCreateModel player)
        {
            var response = new HttpResponseMessage();
            if (!repo.Players.Any(x => x.Id == id))
            {
                repo.Players.Add(new Player
                {
                    Id = id,
                    Name = player.Name
                });
                response.StatusCode = HttpStatusCode.Created;
            }
            else
            {
                response.StatusCode = HttpStatusCode.Conflict;
            }

            return response;
        }

        // GET api/home
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(repo.Players);
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
