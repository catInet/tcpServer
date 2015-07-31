using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace tcpServerPublish
{
    class TableStorageConnector
    {
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                                           Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageConnectionString"));
        public static void SaveCaptchaToDb(string sid, string img)
        {
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("captchamessages");
            CaptchaEntity captcha = new CaptchaEntity();
            captcha.Sid = sid;
            captcha.Img = img;

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(captcha);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        public static string GetMasterID()
        {
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("bandcredentials");
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<BandCredentialsEntity>("band", "test");

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);
            return ((BandCredentialsEntity)retrievedResult.Result).masterID;
        }

    }

    public class CaptchaEntity : TableEntity
    {
        public CaptchaEntity()
        {
            this.PartitionKey = "captcha";

            long ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000;
            var timestamp = ticks / 10000000;

            this.RowKey = timestamp.ToString();
        }


        public string Sid { get; set; }

        public string Img { get; set; }
    }

    public class BandCredentialsEntity : TableEntity
    {
        public BandCredentialsEntity(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public BandCredentialsEntity()
        {
            this.PartitionKey = "band";
            this.RowKey = "test";
        }

        public string masterID { get; set; }
    }
}
