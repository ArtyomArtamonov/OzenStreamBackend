using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatApplication
{
    public static class MongoDatabase
    {
        public static MongoClient Client { get; set; }
        public static IMongoDatabase Database { get; set; }
        public static IMongoCollection<User> UserCollection { get; set; }
        public static IMongoCollection<ChatMessages> ChatMessagesCollection { get; set; }
        public static IMongoCollection<ChatOnline> ChatOnlineCollection { get; set; }

        private static string key = "f88f06249fa960bc16280bb38faed80735e35beedf0af4f664a5516c993ecd49370431af6c70b8f9abd579153456a7c9a62945dc6fa803833467c9c7fbd69c0d";

        public static bool hasAccess(string key)
        {
            if(MongoDatabase.key == key)
            {
                return true;
            }
            return false;
        }

        public static void Init()
        {
            if (Client == null || Database == null || UserCollection == null || ChatMessagesCollection == null || ChatOnlineCollection == null)
            {
                Client = new MongoClient("mongodb+srv://Art3A:Vdovhenko10@ozenstream-mguhk.mongodb.net/test?retryWrites=true&w=majority");
                Database = Client.GetDatabase("Ozen0");
                UserCollection = Database.GetCollection<User>("Users");
                ChatOnlineCollection = Database.GetCollection<ChatOnline>("ChatOnline");
                ChatMessagesCollection = Database.GetCollection<ChatMessages>("ChatMessages");
            }
            if (ChatOnlineCollection.Find(_ => true).FirstOrDefault() == null)
            {
                ChatOnlineCollection.InsertOne(new ChatOnline { id = 0, online = new List<string>() });
            }
        }

        public static IEnumerable<User> GetUsers()
        {
            Init();
            return UserCollection.Find(_ => true).ToEnumerable();
        }

        public static User GetByPhone(string phone)
        {
            Init();
            return UserCollection.Find(x => x.phone == phone).FirstOrDefault();
        }

        public static bool Auth(string phone, string password)
        {
            Init();
            try
            {
                return UserCollection.Find(x => x.phone == phone).FirstOrDefault().password == password;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> MakeOnline(string phone)
        {
            Init();
            try
            {
                if (GetByPhone(phone) != null)
                {
                    if (!ChatOnlineCollection.Find(_ => true).FirstOrDefault().online.Contains(phone))
                    {
                        var document = ChatOnlineCollection.Find(_ => true).FirstOrDefault();
                        document.online.Add(phone);
                        ChatOnlineCollection.DeleteMany(_ => true);
                        ChatOnlineCollection.InsertOne(document);
                        return false;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> MakeOffline(string phone)
        {
            Init();
            try
            {
                var online = ChatOnlineCollection.Find(_ => true).FirstOrDefault().online;
                if (online.Contains(phone))
                {
                    online.Remove(phone);
                    ChatOnlineCollection.DeleteMany(_ => true);
                    ChatOnlineCollection.InsertOne(new ChatOnline { id = 0, online = online });
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static int GetOnline()
        {
            Init();
            return ChatOnlineCollection.Find(_ => true).FirstOrDefault().online.Count();
        }

        public static async Task<bool> SaveUser(string username, string phone, string password)
        {
            Init();
            var user = new User(username: username, phone: phone, password: password);
            try
            {
                await UserCollection.InsertOneAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> SaveUser(User user)
        {
            Init();
            try
            {
                await UserCollection.InsertOneAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static async Task<bool> RemoveUser(string phone)
        {
            Init();
            try
            {
                await UserCollection.DeleteOneAsync(b => b.phone == phone);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static List<ChatMessages> GetMessages()
        {
            Init();
            try
            {
                return ChatMessagesCollection.Find(_ => true).ToList();
            }
            catch
            {
                return null;
            }

        }

        public static async Task<bool> WriteMessage(string username, string content, string date)
        {
            Init();
            try
            {
                var lastMessage = GetMessages().FirstOrDefault();
                var message = new ChatMessages
                {
                    messageId = lastMessage == null ? 0 : lastMessage.messageId + 1,
                    username = username,
                    content = content,
                    date = date
                };
                await ChatMessagesCollection.InsertOneAsync(message);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}