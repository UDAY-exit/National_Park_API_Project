
using Microsoft.AspNetCore.Mvc;
using NationalPark_API.Models;
using NationalPark_API.Models.DTOs;
using NationalPark_API.Repository.IRepository;
using Stripe;
using Stripe.Checkout;

namespace NationalPark_API.Controllers
{
    [Route("api/Booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _repo;
        private readonly IStripeClient _stripe;
        private readonly IConfiguration _config;

        public BookingController(
            IBookingRepository repo,
            IStripeClient stripe,
            IConfiguration config)
        {
            _repo = repo;
            _stripe = stripe;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var booking = await _repo.GetAsync(id);
            return booking == null ? NotFound() : Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Adults + dto.Children is < 1 or > 5)
                return BadRequest("Booking must contain 1 to 5 people.");

            if (await _repo.GetBookingsCountForDateAsync(dto.BookingDate) >= 100)
                return BadRequest("Booking limit reached for this date.");

            var booking = new Booking
            {
                NationalParkId = dto.NationalParkId,
                CustomerName = dto.CustomerName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                BookingDate = dto.BookingDate,
                Adults = dto.Adults,
                Children = dto.Children,
                TotalAmount = dto.Adults * 500m + dto.Children * 250m,
                Status = "Pending"
            };

            await _repo.CreateAsync(booking);

            return CreatedAtAction(nameof(Get), new { id = booking.Id }, booking);
        }

        [HttpPost("{id:int}/checkout")]
        public async Task<IActionResult> Checkout(int id)
        {
            var booking = await _repo.GetAsync(id);
            if (booking == null) return NotFound();
            if (booking.Status != "Pending")
                return BadRequest("Only pending bookings can be paid.");
            if (booking.TotalAmount <= 0)
                return BadRequest("Invalid booking amount.");

            var frontend = _config["Stripe:FrontendUrl"]?.TrimEnd('/');
            if (string.IsNullOrWhiteSpace(frontend))
                return StatusCode(500, "Stripe frontend URL is missing.");

            var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = $"{frontend}/Booking/PaymentSuccess?session_id={{CHECKOUT_SESSION_ID}}&bookingId={booking.Id}",
                CancelUrl = $"{frontend}/Booking/PaymentCancelled",
                CustomerEmail = booking.Email,
                ClientReferenceId = booking.Id.ToString(),
                Metadata = new Dictionary<string, string>
                {
                    ["BookingId"] = booking.Id.ToString()
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "inr",
                            UnitAmount = (long)(booking.TotalAmount * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "National Park Visit Booking",
                                Description = $"Booking #{booking.Id}"
                            }
                        }
                    }
                }
            };

            Console.WriteLine($"Success URL: {options.SuccessUrl}");
            Console.WriteLine($"Cancel URL: {options.CancelUrl}");

            var session = await new SessionService(_stripe).CreateAsync(options);

            return Ok(new { checkoutUrl = session.Url, sessionId = session.Id });
        }

        [HttpPost("{id:int}/verify-payment")]
        public async Task<IActionResult> VerifyPayment(
            int id, [FromQuery] string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return BadRequest("Stripe session ID is required.");

            var booking = await _repo.GetAsync(id);
            if (booking == null) return NotFound();
            if (booking.Status == "Cancelled")
                return BadRequest("Booking is cancelled.");
            if (booking.Status == "Paid")
                return Ok(new { bookingId = id, status = booking.Status });

            var session = await new SessionService(_stripe).GetAsync(sessionId);

            if (session.ClientReferenceId != id.ToString() ||
                session.Metadata == null ||
                !session.Metadata.TryGetValue("BookingId", out var bookingId) ||
                bookingId != id.ToString() ||
                session.PaymentStatus != "paid" ||
                session.AmountTotal != (long)(booking.TotalAmount * 100) ||
                !string.Equals(session.Currency, "inr", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Payment could not be verified.");
            }

            booking.Status = "Paid";

            if (!await _repo.UpdateAsync(booking))
                return StatusCode(500, "Could not update booking status.");

            return Ok(new
            {
                message = "Payment verified.",
                bookingId = id,
                status = booking.Status
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, BookingUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Adults + dto.Children is < 1 or > 5)
                return BadRequest("Booking must contain 1 to 5 people.");

            var booking = await _repo.GetAsync(id);
            if (booking == null) return NotFound();
            if (booking.Status != "Pending")
                return BadRequest("Only pending bookings can be updated.");

            booking.CustomerName = dto.CustomerName;
            booking.Email = dto.Email;
            booking.PhoneNumber = dto.PhoneNumber;
            booking.Address = dto.Address;
            booking.BookingDate = dto.BookingDate;
            booking.Adults = dto.Adults;
            booking.Children = dto.Children;
            booking.TotalAmount = dto.Adults * 500m + dto.Children * 250m;

            return await _repo.UpdateAsync(booking)
                ? Ok(booking)
                : StatusCode(500, "Could not update booking.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _repo.GetAsync(id);
            if (booking == null) return NotFound();
            if (booking.Status != "Pending")
                return BadRequest("Only pending bookings can be cancelled.");

            booking.Status = "Cancelled";

            return await _repo.UpdateAsync(booking)
                ? Ok("Booking cancelled.")
                : StatusCode(500, "Could not cancel booking.");
        }
    }
}
