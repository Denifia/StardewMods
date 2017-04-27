using System;

namespace Denifia.Stardew.SendItems.Domain
{
    public class Friend : IEquatable<Friend>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FarmName { get; set; }

        public string DisplayText
        {
            get;
        }

        public bool Equals(Friend other)
        {
            return Id == other.Id;
        }
    }
}
