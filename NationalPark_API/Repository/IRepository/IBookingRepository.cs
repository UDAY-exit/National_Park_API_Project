
using NationalPark_API.Models;

namespace NationalPark_API.Repository.IRepository
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllAsync();

        Task<Booking> CreateAsync(Booking booking);

        Task<Booking?> GetAsync(int id);

        Task<bool> UpdateAsync(Booking booking);
        Task<int> GetBookingsCountForDateAsync(DateTime bookingDate);

        Task<bool> SaveAsync();
    }
}
