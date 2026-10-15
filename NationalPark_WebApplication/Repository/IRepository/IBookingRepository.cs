
using NationalPark_WebApplication.Models;

namespace NationalPark_WebApplication.Repository.IRepository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<Booking?> CreateBookingAsync(string url, Booking booking);
    }
}
