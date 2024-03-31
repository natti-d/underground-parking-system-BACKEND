using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingSpaces.Controllers;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Models.Response;
using ParkingSpaces.Services;
using System.Security.Claims;

namespace ParkingSpaces_Tests.User_Tests
{
    internal class UserControllerTests
    {
        private const int EXPECTED_STATUS_CODE =  200;
        private Mock<IUserService> mockUserService;
        private UserController userController;

        private UserGetInfo bob;

        [SetUp]
        public void Setup()
        {
            mockUserService = new Mock<IUserService>();
            userController = new UserController(mockUserService.Object);

            bob = new UserGetInfo()
            {
                Username = "bob",
                FirstName = "Boris",
                LastName = "Matev",
                Plate = "123456",
                Email = "bobmatev123@ab.bg"
            };

            mockUserService.Setup(mock => mock.Register(It.IsAny<UserRequest>())).Verifiable();
            mockUserService.Setup(mock => mock.Login(It.IsAny<UserLogin>())).Verifiable();
            mockUserService.Setup(mock => mock.Delete(It.IsAny<ClaimsPrincipal>())).Verifiable();
            mockUserService.Setup(mock => mock.Update(It.IsAny<UserRequest>(), It.IsAny<ClaimsPrincipal>())).Verifiable();
            mockUserService.Setup(mock => mock.GetInfo(It.IsAny<ClaimsPrincipal>())).Returns(Task.FromResult(bob)).Verifiable();
            mockUserService.Setup(mock => mock.SetUpAvatar(It.IsAny<ClaimsPrincipal>(), It.IsAny<IFormFile>())).Verifiable();
        }

        [Test]
        public async Task Register_ShouldReturnStatusOK()
        {
            ActionResult result = await userController.Register(It.IsAny<UserRequest>()) as ActionResult;
            OkResult okResult = result as OkResult;

            mockUserService.Verify(mock => mock.Register(It.IsAny<UserRequest>()));
            Assert.AreEqual(okResult.StatusCode, EXPECTED_STATUS_CODE);
        }

        [Test]
        public async Task Login_ShouldReturnStatusOK()
        {
            ActionResult<string> result = await userController.Login(It.IsAny<UserLogin>());
            OkObjectResult okResult = result.Result as OkObjectResult;

            // verify that the component is called
            mockUserService.Verify(mock => mock.Login(It.IsAny<UserLogin>()));
            Assert.AreEqual(okResult.StatusCode, EXPECTED_STATUS_CODE);
        }


        [Test]
        public async Task Delete_ShouldReturnStatusOK()
        {
            ActionResult result = await userController.Delete() as ActionResult;
            OkResult okResult = result as OkResult;

            mockUserService.Verify(mock => mock.Delete(It.IsAny<ClaimsPrincipal>()));
            Assert.AreEqual(okResult.StatusCode, EXPECTED_STATUS_CODE);
        }

        [Test]
        public async Task Update_ShouldReturnStatusOK()
        {
            ActionResult result = await userController.Update(It.IsAny<UserRequest>()) as ActionResult;
            OkResult okResult = result as OkResult;

            mockUserService.Verify(mock => mock.Update(It.IsAny<UserRequest>(), It.IsAny<ClaimsPrincipal>()));
            Assert.AreEqual(okResult.StatusCode, EXPECTED_STATUS_CODE);
        }

        [Test]
        public async Task GetInfo_ShouldReturnUserInfo()
        {
            ActionResult<UserGetInfo> result = await userController.GetInfo();
            mockUserService.Verify(mock => mock.GetInfo(It.IsAny<ClaimsPrincipal>()));

            var statusCode = (result.Result as ObjectResult).StatusCode;
            var userGetInfo = (result.Result as ObjectResult).Value as UserGetInfo;

            Assert.AreEqual(bob, userGetInfo);
            Assert.AreEqual(statusCode, EXPECTED_STATUS_CODE);
        }

        [Test]
        public async Task SetUpAvatar_ShouldReturnOk()
        {
            ActionResult result = await userController.SetUpAvatar(It.IsAny<IFormFile>());
            mockUserService.Verify(mock => mock.SetUpAvatar(It.IsAny<ClaimsPrincipal>(), It.IsAny<IFormFile>()));

            var statusCode = (result as OkResult).StatusCode;
            Assert.AreEqual(statusCode, EXPECTED_STATUS_CODE);
        }
    }
}
