namespace ParkingSpaces.BackgroundServices
{
    public interface IScopedProcessingService
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}
