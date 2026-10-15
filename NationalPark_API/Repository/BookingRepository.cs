
using Microsoft.EntityFrameworkCore;
using NationalPark_API.Data;
using NationalPark_API.Models;
using NationalPark_API.Repository.IRepository;

namespace NationalPark_API.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.NationalPark)
                .ToListAsync();
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await SaveAsync();

            return booking;
        }

        public async Task<Booking?> GetAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.NationalPark)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            return await SaveAsync();
        }
        public async Task<int> GetBookingsCountForDateAsync(DateTime bookingDate)
        {
            return await _context.Bookings
                .CountAsync(b =>
                    b.BookingDate.Date == bookingDate.Date &&
                    b.Status != "Cancelled");
        }
    }
}
