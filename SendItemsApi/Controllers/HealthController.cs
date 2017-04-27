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
    public class HealthController : Controller
    {
        // GET api/health
        [HttpGet()]
        public bool Get()
        {
            return true;
        }
    }
}
