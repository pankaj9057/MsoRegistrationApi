using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsoRegistrationApi.Interfaces;
public interface IBaseInterface 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }
}