//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using ParkingSpaces.Controllers;
//using ParkingSpaces.Models.Request;
//using ParkingSpaces.Models.Response;
//using ParkingSpaces.Repository.Repository_Interfaces;
//using ParkingSpaces.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
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

//        IEnumerable<ParkSpaceResponse> available;

//        [SetUp]
//        public void Setup()
//        {
//            mockBookingRepository = new Mock<IBookingRepository>();
//            mockUserRepository = new Mock<IUserRepository>();
//            mockBookingService = new Mock<BookingService>(mockBookingRepository.Object, mockUserRepository.Object);

//            bookingController = new BookingController(mockBookingService.Object);

//            //ParkSpaceResponse p1 = new ParkSpaceResponse();
//            //ParkSpaceResponse p2 = new ParkSpaceResponse();
//            //ParkSpaceResponse p3 = new ParkSpaceResponse();

//            //available = new List<ParkSpaceResponse>
//            //{
//            //    p1,
//            //    p2,
//            //    p3
//            //};

//            mockBookingService.Setup(mock => mock.Create(It.IsAny<BookingRequest>(), It.IsAny<int>())).Verifiable();
//            mockBookingService.Setup(mock => mock.Delete(It.IsAny<BookingDelete>())).Verifiable();
//            mockBookingService.Setup(mock => mock.Update(It.IsAny<BookingUpdate>())).Verifiable();
//            mockBookingService.Setup(mock => mock.GetActiveForUser(It.IsAny<int>())).Verifiable();
//            mockBookingService.Setup(mock => mock.GetById(It.IsAny<int>())).Verifiable();
//        }

//        [Test]
//        public async Task GetAvailable_ShouldReturnAvailableSpaces()
//        {
//            int expectedStatusCode = 200;
//            int expectedCount = 3;

//            ActionResult<IEnumerable<ParkSpaceResponse>> result = await parkSpaceController.GetAvailable();
//            OkObjectResult okResult = result.Result as OkObjectResult;
//            IEnumerable<ParkSpaceResponse> parkSpaceResponses = okResult.Value as IEnumerable<ParkSpaceResponse>;

//            mockParkSpaceService.Verify(mock => mock.GetAvailable());

//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//            Assert.AreEqual(parkSpaceResponses.Count(), expectedCount);
//        }

//        [Test]
//        public async Task GetAvailableByFilter_ShouldReturnAvailableByFilter()
//        {
//            int expectedStatusCode = 200;
//            int expectedCount = 3;

//            ActionResult<IEnumerable<ParkSpaceResponse>> result = await parkSpaceController.GetAvailableByFilter(It.IsAny<ParkSpaceGetAvailableByFilter>());
//            OkObjectResult okResult = result.Result as OkObjectResult;
//            IEnumerable<ParkSpaceResponse> parkSpaceResponses = okResult.Value as IEnumerable<ParkSpaceResponse>;

//            // verify that the component is called
//            mockParkSpaceService.Verify(mock => mock.GetAvailableByFilter(It.IsAny<ParkSpaceGetAvailableByFilter>()));
//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//            Assert.AreEqual(parkSpaceResponses.Count(), expectedCount);
//        }
//    }
//}
