using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Denifia.Stardew.SendItemsApi.Domain
{
    public class Mail : TableEntity
    {
        public Mail()
        {

        }
        public Mail(Guid id, string toFarmerId)
        {
            Id = id;
            ToFarmerId = toFarmerId;

            RowKey = Id.ToString();
            PartitionKey = ToFarmerId;
        }

        public Guid Id { get; private set; }
        public string ToFarmerId { get; private set; }
        public string FromFarmerId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
