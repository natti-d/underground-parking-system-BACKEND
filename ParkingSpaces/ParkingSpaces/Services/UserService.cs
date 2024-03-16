using Microsoft.EntityFrameworkCore;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ParkingSpaces.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasherService passwordHasherService)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
        }

        public virtual async Task Login(UserLogin request)
        {
            Expression<Func<User, bool>> expression = user => 
                user.Username == request.Username;

            User user = _userRepository
                .FindByCriteria(expression)
                .FirstOrDefault();

            bool isTheSame = _passwordHasherService.Verify(user.Password, request.Password);

            if (!isTheSame)
            {
                throw new Exception("There is no user with these credentials!");
            }

            if (user == null)
            {
                throw new Exception("There is no user with these credentials!");
            }
        }

        public virtual async Task Register(UserRequest request)
        {
            //if (request.Plate == string.Empty)
            //{
            //    throw new Exception("Type your plate!");
            //}
            //if (request.Username.Length < 3 || request.Username.Length > 30)
            //{
            //    throw new Exception("Username length");
            //}

            //if (request.Password.Length > 30 || request.Password.Length < 8)
            //{
            //    throw new Exception("Password length");
            //}

            //if (request.FirstName.Length > 30 || request.FirstName.Length < 2)
            //{
            //    throw new Exception("Firstname length");
            //}

            //if (request.LastName.Length > 30 || request.LastName.Length < 2)
            //{
            //    throw new Exception("Lastname length");
            //}

            //bool emailvalidation = IsValidEmail(request.Email);
            //if (!emailvalidation)
            //{
            //    throw new Exception("Invalid email!");
            //}

            Expression<Func<User, bool>> usernameExpression = user =>
                user.Username == request.Username;

            bool usernamePresented = await _userRepository.FindAny(usernameExpression);

            if (usernamePresented)
            {
                throw new Exception("This username is already taken!");
            }

            Expression<Func<User, bool>> emailExpression = user =>
                user.Email == request.Email;

            bool emailPresented = await _userRepository.FindAny(emailExpression);

            if (emailPresented)
            {
                throw new Exception("This Email is already taken!");
            }

            // 69
            string passwordHash = _passwordHasherService.Hash(request.Password);

            User user = new User();
            user.Username = request.Username;
            user.Email = request.Email;
            user.Password = passwordHash;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Plate = request.Plate;

            await _userRepository.Create(user);
        }

        public virtual async Task Delete(int userId)
        {
            User user = await _userRepository.FindById(userId);

            if (user == null) 
            {
                throw new Exception("There is no user with this id!");
            }

            await _userRepository.Delete(user);
        }

        public virtual async Task Update(UserRequest request, int userId)
        {
            bool emailvalidation = IsValidEmail(request.Email);
            if (!emailvalidation)
            {
                throw new Exception("Invalid email!");
            }

            if (request.Username.Length < 3 && request.Username.Length > 30)
            {
                throw new Exception("Username length");
            }

            if (request.Password.Length > 30 || request.Password.Length < 8)
            {
                throw new Exception("Password length");
            }

            if (request.FirstName.Length > 30 || request.FirstName.Length < 2)
            {
                throw new Exception("Firstname length");
            }

            if (request.LastName.Length > 30 || request.LastName.Length < 2)
            {
                throw new Exception("Lastname length");
            }

            User userForUpdate = await _userRepository.FindById(userId);

            Expression<Func<User, bool>> usernameExpression = user =>
                user.Username == request.Username
                && user.Username != userForUpdate.Username;

            bool usernamePresented = await _userRepository.FindAny(usernameExpression);

            if (usernamePresented)
            {
                throw new Exception("This username is already taken!");
            }

            Expression<Func<User, bool>> emailExpression = user =>
                user.Email == request.Email
                && user.Email != userForUpdate.Email;

            bool emailPresented = await _userRepository.FindAny(emailExpression);

            if (emailPresented)
            {
                throw new Exception("This Email is already taken!");
            }

            userForUpdate.Username = request.Username;
            userForUpdate.Email = request.Email;
            userForUpdate.Password = request.Password;
            userForUpdate.FirstName = request.FirstName;
            userForUpdate.LastName = request.LastName;
            userForUpdate.Plate = request.Plate;

            await _userRepository.Update(userForUpdate);
        }

        public virtual async Task<UserGetInfo> GetInfo(int userId)
        {
            User user = await _userRepository.FindById(userId);

            return new UserGetInfo()
            {
                Username = user.Username,
                Plate = user.Plate,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        private bool IsValidEmail(string email)
        {
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            Match match = Regex.Match(email, pattern);

            return match.Success;
        }
    }
}
