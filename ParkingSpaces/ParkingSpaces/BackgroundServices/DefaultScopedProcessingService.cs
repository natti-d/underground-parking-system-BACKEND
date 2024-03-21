
using Microsoft.Extensions.Logging;
using ParkingSpaces.Services;

namespace ParkingSpaces.BackgroundServices
{
    public class DefaultScopedProcessingService : IScopedProcessingService
    {
        private readonly IBookingService _bookingService;
        public DefaultScopedProcessingService(
            IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {   
            while (!stoppingToken.IsCancellationRequested)
            {
                // Implement logic to delete bookings older than 1 days
                int days = 3;

                await _bookingService.DeleteOldBookings(days);

                // Sleep for a certain interval before the next iteration
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Run every day
            }
        }
    }
}
