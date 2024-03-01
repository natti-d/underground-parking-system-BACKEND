using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Services;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ParkingSpaces.Authentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserRepository _userRepository;

        public BasicAuthenticationHandler(
            IUserRepository userRepository,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            string token = authorizationHeader.Substring(6);
            string credentialAsStirng = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            string[] credentials = credentialAsStirng.Split(':');

            // ? to try it, to test it
            if (credentials?.Length != 2)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            string username = credentials[0];
            string password = credentials[1];

            // like a pointer, saved in a variable (like normal variable)
            // a storage of a method
            // Func<User, string> func = user => user.LastName;
            // Action - the same, but return void

            // data structure representing code
            // anylize the code direct in the sql language

            Expression<Func<User, bool>> expression = user =>
                user.Username == username
                && user.Password == password;

            User user = _userRepository
                .FindByCriteria(expression)
                .FirstOrDefault();

            if(user == null)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            // claim: the attributes for this user (type, value)
            // 
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            var identity = new ClaimsIdentity(claims, "Basic");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }
    }
}
