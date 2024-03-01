using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Services;

namespace ParkingSpaces.Controllers
{
    [Route("api/parkSpace[action]")]
    [Authorize] // require to be authorized
    [ApiController]
    public class ParkSpaceController : ControllerBase
    {
        private readonly IParkSpaceService _parkSpaceService;

        public ParkSpaceController(IParkSpaceService parkSpaceService)
        {
            _parkSpaceService = parkSpaceService ?? throw new NullReferenceException();
        }

        // one multiple selects query
        // real time space availability

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ParkSpaceResponse>>> GetAvailable()
        {
            try
            {
                return Ok(await _parkSpaceService.GetAvailable());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<ParkSpaceResponse>>> GetAvailableByFilter([FromQuery]ParkSpaceGetAvailableByFilter request)
        {
            try
            {
                return Ok(await _parkSpaceService.GetAvailableByFilter(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
