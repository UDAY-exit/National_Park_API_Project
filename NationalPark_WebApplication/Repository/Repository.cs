using NationalPark_WebApplication.Repository.IRepository;
using Newtonsoft.Json;
using System.Text;

namespace NationalPark_WebApplication.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public Repository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreateAsync(string url, T objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if(objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate),
                    Encoding.UTF8,
                    "application/json"
                );
            }
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync( request );
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.Created)
                return true;return false;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + "/" + id.ToString());
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                return true;return false;
        }

        public Task<IEnumerable<T>> GetAllAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(string url, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            if (objToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate),
                    Encoding.UTF8,
                    "application/json"
                );
            }
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true; return false;
        }
    }
}
