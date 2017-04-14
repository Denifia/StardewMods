using System;
using System.Collections.Generic;

namespace denifia.stardew.sendletters
{
    public class ModConfig
    {
        public Uri ApiUrl { get; set; }
        public string YourUniqueId { get; set; }
        public string YourDisplayName { get; set; }
        public List<string> YourFriendsUniqueIds { get; set; }
    }
}