using System;

namespace Denifia.Stardew.SendItemsApi.Models
{
    public class CreateMailModel
    {
        public string FromFarmerId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
