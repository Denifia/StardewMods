using System.Collections.Generic;

namespace Denifia.Stardew.SendItems.Domain
{
    public class Farmer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FarmName { get; set; }
        public List<Friend> Friends { get; set; }

        public string DisplayText
        {
            get;
        }
    }
}
