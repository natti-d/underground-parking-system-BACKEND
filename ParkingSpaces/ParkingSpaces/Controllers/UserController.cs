using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using System.Text;
using System.Text.RegularExpressions;

namespace ParkingSpaces.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ParkingSpacesDbContext _dbContext;

        public UserController(ParkingSpacesDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext ?? throw new NullReferenceException();
        }
        [HttpPost]
        public ActionResult<string> Login(UserLoginRequest request)
        {
            var user = _dbContext.Users
                .FirstOrDefault(x => 
                    x.Username == request.Username && 
                    x.Password == request.Password
                    );

            if (user == null)
            {
                return NotFound();
            }

            string credentialString = user.Username.ToString() + user.Password.ToString();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(credentialString);

            string token = Convert.ToBase64String(plainTextBytes);
            return Ok(token);
        }

        public ActionResult<string> Register(UserRegisterRequest request)
        { 
            bool emailvalidation = IsValidEmail(request.Email);
            if (emailvalidation)
            {
                return BadRequest("Invalid email!");
            }
            if (_dbContext.Users.Any(x => x.Username == request.Username))
            {
                return BadRequest("This username is already taken!");
            }
            if (_dbContext.Users.Any(x => x.Email == request.Email))
            {
                return BadRequest("This Email is already taken!");
            }
            if (request.Username.Length < 3 && request.Username.Length > 30)
            {
                return BadRequest();
            }
            if (request.Password.Length > 30 || request.Password.Length < 8)
            {
                return BadRequest();
            }
            if (request.FirstName.Length > 30 || request.FirstName.Length < 2)
            {
                return BadRequest();
            }
            if (request.LastName.Length > 30 || request.LastName.Length < 2)
            {
                return BadRequest();
            }

            


            User user = new User();
            user.Username = request.Username;
            user.Email = request.Email;
            user.Password = request.Password;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;


            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            string credentialString = user.Username.ToString() + user.Password.ToString();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(credentialString);

            return Ok(Convert.ToBase64String(plainTextBytes));
        }
        private bool IsValidEmail(string email)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
    }
}
