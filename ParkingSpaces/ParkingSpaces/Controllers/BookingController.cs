using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Services;

namespace ParkingSpaces.Controllers
{
    [Route("api/[controller]")]
    // routing?
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }




    }
}
