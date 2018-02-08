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
            var query = string.Format("SELECT * FROM c WHERE c.tenantTitle", tenant);
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Tenant> tenantQuery = this.client.CreateDocumentQuery<Tenant>(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Tenants"), query, queryOptions);
            return tenantQuery;
        }

        public object Post([FromBody] Tenant postcontent)
        {
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            return Request.CreateResponse(HttpStatusCode.OK,
                client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Tenants"),new Tenant { tenantTitle = postcontent.tenantTitle }));
        }
    }
}
