using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Services;
using System.Security.Claims;

namespace ParkingSpaces.Controllers
{
    [Route("api/booking")]
    [Authorize]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService ?? throw new NullReferenceException();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create(BookingRequest request)
        {
            int userId = int.Parse(User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value); // what to do?

            try
            {
                await _bookingService.Create(request, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete(BookingDelete request)
        {
            try
            {
                await _bookingService.Delete(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update(BookingUpdate request)
        {
            try
            {
                await _bookingService.Update(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<BookingResponse>>> GetActiveForUser()
        {
            int userId = int.Parse(User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value);

            try
            {
                // the await keyword make this method async
                return Ok(await _bookingService.GetActiveForUser(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/booking/{bookingId}")]
        public virtual async Task<ActionResult<BookingResponse>> GetById(int bookingId)
        {
            try
            {
                return Ok(await _bookingService.GetById(bookingId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
