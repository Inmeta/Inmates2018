using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Documents.Client;
using RavenAPI.Helpers;
using ProcessMessage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using RavenDatabase.Collections;

namespace RavenAPI.Controllers
{
    public class GridEvent<T> where T : class
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string EventType { get; set; }
        public T Data { get; set; }
        public DateTime EventTime { get; set; }
    }

    public class MoodsController : ApiController
    {
        //private DocumentClient client;   
        
        public IQueryable<Mood> Get()
        {
            return new MoodCollectionAPI().Get();
        }

        public IQueryable<Mood> Get(string tenantId)
        {
            return new MoodCollectionAPI().Get(tenantId);
        }
       
        public string Post([FromBody] Mood content)
        {
            return new MoodCollectionAPI().Post(content);
        }

        public void Put([FromBody]Mood content)
        {
            new MoodCollectionAPI().Put(content);
        }
    }
}
