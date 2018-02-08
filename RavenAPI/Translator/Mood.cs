using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System;
using System.IO;

namespace Microsoft.Translator.Samples
{
    class MoodTest
    {
        public static void Sentiment(string text, string messageId, string toLanguage = "en")
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(text);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "79dd65f1796c482eb3b6062a91906c9a");

            var uri = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment?" + queryString;

            HttpResponseMessage response;

            // Request body
            string json = "{\"documents\": [{\"language\": \"en\", \"id\": \"" + messageId + "\",\"text\": \"" + text + "\"";
            byte[] byteData = Encoding.UTF8.GetBytes(json);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PostAsync(uri, content).GetAwaiter().GetResult();
            }
            Stream receiveStream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            Console.WriteLine(readStream.ReadToEnd());
        }
    }
}
