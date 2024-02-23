using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ParkingSpaces.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize] // require to be authorized
    [ApiController]
    public class ParkingSpaceController : ControllerBase
    {
        // get all

        // get all available
    }
}
