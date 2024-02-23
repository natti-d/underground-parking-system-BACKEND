using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Services;
using System.Security.Claims;

namespace ParkingSpaces.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize] // require to be authorized
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateBooking(BookingCreateBookingRequest request)
        {
            string username = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            try
            {
                await _bookingService.CreateBooking(request, username);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public virtual async Task<IActionResult> DeleteBooking(BookingDeleteBookingRequest request)
        {
            try
            {
                await _bookingService.DeleteBooking(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public virtual async Task<IActionResult> UpdateBooking(BookingUpdateBookingRequest request)
        {
            try
            {
                await _bookingService.UpdateBooking(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<BookingGetActiveBookingsResponse>>> GetActiveBookings()
        {
            string username = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;

            try
            {
                return Ok(await _bookingService.GetActiveBookings(username));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
