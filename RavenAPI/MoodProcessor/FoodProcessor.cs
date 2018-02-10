using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace MoodProcessor
{
    public class GridEvent
    {
        public string Id { get; set; }
        public string EventType { get; set; }
        public string Subject { get; set; }
        public DateTime EventTime { get; set; }
        //public Message Data { get; set; }
        public string Topic { get; set; }
    }
    public static class MoodFoodProcessor
    {
        [FunctionName("MoodFoodProcessor")]
        public static async Task<object> Run(
            [HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req
            , TraceWriter log)
        {
            var data = await req.Content.ReadAsStringAsync();
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
                    //Controllers.MoodUpdater mu = new MoodUpdater();
                    //mu.ProcessFireMoodUpdate(item.Data);
                }
            }


            log.Info("C# HTTP trigger function processed a request.");

            // Fetching the name from the path parameter in the request URL
            return req.CreateResponse(HttpStatusCode.OK, "Hello");
        }
 


    }
}
