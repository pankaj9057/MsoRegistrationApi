using System; 
using ReactWindowsAuth.Interfaces;   
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ReactWindowsAuth.Models;
public class RegistrationDetails : IBaseInterface
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public Title Title {get;set;}
    public string Firstname {get;set;}
    public string Surname {get;set;}
    public DateTime DateOfBirth {get;set;}
    public Role Role {get;set;}
    public Brand Brand {get;set;}
    public string FCANumber{get;set;}
}

public class Title:SelectData
{
   
}
public class Role:SelectData
{
   
}
public class Brand:SelectData
{
   
}
public class SelectData: IBaseInterface
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }    
    public string value{get;set;}
    public string caption{get;set;}
}


