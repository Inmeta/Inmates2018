﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Documents.Client;
using RavenAPI.Helpers;
using ProcessMessage;
using Newtonsoft.Json;

namespace RavenAPI.Controllers
{
    public class Mood
    {
        public double Average { get; set; }
        public double Trend { get; set; }
        public int NumMessages { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string TenantId { get; set; }
    }
    public class MoodUpdater
    {
        public void UpdateMoodForReceiver(Message msg)
        {
            var mc = new MoodsController();
            IQueryable<Mood> mood = mc.Get(msg.receiverTenantId);
            Mood m;
            if (mood.Select(x => x.TenantId == msg.receiverTenantId) is null)
            {
                var nm = new Mood()
                {
                    Average = 0.5,
                    Trend = 0.5,
                    NumMessages = 1,
                    TenantId = msg.receiverTenantId
                };
                mc.Post(nm);
                m = nm;
            }
            else
            {
                m = mood.First();
            }

            var enMsg = Language.Translate(msg.messageContent);
            var newMsgMood = MoodService.Sentiment(enMsg, msg.messageId);

            m.NumMessages++;
            m.Trend = (m.Trend + m.Average + newMsgMood.Documents[0].Score) / 3;
            m.Average = (m.Average * (m.NumMessages - 1) + newMsgMood.Documents[0].Score) / m.NumMessages;
            mc.Put(m);
        }
    }

    public class MoodsController : ApiController
    {
        private DocumentClient client;   
        
        public IQueryable<Mood> Get()
        {
            var query = "SELECT * FROM m";
            client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };           
            IQueryable<Mood> messageQuery = client.CreateDocumentQuery<Mood>
                (UriFactory.CreateDocumentCollectionUri("RavenCollection", "Moods"), query, queryOptions);
            return messageQuery;
        }

        public IQueryable<Mood> Get(string tenantId)
        {
            var query = string.Format("SELECT * FROM m WHERE m.tenantId='{0}'", tenantId);

            client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<Mood> messageQuery = this.client.CreateDocumentQuery<Mood>
                (UriFactory.CreateDocumentCollectionUri("RavenCollection", "Moods"), query, queryOptions);
            return messageQuery;
        }
       
        public string Post([FromBody] Mood postcontent)
        {
            var date = DateTime.Now;
            var guid = Guid.NewGuid().ToString();
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);


            client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri("RavenCollection", "Moods"),
                new Mood
                {
                    Average = postcontent.Average,
                    Trend = postcontent.Trend,
                    NumMessages = postcontent.NumMessages,
                    TenantId = postcontent.TenantId
                });
            return guid;
        }

        public void Put([FromBody]Mood value)
        {
            this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
            client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri("RavenCollection", "Moods"), value);
        }


    }
}