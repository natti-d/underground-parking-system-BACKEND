using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using ParkingSpaces.Services;
using System.Text;
using System.Text.Encodings.Web;

namespace ParkingSpaces.Authentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthService _authService;

        public BasicAuthenticationHandler(AuthService authService, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(Request.Headers.ContainsKey("Authroization"))
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Missing Authorization key"
                ));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Authorization header does not start with 'Basic'"
                ));
            }

            string authBase64Decoded = Encoding.UTF8.GetString(
                    Convert.FromBase64String(
                        authorizationHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)
                    )); ;

            string[] authSplit = authBase64Decoded.Split(':');

            if (authSplit.Length != 2)
            {
                return Task.FromResult(
                    AuthenticateResult.Fail("Invalid Authorization header format"
                ));
            }

            string username = authSplit[0];
            string password = authSplit[1];

            int userId = _authService.GetUserIdFromToken(authBase64Decoded);

            // i think this will not be hit?
            if (userId == 0)
            {
                throw new Exception();
            }
        }
    }
}
