using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MoodProcessorFunctions
{
    public class GridEvent
    {
        public string Id { get; set; }
        public string EventType { get; set; }
        public string Subject { get; set; }
        public DateTime EventTime { get; set; }
        public object Data { get; set; }
        public string Topic { get; set; }
    }

    public static class Function1
    {

        [FunctionName("MoodFoodProcessor")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            // Get request body
            //dynamic data = await req.Content.ReadAsAsync<object>();
            dynamic data = await req.Content.ReadAsStringAsync();

            // Set name to query string or body data
            name = name ?? data?.name;

            var events = JsonConvert.DeserializeObject<GridEvent[]>(data);
            //if (req.Headers.GetValues("Aeg-Event-Type").FirstOrDefault() == "SubscriptionValidation")
            //{
            //    var code = events[0].Data["validationCode"];
            //    return req.CreateResponse(HttpStatusCode.OK,new { validationResponse = code });
            //}
            foreach (var item in events)
            {
                //log.Info(item);
                log.Info($"Event => {item.EventType} Subject => {item.Subject}\n");
                //log.Info(item);
                if (item.EventType == "UpdateMood")
                {
                    //new RavenDatabase.Collections.MoodsCollectionAPI().HandleMoodUpdate(item.Data);
                }
            }


            log.Info("C# HTTP trigger function processed a request.");

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, "Hello");


        }
    }

}


