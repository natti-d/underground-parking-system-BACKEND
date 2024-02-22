using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using ParkingSpaces.Services;

namespace ParkingSpaces.Configuration
{
    public class Dependencies
    {
        public void DefineDependencies(WebApplicationBuilder builder)
        {
            // register services!

            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
