using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denifia.stardew.sendletters.Domain
{
    public class Friend
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FarmName { get; set; }

        public bool IsRemote { get; set; }
    }
}
