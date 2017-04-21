using System.Collections.Generic;

namespace Denifia.Stardew.SendItems.Domain
{
    public class Farmer
    {
        public string Name { get; set; }
        public string FarmName { get; set; }
        public List<Friend> Friends { get; set; }
    }
}
