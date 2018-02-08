using System;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace Microsoft.Translator.Samples
{
    class GetTranslationsSample
    {
        public static void TranslateContent(string authToken)
        {
            string text = "Ja det er ikke enkelt å løse to språk på en gang. Det er rett og slett ikke rett fram.";
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/GetTranslations?text="
                + text 
                //+ "&from=no" 
                + "&to=en" 
                + "&maxTranslations=5";
            string requestBody = GenerateTranslateOptionsRequestBody("general", "text/plain", "true", "MsgId", "", "");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("Authorization", authToken);
            request.ContentType = "text/xml";
            request.Method = "POST";
            using (Stream stream = request.GetRequestStream())
            {
                byte[] arrBytes = Encoding.ASCII.GetBytes(requestBody);
                stream.Write(arrBytes, 0, arrBytes.Length);
            }
            using (WebResponse response = request.GetResponse())
            using (Stream respStream = response.GetResponseStream())
            {
                StreamReader rdr = new StreamReader(respStream, System.Text.Encoding.ASCII);
                string strResponse = rdr.ReadToEnd();

                Console.WriteLine("Available translations for source text '{0}' are", text);
                XDocument doc = XDocument.Parse(@strResponse);
                XNamespace ns = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2";
                int i = 1;
                foreach (XElement xe in doc.Descendants(ns + "GetTranslationsResponse"))
                {
                    foreach (var node in xe.Elements())
                    {
                        Console.WriteLine("{0} = {1}", node.Name.LocalName, node.Value);
                    }
                }

                foreach (XElement xe in doc.Descendants(ns + "TranslationMatch"))
                {
                    Console.WriteLine("{0}Result {1}", Environment.NewLine, i++);
                    foreach (var node in xe.Elements())
                    {
                        Console.WriteLine("{0} = {1}", node.Name.LocalName, node.Value);
                    }
                }
            }
        }

        // builds the outer XML body
        private static string GenerateTranslateOptionsRequestBody(string category, string contentType, string reservedFlags, string state, string uri, string user)
        {
            string body =
                "<TranslateOptions xmlns=\"http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2\">" +
                "  <Category>{0}</Category>" +
                "  <ContentType>{1}</ContentType>" +
                "  <IncludeMultipleMTAlternatives>{2}</IncludeMultipleMTAlternatives>" +
                "  <State>{3}</State>" +
                "  <Uri>{4}</Uri>" +
                "  <User>{5}</User>" +
                "</TranslateOptions>";
            return string.Format(body, category, contentType, reservedFlags, state, uri, user);
        }
    }
}
