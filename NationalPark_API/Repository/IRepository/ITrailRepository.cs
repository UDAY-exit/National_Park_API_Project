using NationalPark_API.Models;

namespace NationalPark_API.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        Trail GetTrail(int trailId);
        bool TrailExists(int trailId);
        bool TrailExists(string trailName);
        bool createTrail(Trail trail);
        bool updateTrail(Trail trail);
        bool deleteTrail(Trail trail);
        bool Save();
        ICollection<Trail> GetTrailsInNationalPark(int nationalParkId);
    }
}
