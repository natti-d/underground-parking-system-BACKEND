using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ParkingSpaces.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new NullReferenceException();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Login(UserLoginRequest request)
        {
            try
            {
                await _userService.Login(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Register(UserRegisterRequest request)
        {
            try
            {
                await _userService.Register(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public virtual async Task<IActionResult> Delete()
        {
            int userId = int.Parse(User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value);

            try
            {
                await _userService.Delete(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public virtual async Task<IActionResult> Update(UserUpdateRequest request)
        {
            int userId = int.Parse(User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value);

            try
            {
                await _userService.Update(request, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public virtual async Task<ActionResult<UserGetInfoResponse>> GetInfo()
        {
            int userId = int.Parse(User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value);

            try
            {
                return Ok(await _userService.GetInfo(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
