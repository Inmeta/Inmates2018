using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Documents.Client;
using RavenAPI.Helpers;

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
    }

    public class MessagesController : ApiController
    {
        private DocumentClient client;   
        
        public IQueryable<Message> Get()
        {
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
           
            IQueryable<Message> messageQuery = this.client.CreateDocumentQuery<Message>(
                UriFactory.CreateDocumentCollectionUri("RavenCollection", "Messages"),
                "SELECT * FROM c",
                queryOptions);

            return messageQuery;
        }

        public IQueryable<Message> Get(string senderTenantName)
        {
            var query = string.Format("SELECT * FROM c WHERE c.senderTenantName='{0}'", senderTenantName);
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };            
            IQueryable<Message> messageQuery = this.client.CreateDocumentQuery<Message>(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Messages"), query, queryOptions);
            return messageQuery;
        }
       
        public object Post([FromBody] Message postcontent)
        {
            var guid = Guid.NewGuid().ToString();
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);

            return Request.CreateResponse(HttpStatusCode.OK,
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
                    senderUser = postcontent.senderUser
                })
           );
        }
    }
}
