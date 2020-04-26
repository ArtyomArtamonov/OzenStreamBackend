using MongoDB.Bson.Serialization.Attributes;

namespace ChatApplication
{
    public class User
    {
        [BsonId]
        public string phone { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public User(string username, string phone, string password)
        {
            this.username = username;
            this.phone = phone;
            this.password = password;
        }
    }
}