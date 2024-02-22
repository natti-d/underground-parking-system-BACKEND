using ParkingSpaces.Models.DB;
using ParkingSpaces.Repository.Repository_Interfaces;
using System.Linq.Expressions;
using System.Text;

namespace ParkingSpaces.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string CreateToken(string username, string password)
        {
            string credentialString = username + ":" + password;
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(credentialString);

            string token = Convert.ToBase64String(plainTextBytes);
            return token;
        }

        // what is the best way to handle this?
        public int GetUserIdFromToken(string token)
        {
            string decodedAuthenticationToken = Encoding.UTF8.GetString(
                    Convert.FromBase64String(token));

            //Convert the string into an string array
            string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');

            Expression<Func<User, bool>> expression = 
                user => user.Username == usernamePasswordArray[0] && user.Password == usernamePasswordArray[1];

            User user = _userRepository.FindByCriteria(expression);

            if (user == null)
            {
                throw new Exception();
            }

            return user.Id;
        }
    }
}
