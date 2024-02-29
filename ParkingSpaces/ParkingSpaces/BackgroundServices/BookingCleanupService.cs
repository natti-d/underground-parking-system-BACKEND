using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Services;

namespace ParkingSpaces.BackgroundServices
{
    public class BookingCleanupService : BackgroundService
    {
        private readonly ILogger<BookingCleanupService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BookingCleanupService(
            ILogger<BookingCleanupService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
            "{Name} is running.", nameof(BookingCleanupService));

            await DoWorkAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "{Name} is stopping.", nameof(BookingCleanupService));

            await base.StopAsync(cancellationToken);
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "{Name} is working.", nameof(BookingCleanupService));

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                IScopedProcessingService scopedProcessingService =
                    scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.DoWorkAsync(stoppingToken);
            }
        }
    }
}
