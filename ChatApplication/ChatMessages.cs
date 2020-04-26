using MongoDB.Bson.Serialization.Attributes;

namespace ChatApplication
{
    public class ChatMessages
    {
        [BsonId]
        public long messageId { get; set; }
        public string username { get; set; }
        public string content { get; set; }
        public string date { get; set; }
    }
}