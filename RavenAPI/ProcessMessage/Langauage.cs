using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Web;

namespace ProcessMessage
{
    class Language
    {
        public static string Translate(string text, string toLanguage = "en")
        {
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(text)
                //+ "&from=" + from 
                + "&to=" + toLanguage;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            string token = TokenService.GetToken();
            httpWebRequest.Headers.Add("Authorization", token);
            using (WebResponse response = httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                string translation = (string)dcs.ReadObject(stream);
                return translation;
            }
        }
    }
}
