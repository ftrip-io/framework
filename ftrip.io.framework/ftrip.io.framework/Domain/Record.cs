using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ftrip.io.framework.Domain
{
    public class Record : IIdentifiable<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}