using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Response;
using System.Security.Claims;

namespace ParkingSpaces.Services
{
    public interface IBookingService
    {
        public Task Create(BookingRequest request, ClaimsPrincipal user);

        public Task Delete(BookingDelete request);

        public Task Update(BookingUpdate booking);

        public Task<BookingResponse> GetById(int bookingId);

        public Task<IEnumerable<BookingResponse>> GetActiveForUser(ClaimsPrincipal user);

        public Task<IEnumerable<BookingResponse>> GetAll(ClaimsPrincipal user, int page, int count);
        public Task<IQueryable<BookingResponse>> GetActiveForNow();

        public Task<IQueryable<BookingResponse>> GetAvailableByFilter(ParkSpaceGetAvailableByFilter request);

        public Task DeleteOldBookings(int days);
    }
}
