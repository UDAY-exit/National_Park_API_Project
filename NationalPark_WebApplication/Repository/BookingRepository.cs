
using System.Net.Http.Json;
using NationalPark_WebApplication.Models;
using NationalPark_WebApplication.Repository.IRepository;

namespace NationalPark_WebApplication.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingRepository(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Booking?> CreateBookingAsync(string url, Booking booking)
        {
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsJsonAsync(url, booking);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<Booking>();
        }
    }
}
