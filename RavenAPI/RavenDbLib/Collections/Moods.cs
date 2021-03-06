﻿using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using ProcessMessage;
using RavenDatabase.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace RavenDatabase.Collections
{

        public class Mood
        {
            public double Average { get; set; }
            public double Trend { get; set; }
            public int NumMessages { get; set; }

            [JsonProperty(PropertyName = "id")]
            public string id { get; set; }
            public string TenantName { get; set; }
        }

        public class GridEvent<T> where T : class
        {
            public string Id { get; set; }
            public string Subject { get; set; }
            public string EventType { get; set; }
            public T Data { get; set; }
            public DateTime EventTime { get; set; }
        }


    public class MoodCollectionAPI
    {
        private DocumentClient client;

        public void DispatchMoodUpdate(Message msg)
        {
            //HandleMoodUpdate(msg);
            string topicEndpoint = "https://inmateseventgrid.northeurope-1.eventgrid.azure.net/api/events";
            string sasKey = "rl4ZTyin7VJ/VdVisbJx07a4YRUcrlTmMXxon6qQaKk=";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("aeg-sas-key", sasKey);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("RavenAPI");

            List<GridEvent<Message>> eventList = new List<GridEvent<Message>>
            {
                new GridEvent<Message>
                {
                    Subject = "NewMessage",
                    EventTime = DateTime.UtcNow,
                    EventType = "UpdateMood",
                    Id = Guid.NewGuid().ToString(),
                    Data = msg
                }
            };

            string json = JsonConvert.SerializeObject(eventList);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, topicEndpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();
            HandleMoodUpdate(msg);
        }
    

            public void HandleMoodUpdate(Message msg)
            {
                IQueryable<Mood> mood = Get(msg.receiverTenantId);
                bool hasValues = false;

                var enMsg = Language.Translate(msg.messageContent);
                var newMsgMood = MoodService.Sentiment(enMsg, msg.id);


                foreach (Mood m in mood)
                {
                    m.NumMessages++;
                    m.Trend = (m.Trend + m.Average + newMsgMood.Documents[0].Score) / 3;
                    m.Average = (m.Average * (m.NumMessages - 1) + newMsgMood.Documents[0].Score) / m.NumMessages;
                    Put(m);

                    hasValues = true;
                }
                if (!hasValues)
                {
                    var nm = new Mood()
                    {
                        Average = 0.5,
                        Trend = 0.5,
                        NumMessages = 1,
                        TenantName = msg.receiverTenantName,
                        id = msg.receiverTenantId
                    };
                    Post(nm);
                }

            }

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
                var query = string.Format("SELECT * FROM m WHERE m.id='{0}'", tenantId);

                client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
                IQueryable<Mood> messageQuery = this.client.CreateDocumentQuery<Mood>
                    (UriFactory.CreateDocumentCollectionUri("RavenCollection", "Moods"), query, queryOptions);
                return messageQuery;
            }

            public string Post(Mood content)
            {
                var date = DateTime.Now;
                var guid = Guid.NewGuid().ToString();
                this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
                client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri("RavenCollection", "Moods"),
                    new Mood
                    {
                        Average = content.Average,
                        Trend = content.Trend,
                        NumMessages = content.NumMessages,
                        TenantName = content.TenantName,
                        id = content.id
                    });
                return guid;
            }

            public void Put(Mood content)
            {
                this.client = new DocumentClient(new Uri("https://ravendb.documents.azure.com:443/"), AuthHelper.CosmosKey);
                client.UpsertDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri("RavenCollection", "Moods"),
                    new Mood
                    {
                        Average = content.Average,
                        Trend = content.Trend,
                        NumMessages = content.NumMessages,
                        TenantName = content.TenantName,
                        id = content.id
                    });

            }
        }

 
}
