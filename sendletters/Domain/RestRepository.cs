using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;

namespace Denifia.Stardew.SendLetters.Domain
{
    public class RestRepository : Repository
    {
        public RestRepository(IModHelper modHelper, IMemoryCache memoryCache) 
            : base(modHelper, memoryCache)
        {
        }


    }
}
