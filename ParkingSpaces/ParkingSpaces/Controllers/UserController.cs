using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ParkingSpaces.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ParkingSpacesDbContext _dbContext;

        public UserController(
            ParkingSpacesDbContext dbContext,
            IAuthService authService,
            IConfiguration configuration)
        {
            _authService = authService;
            _dbContext = dbContext ?? throw new NullReferenceException();
        }

        [HttpPost]
        public ActionResult<string> Login(UserLoginRequest request)
        {
            var user = _dbContext.Users
                .FirstOrDefault(x =>
                    x.Username == request.Username &&
                    x.Password == request.Password);

            if (user == null)
            {
                return NotFound();
            }

            string token = _authService.CreateToken(user.Username, user.Password);
            return Ok(token);
        }

        [HttpGet]
        public virtual async Task<int> GetNumber()
        {
            return 10;
        }

        [HttpPost]
        public ActionResult<string> Register(UserRegisterRequest request)
        {
            //bool emailvalidation = IsValidEmail(request.Email);
            //if (emailvalidation)
            //{
            //    return BadRequest("Invalid email!");
            //}
            //if (_dbContext.Users.Any(x => x.Username == request.Username))
            //{
            //    return BadRequest("This username is already taken!");
            //}
            //if (_dbContext.Users.Any(x => x.Email == request.Email))
            //{
            //    return BadRequest("This Email is already taken!");
            //}
            //if (request.Username.Length < 3 && request.Username.Length > 30)
            //{
            //    return BadRequest();
            //}
            //if (request.Password.Length > 30 || request.Password.Length < 8)
            //{
            //    return BadRequest();
            //}
            //if (request.FirstName.Length > 30 || request.FirstName.Length < 2)
            //{
            //    return BadRequest();
            //}
            //if (request.LastName.Length > 30 || request.LastName.Length < 2)
            //{
            //    return BadRequest();
            //}

            User user = new User();
            user.Username = request.Username;
            user.Email = request.Email;
            user.Password = request.Password;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Plate = request.Plate;

            string token = _authService.CreateToken(user.Username, user.Password);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        public ActionResult<int> GetMaginNumber()
        {
            return Ok(10);
        }

        private bool IsValidEmail(string email)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }


    
    }
}
