using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Services;
using System.ComponentModel;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ParkingSpaces.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new NullReferenceException();
        }

        [HttpPost]
        [Route("login")]
        public virtual async Task<ActionResult<string>> Login(UserLogin request)
        {
            try
            {
                string token = await _userService.Login(request);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public virtual async Task<IActionResult> Register(UserRequest request)
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
            try
            {
                await _userService.Delete(User);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]

        public virtual async Task<IActionResult> Update(UserRequest request)
        {
            try
            {
                await _userService.Update(request, User);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]   
        public virtual async Task<ActionResult<UserGetInfo>> GetInfo()
        {
            try
            {
                return Ok(await _userService.GetInfo(User));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("setUpPhoto")]
        public virtual async Task<ActionResult> SetUpAvatar(IFormFile file)
        {
            try
            {
                await _userService.SetUpAvatar(User, file);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
