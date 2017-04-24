using System;

namespace Denifia.Stardew.SendItems.Domain
{
    public class Mail
    {
        public Guid Id { get; set; }
        public string FromFarmerId { get; set; }
        public string ToFarmerId { get; set; }
        public string Text { get; set; }
        public MailStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
