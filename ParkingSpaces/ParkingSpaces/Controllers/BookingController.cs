using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Services;

namespace ParkingSpaces.Controllers
{
    [Route("api/[controller]")]
    // routing?
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _accountService;

        public BookingController(IBookingService accountService)
        {
            _accountService = accountService;
        }
    }
}
