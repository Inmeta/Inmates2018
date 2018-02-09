using System;
using System.Linq;
using System.Web.Http;

// ADD THIS PART TO YOUR CODE
using Microsoft.Azure.Documents.Client;
using RavenAPI.Helpers;


namespace RavenAPI.Controllers
{
    public class Tenants
    {
        public string name { get; set; }    
    }

    public class ValuesController : ApiController
    {
        private DocumentClient client;


        // GET api/values
        public IQueryable<Tenants> Get()
        {
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            IQueryable<Tenants> tenantsQuery = this.client.CreateDocumentQuery<Tenants>(
                UriFactory.CreateDocumentCollectionUri("RavenCollection", "Tenants1"),
                "SELECT * FROM Tenants1", 
                queryOptions);
            
            return tenantsQuery;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
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
