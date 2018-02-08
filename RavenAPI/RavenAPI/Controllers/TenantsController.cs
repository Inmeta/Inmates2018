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
    public class Tenant
    {
        public string title { get; set; }        
    }

    public class TenantsController : ApiController
    {
        private DocumentClient client;

        // GET: api/Messages
        public IQueryable<Tenant> Get()
        {
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Tenant> tenantQuery = this.client.CreateDocumentQuery<Tenant>(
                UriFactory.CreateDocumentCollectionUri("RavenCollection", "Tenants"),
                "SELECT * FROM Tenants",
                queryOptions);

            return tenantQuery;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public object Post([FromBody] Tenant postcontent)
        {            
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);

            return Request.CreateResponse(HttpStatusCode.OK,
               client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri("RavenCollection", "Tenants"),
                new Tenant
                {
                    title = postcontent.title                    
                })
           );
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
