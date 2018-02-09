using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Documents.Client;
using RavenAPI.Helpers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace RavenAPI.Controllers
{
    public class Message
    {
        public string messageContent { get; set; }
        public string messageId { get; set; }
        public string senderTenantId { get; set; }
        public string senderTenantName { get; set; }
        public string senderUser { get; set; }
        public string receiverTenantId { get; set; }
        public string receiverTenantName { get; set; }
        public string senderLanguage { get; set; }
        public int priority { get; set; }
        public bool riskOfWar { get; set; }
        public DateTime messageTimestamp { get; set; }
        public string hashmessage { get; set; }
        public bool tampered { get; set; }
        public string id { get; set; }
    }

    public class MessagesController : ApiController
    {
        private DocumentClient client;

        public IQueryable<Message> Get()
        {
            var query = "SELECT * FROM c";
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Message> messageQuery = this.client.CreateDocumentQuery<Message>(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Messages"), query, queryOptions);
            return messageQuery;
        }

        public IQueryable<Message> Get(string senderTenantName, string senderTenantId, string receiverTenantId, string receivertenantName)
        {
            var query = "";
            if (!String.IsNullOrEmpty(senderTenantName))
                query = string.Format("SELECT * FROM c WHERE c.senderTenantName='{0}'", senderTenantName);
            else if (!String.IsNullOrEmpty(senderTenantId))
                query = string.Format("SELECT * FROM c WHERE c.senderTenantId='{0}'", senderTenantId);
            else if (!String.IsNullOrEmpty(receiverTenantId))
                query = string.Format("SELECT * FROM c WHERE c.receiverTenantId='{0}'", receiverTenantId);
            else if (!String.IsNullOrEmpty(receivertenantName))
                query = string.Format("SELECT * FROM c WHERE c.receiverTenantName='{0}'", receivertenantName);

            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Message> messageQuery = this.client.CreateDocumentQuery<Message>(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Messages"), query, queryOptions);
            return messageQuery;
        }

        public IQueryable<Message> Get(string messageid)
        {
            var query = string.Format("SELECT * FROM c WHERE c.messageId='{0}'", messageid);
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Message> messageQuery = this.client.CreateDocumentQuery<Message>(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Messages"), query, queryOptions);
            return messageQuery;
        }

        public string Post([FromBody] Message postcontent)
        {
            var date = DateTime.Now;
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);

            HashAlgorithm alg = MD5.Create();
            var hashed = alg.ComputeHash(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postcontent)));

            var hashstring = Convert.ToBase64String(hashed);

            client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri("RavenCollection", "Messages"),
                new Message
                {
                    messageContent = postcontent.messageContent,
                    messageId = guid,
                    priority = postcontent.priority,
                    receiverTenantId = postcontent.receiverTenantId,
                    receiverTenantName = postcontent.receiverTenantName,
                    riskOfWar = postcontent.riskOfWar,
                    senderLanguage = postcontent.senderLanguage,
                    senderTenantId = postcontent.senderTenantId,
                    senderTenantName = postcontent.senderTenantName,
                    senderUser = postcontent.senderUser,
                    messageTimestamp = date,
                    id = postcontent.id,
                    hashmessage = hashstring,
                    tampered = postcontent.tampered
                });
            //MoodUpdater mc = new MoodUpdater();
            //mc.UpdateMoodForReceiver(postcontent);
            return guid;
        }

        public object Put([FromBody] Message value)
        {
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            
            client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Messages"), 
            new Message
            {
                messageContent = value.messageContent,
                messageId = value.messageId,
                priority = value.priority,
                receiverTenantId = value.receiverTenantId,
                receiverTenantName = value.receiverTenantName,
                riskOfWar = value.riskOfWar,
                senderLanguage = value.senderLanguage,
                senderTenantId = value.senderTenantId,
                senderTenantName = value.senderTenantName,
                senderUser = value.senderUser,
                messageTimestamp = value.messageTimestamp,
                id = value.id
            });
            return value;
        }
    }
}
