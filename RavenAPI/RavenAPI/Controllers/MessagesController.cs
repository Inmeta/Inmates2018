using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
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

        // GET: api/Messages
        public IQueryable<Message> Get()
        {
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Message> messageQuery = this.client.CreateDocumentQuery<Message>(
                UriFactory.CreateDocumentCollectionUri("RavenCollection", "Messages"),
                "SELECT * FROM Messages",
                queryOptions);

            return messageQuery;
        }

        // GET: api/Messages/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Messages
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

        // PUT: api/Messages/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE: api/Messages/5
        public void Delete(int id)
        {
        }
    }
}
