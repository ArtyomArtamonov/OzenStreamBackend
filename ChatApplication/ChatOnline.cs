using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ChatApplication
{
    public class ChatOnline
    {
        [BsonId]
        public int id { get; set; }
        public List<string> online { get; set; }
    }
}