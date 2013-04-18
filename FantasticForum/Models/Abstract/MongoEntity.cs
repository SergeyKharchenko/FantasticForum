using MongoDB.Bson;

namespace Models.Abstract
{
    public abstract class MongoEntity
    {
        public ObjectId Id { get; set; }
    }
}