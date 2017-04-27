using System;

namespace Denifia.Stardew.SendItems.Domain
{
    public class Mail : IEquatable<Mail>
    {
        public Guid Id { get; set; }
        public string FromFarmerId { get; set; }
        public string ToFarmerId { get; set; }
        public string Text { get; set; }
        public MailStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool Equals(Mail other)
        {
            return Id == other.Id;
        }
    }
}
