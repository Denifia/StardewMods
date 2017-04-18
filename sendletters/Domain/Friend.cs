using Denifia.Stardew.SendLetters.Common.Domain;
using System.Collections.Generic;

namespace Denifia.Stardew.SendLetters.Domain
{
    public class Friend : Entity
    {
        public string Name { get; set; }
        public string FarmName { get; set; }
        public List<string> Players { get; set; }
    }
}
