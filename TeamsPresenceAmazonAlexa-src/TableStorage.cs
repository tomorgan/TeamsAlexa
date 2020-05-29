using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

namespace TeamsPresenceBot
{

    public class TableStorage
    {
        CloudTable table;


        public async Task Initialise()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("YOU PUT YOUR TABLE STORAGE CONNECTION STRING IN HERE");
            CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
            table = cloudTableClient.GetTableReference("presence");
        }

        public async Task<List<PresenceInfo>> GetTodaysPresenceInfo()
        {
            TableQuery<PresenceInfo> query = new TableQuery<PresenceInfo>()
                     .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, DateTime.UtcNow.Date.ToString("R")));
            TableContinuationToken token = null;
            var entities = new List<PresenceInfo>();
            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(new TableQuery<PresenceInfo>(), token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities;
        }
    }

}



