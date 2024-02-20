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

        public int GetUserIdFromToken(string token)
        {
            byte[] plainTextBytes = Convert.FromBase64String(token);
            string[] credential = Convert.ToString(plainTextBytes).Split(':');

            Expression<Func<User, bool>> expression = 
                user => user.Username == credential[0] && user.Password == credential[1];

            User user = _userRepository.FindByCriteria(expression);

            if (user == null)
            {
                throw new Exception();
            }

            return user.Id;
        }
    }
}
