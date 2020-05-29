using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types


namespace GetPresenceAndUpdate
{


    public class TableStorage
    {
        CloudTable table;


        public async Task Initialise()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("YOU PUT YOUR TABLE STORAGE CONNECTION STRING IN HERE");
            CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
            table = cloudTableClient.GetTableReference("presence");
            await table.CreateIfNotExistsAsync().ConfigureAwait(false);
        }

        public async Task AddPresenceInfo(PresenceInfo info)
        {
            info.PartitionKey = DateTime.UtcNow.Date.ToString("R");
            info.RowKey = Guid.NewGuid().ToString();
            TableOperation insertOperation = TableOperation.Insert(info);
            await table.ExecuteAsync(insertOperation);
        }

    }
}
