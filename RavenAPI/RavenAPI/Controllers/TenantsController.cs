using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Documents.Client;
using RavenAPI.Helpers;

namespace RavenAPI.Controllers
{
    public class Tenant
    {
        public string tenantTitle { get; set; }
        public string tenantName { get; set; }
        public string tenantId { get; set; }
        public string id { get; set; }
    }

    public class TenantsController : ApiController
    {
        private DocumentClient client;

        public IQueryable<Tenant> Get()
        {
            var query = "SELECT * FROM c";
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Tenant> tenantQuery = this.client.CreateDocumentQuery<Tenant>(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Tenants"), query, queryOptions);
            return tenantQuery;
        }

        public IQueryable<Tenant> Get(string tenant)
        {
            var query = string.Format("SELECT * FROM c WHERE c.tenantTitle='{0}'", tenant);
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Tenant> tenantQuery = this.client.CreateDocumentQuery<Tenant>(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Tenants"), query, queryOptions);
            return tenantQuery;
        }        

        public string Post([FromBody] Tenant postcontent)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri("RavenCollection", "Tenants"),
                new Tenant
                {
                    tenantTitle = postcontent.tenantTitle,
                    tenantId = guid,
                    tenantName = postcontent.tenantName,
                    id = postcontent.id
                });

            return guid;
        }
    }
}
