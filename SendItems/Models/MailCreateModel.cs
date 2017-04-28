using System;

namespace Denifia.Stardew.SendItems.Models
{
    public class MailCreateModel
    {
        public string ToFarmerId { get; set; }
        public string FromFarmerId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
