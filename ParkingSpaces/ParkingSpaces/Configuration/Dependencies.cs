using Microsoft.Extensions.DependencyInjection;
using ParkingSpaces.BackgroundServices;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using ParkingSpaces.Services;

namespace ParkingSpaces.Configuration
{
    public class Dependencies
    {
        public void DefineDependencies(WebApplicationBuilder builder)
        {
            // services
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IParkSpaceService, ParkSpaceService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // repositories
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IParkSpaceRepository, ParkSpaceRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // background service
            builder.Services.AddHostedService<BookingCleanupService>();
            builder.Services.AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();
        }
    }
}
