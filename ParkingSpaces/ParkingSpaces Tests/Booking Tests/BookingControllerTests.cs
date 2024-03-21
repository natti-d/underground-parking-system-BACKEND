//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using ParkingSpaces.Controllers;
//using ParkingSpaces.Models.DB;
//using ParkingSpaces.Models.NewFolder;
//using ParkingSpaces.Models.Request;
//using ParkingSpaces.Models.Response;
//using ParkingSpaces.Repository.Repository_Interfaces;
//using ParkingSpaces.Repository.Repository_Models;
//using ParkingSpaces.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Metadata;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace ParkingSpaces_Tests.Booking_Tests
//{
//    internal class BookingControllerTests
//    {
//        private BookingController bookingController;

//        private Mock<BookingService> mockBookingService;
//        private Mock<IBookingRepository> mockBookingRepository;
//        private Mock<IUserRepository> mockUserRepository;

//        IEnumerable<BookingResponse> active;

//        private int userId = 1;
//        private int bookingId = 10;

//        private string username = "test";
//        private string password = "test";

//        [SetUp]
//        public void Setup()
//        {
//            mockBookingRepository = new Mock<IBookingRepository>();
//            mockUserRepository = new Mock<IUserRepository>();
//            mockBookingService = new Mock<BookingService>(mockBookingRepository.Object, mockUserRepository.Object);

//            bookingController = new BookingController(mockBookingService.Object);

//            BookingResponse b1 = new BookingResponse();
//            BookingResponse b2 = new BookingResponse();
//            BookingResponse b3 = new BookingResponse();

//            active = new List<BookingResponse>()
//            {
//                b1,
//                b2,
//                b3
//            };

//            b3.BookingId = bookingId;

//            mockBookingService.Setup(mock => mock.Create(It.IsAny<BookingRequest>(), It.IsAny<int>())).Verifiable();
//            mockBookingService.Setup(mock => mock.Delete(It.IsAny<BookingDelete>())).Verifiable();
//            mockBookingService.Setup(mock => mock.Update(It.IsAny<BookingUpdate>())).Verifiable();
//            mockBookingService.Setup(mock => mock.GetActiveForUser(userId)).Returns(Task.FromResult(active)).Verifiable();
//            //mockBookingService.Setup(mock => mock.GetById(bookingId)).Returns(Task.FromResult(b3)).Verifiable();
//        }

//        [Test]
//        public async Task Create_ShouldReturnStatusOk()
//        {
//            int expectedStatusCode = 200;

//            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

//            // the actual request header (Basic Auth)
//            bookingController.ControllerContext = new ControllerContext();
//            bookingController.ControllerContext.HttpContext = new DefaultHttpContext();
//            bookingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic " + credentials;

//            // basic Auth handler task
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
//                // Add other claims as needed
//            };

//            ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
//            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

//            bookingController.ControllerContext.HttpContext.User = principal;

//            ActionResult result = await bookingController.Create(It.IsAny<BookingRequest>()) as ActionResult;
//            OkResult okResult = result as OkResult;

//            mockBookingService.Verify(mock => mock.Create(It.IsAny<BookingRequest>(), It.IsAny<int>()));

//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        }

//        [Test]
//        public async Task Delete_ShouldReturnStatusOk()
//        {
//            int expectedStatusCode = 200;

//            ActionResult result = await bookingController.Delete(It.IsAny<BookingDelete>()) as ActionResult;
//            OkResult okResult = result as OkResult;


//            // verify that the component is called
//            mockBookingService.Verify(mock => mock.Delete(It.IsAny<BookingDelete>()));
//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        }

//        [Test]
//        public async Task Update_ShouldReturnStatusOk()
//        {
//            int expectedStatusCode = 200;

//            ActionResult result = await bookingController.Update(It.IsAny<BookingUpdate>()) as ActionResult;
//            OkResult okResult = result as OkResult;


//            // verify that the component is called
//            mockBookingService.Verify(mock => mock.Update(It.IsAny<BookingUpdate>()));
//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        }

//        [Test]
//        public async Task GetActiveForUser_ShouldReturnTheActiveBookings()
//        {
//            int expectedStatusCode = 200;
//            int expectedCount = 3;

//            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

//            // the actual request header (Basic Auth)
//            bookingController.ControllerContext = new ControllerContext();
//            bookingController.ControllerContext.HttpContext = new DefaultHttpContext();
//            bookingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic " + credentials;

//            // basic Auth handler task
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
//                // Add other claims as needed
//            };

//            ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
//            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

//            bookingController.ControllerContext.HttpContext.User = principal;

//            ActionResult<IEnumerable<BookingResponse>> result = await bookingController.GetActiveForUser();

//            OkObjectResult okResult = result.Result as OkObjectResult;
//            IEnumerable<BookingResponse> bookingsResponse = okResult.Value as IEnumerable<BookingResponse>;

//            mockBookingService.Verify(mock => mock.GetActiveForUser(userId));

//            Assert.AreEqual(bookingsResponse.Count(), expectedCount);
//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        }

//        //[Test]
//        //public async Task GetById_ShouldReturnCorrectBooking()
//        //{
//        //    int expectedStatusCode = 200;
//        //    int expectedCount = 3;

//        //    string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

//        //    // the actual request header (Basic Auth)
//        //    bookingController.ControllerContext = new ControllerContext();
//        //    bookingController.ControllerContext.HttpContext = new DefaultHttpContext();
//        //    bookingController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic " + credentials;

//        //    // basic Auth handler task
//        //    var claims = new[]
//        //    {
//        //        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
//        //        // Add other claims as needed
//        //    };

//        //    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
//        //    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

//        //    bookingController.ControllerContext.HttpContext.User = principal;

//        //    ActionResult<BookingResponse> result = await bookingController.GetById(bookingId);


//        //    OkObjectResult okResult = result.Result as OkObjectResult;
//        //    BookingResponse bookingResponse = okResult.Value as BookingResponse;

//        //    mockBookingService.Verify(mock => mock.GetById(10));

//        //    Assert.AreEqual(bookingResponse.BookingId, bookingId);
//        //    Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        //}
//    }
//}
