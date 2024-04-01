using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingSpaces.Controllers;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.NewFolder;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using ParkingSpaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSpaces_Tests.Booking_Tests
{
    internal class BookingControllerTests
    {
        private BookingController bookingController;
        private Mock<IBookingService> mockBookingService;

        IEnumerable<BookingResponse> active;

        private int userId = 1;
        private int bookingId = 10;

        private string username = "test";
        private string password = "test";

        [SetUp]
        public void Setup()
        {
            mockBookingService = new Mock<IBookingService>();

            bookingController = new BookingController(mockBookingService.Object);

            BookingResponse b1 = new BookingResponse();
            BookingResponse b2 = new BookingResponse();
            BookingResponse b3 = new BookingResponse();

            active = new List<BookingResponse>()
            {
                b1,
                b2,
                b3
            };

            b1.BookingId = bookingId;

            mockBookingService.Setup(mock => mock.Create(It.IsAny<BookingRequest>(), It.IsAny<ClaimsPrincipal>())).Verifiable();
            mockBookingService.Setup(mock => mock.Delete(It.IsAny<BookingDelete>())).Verifiable();
            mockBookingService.Setup(mock => mock.Update(It.IsAny<BookingUpdate>())).Verifiable();
            mockBookingService.Setup(mock => mock.GetActiveForUser(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(active)).Verifiable();
            mockBookingService.Setup(mock => mock.GetAll(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(active)).Verifiable();
            mockBookingService.Setup(mock => mock.GetById(It.IsAny<int>())).Returns(Task.FromResult(active.First())).Verifiable();
        }

        [Test]
        public async Task Create_ShouldReturnStatusOk()
        {
            int expectedStatusCode = 200;

            ActionResult result = await bookingController.Create(It.IsAny<BookingRequest>()) as ActionResult;
            OkResult okResult = result as OkResult;

            mockBookingService.Verify(mock => mock.Create(It.IsAny<BookingRequest>(), It.IsAny<ClaimsPrincipal>()));

            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
        }

        [Test]
        public async Task Delete_ShouldReturnStatusOk()
        {
            int expectedStatusCode = 200;

            ActionResult result = await bookingController.Delete(It.IsAny<BookingDelete>()) as ActionResult;
            OkResult okResult = result as OkResult;


            // verify that the component is called
            mockBookingService.Verify(mock => mock.Delete(It.IsAny<BookingDelete>()));
            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
        }

        [Test]
        public async Task Update_ShouldReturnStatusOk()
        {
            int expectedStatusCode = 200;

            ActionResult result = await bookingController.Update(It.IsAny<BookingUpdate>()) as ActionResult;
            OkResult okResult = result as OkResult;


            // verify that the component is called
            mockBookingService.Verify(mock => mock.Update(It.IsAny<BookingUpdate>()));
            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
        }

        [Test]
        public async Task GetActiveForUser_ShouldReturnTheActiveBookings()
        {
            int expectedStatusCode = 200;
            int expectedCount = 3;

            ActionResult<IEnumerable<BookingResponse>> result = await bookingController.GetActiveForUser();

            OkObjectResult okResult = result.Result as OkObjectResult;
            IEnumerable<BookingResponse> bookingsResponse = okResult.Value as IEnumerable<BookingResponse>;

            mockBookingService.Verify(mock => mock.GetActiveForUser(It.IsAny<ClaimsPrincipal>()));

            Assert.AreEqual(bookingsResponse.Count(), expectedCount);
            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
        }

        [Test]
        public async Task GetAll_ShouldReturnOk()
        {
            int expectedStatusCode = 200;
            int expectedCount = 3;

            ActionResult<IEnumerable<BookingResponse>> result = await bookingController.GetAll(It.IsAny<int>());

            OkObjectResult okResult = result.Result as OkObjectResult;
            IEnumerable<BookingResponse> bookingResponse = okResult.Value as IEnumerable<BookingResponse>;

            mockBookingService.Verify(mock => mock.GetAll(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), It.IsAny<int>()));

            Assert.AreEqual(bookingResponse.Count(), expectedCount);
            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
        }


        [Test]
        public async Task GetById_ShouldReturnCorrectBooking()
        {
            int expectedStatusCode = 200;

            ActionResult<BookingResponse> result = await bookingController.GetById(It.IsAny<int>());


            OkObjectResult okResult = result.Result as OkObjectResult;
            BookingResponse bookingResponse = okResult.Value as BookingResponse;

            mockBookingService.Verify(mock => mock.GetById(It.IsAny<int>()));

            Assert.AreEqual(bookingResponse.BookingId, bookingId);
            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
        }
    }
}
