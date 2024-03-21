using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ParkingSpaces.Auth.Jwt;
using ParkingSpaces.BackgroundServices;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using ParkingSpaces.Services;

namespace ParkingSpaces.Configuration
{
    public static class Dependencies
    {
        public static void DefineDependencies(WebApplicationBuilder builder)
        {
            // services
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IParkSpaceService, ParkSpaceService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            // repositories
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IParkSpaceRepository, ParkSpaceRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // background service
            builder.Services.AddHostedService<BookingCleanupService>();
            builder.Services.AddScoped<IScopedProcessingService, DefaultScopedProcessingService>();

            // fixing circular dependencies in .NET Core
            builder.Services.AddLazyResolution();
        }

        public static IServiceCollection AddLazyResolution(this IServiceCollection services)
        {
            return services.AddTransient(
                typeof(Lazy<>),
                typeof(LazilyResolved<>));
        }
    }
}
