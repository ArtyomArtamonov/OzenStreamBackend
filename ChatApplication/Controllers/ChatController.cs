using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChatApplication.Controllers
{
    [Route("[controller]")]
    public class ChatController : Controller
    {
        // Users online configuration
        [HttpGet]
        public async Task<bool> MakeOnline(string phone, string key)
        {
            if (MongoDatabase.hasAccess(key))
                return await MongoDatabase.MakeOnline(phone);
            return false;
        }
        [HttpGet]
        public async Task<bool> MakeOffline(string phone, string key)
        {
            if (MongoDatabase.hasAccess(key))
                return await MongoDatabase.MakeOffline(phone);
            return false;
        }
        [HttpGet]
        public int GetOnline(string key)
        {
            if (MongoDatabase.hasAccess(key))
                return MongoDatabase.GetOnline();
            return -1;
        }

        // Chat configuration
        [HttpGet]
        public string GetMessages(string key)
        {
            if (MongoDatabase.hasAccess(key))
                return JsonConvert.SerializeObject(MongoDatabase.GetMessages());
            return "No access";
        }
        [HttpGet]
        public async Task<bool> WriteMessage(string username, string content, string date, string key)
        {
            if (MongoDatabase.hasAccess(key))
                return await MongoDatabase.WriteMessage(username, content, date);
            return false;
        }
    }
}