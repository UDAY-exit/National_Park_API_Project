
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using NationalPark_WebApplication.Models;
using NationalPark_WebApplication.Repository.IRepository;

namespace NationalPark_WebApplication.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingController(
            IBookingRepository bookingRepository,
            INationalParkRepository nationalParkRepository,
            IHttpClientFactory httpClientFactory)
        {
            _bookingRepository = bookingRepository;
            _nationalParkRepository = nationalParkRepository;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int nationalParkId)
        {
            var park = await _nationalParkRepository.GetAsync(
                SD.NationalParkApiPath, nationalParkId);

            if (park == null)
                return NotFound("National Park not found.");

            var booking = new Booking
            {
                NationalParkId = nationalParkId,
                BookingDate = DateTime.Today.AddDays(1),
                Adults = 1,
                Children = 0
            };

            ViewBag.NationalPark = park;
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (booking.Adults + booking.Children is < 1 or > 5)
                ModelState.AddModelError("", "You can book for 1 to 5 people only.");

            if (!ModelState.IsValid)
            {
                ViewBag.NationalPark = await _nationalParkRepository.GetAsync(
                    SD.NationalParkApiPath, booking.NationalParkId);

                return View(booking);
            }

            var createdBooking = await _bookingRepository.CreateBookingAsync(
                SD.BookingAPIPath, booking);

            if (createdBooking == null || createdBooking.Id <= 0)
            {
                ModelState.AddModelError("", "Booking creation failed. Please try again.");

                ViewBag.NationalPark = await _nationalParkRepository.GetAsync(
                    SD.NationalParkApiPath, booking.NationalParkId);

                return View(booking);
            }

            var client = _httpClientFactory.CreateClient();

            var checkoutUrl = $"{SD.BookingAPIPath}/{createdBooking.Id}/checkout";
            var response = await client.PostAsync(checkoutUrl, null);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    $"Booking #{createdBooking.Id} was created, but payment could not be started.");

                ViewBag.NationalPark = await _nationalParkRepository.GetAsync(
                    SD.NationalParkApiPath, booking.NationalParkId);

                return View(booking);
            }

            var checkout = await response.Content.ReadFromJsonAsync<CheckoutResponse>();

            if (string.IsNullOrWhiteSpace(checkout?.CheckoutUrl))
                return BadRequest("Stripe did not return a checkout URL.");

            return Redirect(checkout.CheckoutUrl);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(
            [FromQuery(Name = "session_id")] string sessionId,
            [FromQuery] int bookingId)
        {
            if (string.IsNullOrWhiteSpace(sessionId) || bookingId <= 0)
                return BadRequest("Invalid payment details.");

            var client = _httpClientFactory.CreateClient();

            var verifyUrl =
                $"{SD.BookingAPIPath}/{bookingId}/verify-payment" +
                $"?sessionId={Uri.EscapeDataString(sessionId)}";

            var response = await client.PostAsync(verifyUrl, null);

            if (!response.IsSuccessStatusCode)
                return View("PaymentFailed");

            return View("PaymentSuccess");
        }

        [HttpGet]
        public IActionResult PaymentCancelled()
        {
            return View();
        }

        private sealed class CheckoutResponse
        {
            public string? CheckoutUrl { get; set; }
            public string? SessionId { get; set; }
        }
    }
}
