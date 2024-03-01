using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingSpaces.Controllers;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using ParkingSpaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSpaces_Tests.ParkSpace_Tests
{
    internal class ParkSpaceControllerTests
    {
        private Mock<ParkSpaceService> mockParkSpaceService;
        private Mock<IParkSpaceRepository> mockParkSpaceRepository;
        private ParkSpaceController parkSpaceController;

        private Mock<BookingService> mockBookingService;
        private Mock<IBookingRepository> mockBookingRepository;
        private Mock<IUserRepository> mockUserRepository;

        IEnumerable<ParkSpaceResponse> available;

        [SetUp]
        public void Setup()
        {
            mockBookingRepository = new Mock<IBookingRepository>();
            mockUserRepository = new Mock<IUserRepository>();
            mockBookingService = new Mock<BookingService>(mockBookingRepository.Object, mockUserRepository.Object);

            mockParkSpaceRepository = new Mock<IParkSpaceRepository>();
            mockParkSpaceService = new Mock<ParkSpaceService>(mockParkSpaceRepository.Object, mockBookingService.Object);

            parkSpaceController = new ParkSpaceController(mockParkSpaceService.Object);

            ParkSpaceResponse p1 = new ParkSpaceResponse();
            ParkSpaceResponse p2 = new ParkSpaceResponse();
            ParkSpaceResponse p3 = new ParkSpaceResponse();

            available = new List<ParkSpaceResponse>
            {
                p1,
                p2,
                p3
            };

            mockParkSpaceService.Setup(mock => mock.GetAvailable()).Returns(Task.FromResult(available)).Verifiable();
            mockParkSpaceService.Setup(mock => mock.GetAvailableByFilter(It.IsAny<ParkSpaceGetAvailableByFilter>()))
                .Returns(Task.FromResult(available))
                .Verifiable();
        }

        [Test]
        public async Task GetAvailable_ShouldReturnAvailableSpaces()
        {
            int expectedStatusCode = 200;
            int expectedCount = 3;

            ActionResult<IEnumerable<ParkSpaceResponse>> result = await parkSpaceController.GetAvailable();
            OkObjectResult okResult = result.Result as OkObjectResult;
            IEnumerable<ParkSpaceResponse> parkSpaceResponses = okResult.Value as IEnumerable<ParkSpaceResponse>;

            mockParkSpaceService.Verify(mock => mock.GetAvailable());

            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
            Assert.AreEqual(parkSpaceResponses.Count(), expectedCount);
        }

        [Test]
        public async Task GetAvailableByFilter_ShouldReturnAvailableByFilter()
        {
            int expectedStatusCode = 200;
            int expectedCount = 3;

            ActionResult<IEnumerable<ParkSpaceResponse>> result = await parkSpaceController.GetAvailableByFilter(It.IsAny<ParkSpaceGetAvailableByFilter>());
            OkObjectResult okResult = result.Result as OkObjectResult;
            IEnumerable<ParkSpaceResponse> parkSpaceResponses = okResult.Value as IEnumerable<ParkSpaceResponse>;

            // verify that the component is called
            mockParkSpaceService.Verify(mock => mock.GetAvailableByFilter(It.IsAny<ParkSpaceGetAvailableByFilter>()));
            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
            Assert.AreEqual(parkSpaceResponses.Count(), expectedCount);
        }
    }
}
