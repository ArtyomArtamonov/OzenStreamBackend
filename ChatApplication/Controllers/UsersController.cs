using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ChatApplication.Controllers
{

    public class IndexController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
    [Route("[controller]")]
    public class UsersController : Controller
    {
        [HttpGet]
        public string Get(string key)
        {
            if (MongoDatabase.hasAccess(key))
            {
                return JsonConvert.SerializeObject(MongoDatabase.GetUsers());
            }
            return "No access";
        }
        [HttpGet]
        public string GetByPhone(string phone, string key)
        {
            if (MongoDatabase.hasAccess(key))
                return JsonConvert.SerializeObject(MongoDatabase.GetByPhone(phone));
            return "No access";
        }
        [HttpGet]
        public bool Auth(string phone, string password, string key)
        {
            if (MongoDatabase.hasAccess(key))
                return MongoDatabase.Auth(phone, password);
            return false;
        }
        [HttpGet]
        public async Task<bool> Insert(string username, string phone, string password, string key)
        {
            if (MongoDatabase.hasAccess(key))
            {
                if(MongoDatabase.GetByPhone(phone) == null)
                {
                    return await MongoDatabase.SaveUser(username: username, phone: phone, password: password);
                }
                return false;
            }
                
            return false;
        }
        [HttpGet]
        public async Task<bool> Remove(string phone, string key)
        {
            if (MongoDatabase.hasAccess(key))
                return await MongoDatabase.RemoveUser(phone);
            return false;
        }
    }
}
