using MongoDB.Bson;

namespace CityNavigator.Services.Base
{
    public interface ICollection
    {
        ObjectId Id { get; set; }
    }
}
