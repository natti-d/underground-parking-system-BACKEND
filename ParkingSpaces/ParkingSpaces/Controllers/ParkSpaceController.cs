using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Services;

namespace ParkingSpaces.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize] // require to be authorized
    [ApiController]
    public class ParkSpaceController : ControllerBase
    {
        private readonly IParkSpaceService _parkSpaceService;

        public ParkSpaceController(IParkSpaceService parkSpaceService)
        {
            _parkSpaceService = parkSpaceService;
        }

        // tasks:
        // get all
        // one multiple selects query
        // background service
        // real time space availability
        // background service to delete the bookings after 24 hours

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ParkSpaceGetAvaildableParkSpacesResponse>>> GetAvaildableParkSpaces()
        {
            try
            {
                return Ok(await _parkSpaceService.GetAvaildableParkSpaces());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
