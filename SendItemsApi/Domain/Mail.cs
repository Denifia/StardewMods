using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Denifia.Stardew.SendItemsApi.Domain
{
    public class Mail : TableEntity
    {
        public Mail()
        {

        }
        public Mail(string id, string toFarmerId)
        {
            RowKey = id;
            PartitionKey = toFarmerId;
        }

        public string Id { get { return  RowKey; } }
        public string ToFarmerId { get { return PartitionKey; } }
        public string FromFarmerId { get; set; }
        public string Text { get; set; }
        public DateTime ClientCreatedDate { get; set; }
        public DateTime ServerCreatedDate { get; set; }
    }
}
