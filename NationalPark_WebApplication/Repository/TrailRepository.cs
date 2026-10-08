using NationalPark_WebApplication.Models;
using NationalPark_WebApplication.Repository.IRepository;

namespace NationalPark_WebApplication.Repository
{
    public class TrailRepository:Repository<Trail>,ITrailRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TrailRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
