using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using ChatApplication.Structures;

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
        public async Task<InsertStructure> Insert(string username, string phone, string password, string key)
        {
            if (MongoDatabase.hasAccess(key))
            {
                    if( await MongoDatabase.SaveUser(username: username, phone: phone, password: password))
                    {
                        return new InsertStructure
                        {
                            status = "200",
                            response = true
                        };
                    }
            }
            return new InsertStructure
            {
                status = "500",
                response = false
            };
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
