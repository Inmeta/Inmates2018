using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Documents.Client;
using RavenAPI.Helpers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using RavenDatabase.Collections;

namespace RavenAPI.Controllers
{

    public class MessagesController : ApiController
    {
        //private DocumentClient client;

        public IQueryable<Message> Get()
        {
            return new MessageCollectionAPI().Get();
        }

        public IQueryable<Message> Get(string senderTenantName, string senderTenantId, string receiverTenantId, string receivertenantName)
        {
            return new MessageCollectionAPI().Get(senderTenantName,senderTenantId,receiverTenantId,receivertenantName);
        }

        public IQueryable<Message> Get(string messageid)
        {
            return new MessageCollectionAPI().Get(messageid);
        }

        public string Post([FromBody] Message postcontent)
        {
            return new MessageCollectionAPI().Post(postcontent);
        }

        public object Put([FromBody] Message value)
        {
            return new MessageCollectionAPI().Put(value);
        }
    }
}
