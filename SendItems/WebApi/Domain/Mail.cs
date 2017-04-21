using System;

namespace Denifia.Stardew.SendItems.Api.Domain
{
    public class Mail
    {
        public string Id { get; set; }
        public string FromFarmerId { get; set; }
        public string ToFarmerId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
