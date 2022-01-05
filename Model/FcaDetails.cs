using ReactWindowsAuth.Interfaces;   
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ReactWindowsAuth.Models{
public class FcaDetails : IBaseInterface
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string FcaNumber {get;set;}
    public bool IsValid {get;set;} 
}
}