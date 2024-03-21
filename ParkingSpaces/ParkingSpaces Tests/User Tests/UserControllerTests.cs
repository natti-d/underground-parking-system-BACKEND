//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using ParkingSpaces.Controllers;
//using ParkingSpaces.Models.DB;
//using ParkingSpaces.Models.Request;
//using ParkingSpaces.Models.Response;
//using ParkingSpaces.Repository.Repository_Interfaces;
//using ParkingSpaces.Repository.Repository_Models;
//using ParkingSpaces.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Net;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;

//namespace ParkingSpaces_Tests.User_Tests
//{
//    internal class UserControllerTests
//    {
//        private Mock<UserService> mockUserService;
//        private Mock<IUserRepository> mockUserRepository;

//        private UserController userController;

//        private UserGetInfo bob;

//        private int userId = 1;
//        private string username = "test";
//        private string password = "test";

//        [SetUp]
//        public void Setup()
//        {
//            mockUserRepository = new Mock<IUserRepository>();
//            mockUserService = new Mock<UserService>(mockUserRepository.Object);

//            userController = new UserController(mockUserService.Object);

//            bob = new UserGetInfo()
//            {
//                Username = "bob",
//                FirstName = "Boris",
//                LastName = "Matev",
//                Plate = "123456",
//                Email = "bobmatev123@ab.bg"
//            };

//            mockUserService.Setup(mock => mock.Register(It.IsAny<UserRequest>())).Verifiable();
//            mockUserService.Setup(mock => mock.Login(It.IsAny<UserLogin>())).Verifiable();
//            mockUserService.Setup(mock => mock.Delete(It.IsAny<int>())).Verifiable();
//            mockUserService.Setup(mock => mock.Update(It.IsAny<UserRequest>(), It.IsAny<int>())).Verifiable();
//            mockUserService.Setup(mock => mock.GetInfo(userId)).Returns(Task.FromResult(bob)).Verifiable();
//        }

//        [Test]
//        public async Task Register_ShouldReturnStatusOK()
//        {
//            var expectedStatusCode = 200; // Expected status code for OK

//            ActionResult result = await userController.Register(It.IsAny<UserRequest>()) as ActionResult;
//            OkResult okResult = result as OkResult;

//            mockUserService.Verify(mock => mock.Register(It.IsAny<UserRequest>()));
//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        }

//        [Test]
//        public async Task Login_ShouldReturnStatusOK()
//        {   
//            var expectedStatusCode = 200;

//            ActionResult result = await userController.Login(It.IsAny<UserLogin>()) as ActionResult;
//            OkResult okResult = result as OkResult;

//            // verify that the component is called
//            mockUserService.Verify(mock => mock.Login(It.IsAny<UserLogin>()));
//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        }


//        [Test]
//        public async Task Delete_ShouldReturnStatusOK()
//        {
//            var expectedStatusCode = 200;

//            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

//            // the actual request header (Basic Auth)
//            userController.ControllerContext = new ControllerContext();
//            userController.ControllerContext.HttpContext = new DefaultHttpContext();
//            userController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic " + credentials;

//            // basic Auth handler task
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
//                // Add other claims as needed
//            };

//            ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
//            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

//            userController.ControllerContext.HttpContext.User = principal;

//            ActionResult result = await userController.Delete() as ActionResult;

//            OkResult okResult = result as OkResult;

//            mockUserService.Verify(mock => mock.Delete(It.IsAny<int>()));
//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        }

//        [Test]
//        public async Task Update_ShouldReturnStatusOK()
//        {
//            var expectedStatusCode = 200;

//            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

//            // the actual request header (Basic Auth)
//            userController.ControllerContext = new ControllerContext();
//            userController.ControllerContext.HttpContext = new DefaultHttpContext();
//            userController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic " + credentials;

//            // basic Auth handler task
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
//                // Add other claims as needed
//            };

//            ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
//            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

//            userController.ControllerContext.HttpContext.User = principal;

//            ActionResult result = await userController.Update(It.IsAny<UserRequest>()) as ActionResult;
//            OkResult okResult = result as OkResult;

//            mockUserService.Verify(mock => mock.Update(It.IsAny<UserRequest>(), userId));
//            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
//        }

//        [Test]
//        public async Task GetInfo_ShouldReturnUserInfo()
//        {
//            var expectedStatusCode = 200;

//            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));

//            // the actual request header (Basic Auth)
//            userController.ControllerContext = new ControllerContext();
//            userController.ControllerContext.HttpContext = new DefaultHttpContext();
//            userController.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Basic " + credentials;

//            // basic Auth handler task
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
//            };

//            ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
//            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

//            userController.ControllerContext.HttpContext.User = principal;

//            ActionResult<UserGetInfo> result = await userController.GetInfo();
//            mockUserService.Verify(mock => mock.GetInfo(userId));

//            var statusCode = (result.Result as ObjectResult)?.StatusCode;

//            // to test this
//            var userGetInfo = (result.Result as ObjectResult)?.Value as UserGetInfo;

//            Assert.AreEqual(bob, userGetInfo);
//            Assert.AreEqual(statusCode, expectedStatusCode);
//        }
//    }
//}
