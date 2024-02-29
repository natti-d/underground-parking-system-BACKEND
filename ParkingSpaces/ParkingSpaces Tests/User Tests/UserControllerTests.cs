using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingSpaces.Controllers;
using ParkingSpaces.Models.DB;
using ParkingSpaces.Models.Request;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Repository.Repository_Models;
using ParkingSpaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSpaces_Tests.User_Tests
{
    internal class UserControllerTests
    {
        private Mock<UserService> mockUserService;
        private Mock<IUserRepository> mockUserRepository;

        private UserController userController;

        private User bob, alice;
        private ICollection<User> users;

        [SetUp]
        public void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockUserService = new Mock<UserService>(mockUserRepository.Object);

            userController = new UserController(mockUserService.Object);

            bob = new User()
            {
                Id = 1,
                Username = "bob",
                Password = "12345678",
                FirstName = "Boris",
                LastName = "Matev",
                Plate = "123456",
                Email = "bobmatev123@ab.bg"
            };

            alice = new User()
            {
                Id = 1,
                Username = "alice",
                Password = "12345678",
                FirstName = "Alice",
                LastName = "Mateva",
                Plate = "123456",
                Email = "alicemateva123@ab.bg"
            };

            users = new List<User>
            {
                bob,
                alice
            };

            mockUserService.Setup(mock => mock.Register(It.IsAny<UserRequest>()))
                .Verifiable();

            mockUserService.Setup(mock => mock.Delete(It.IsAny<int>()))
                .Verifiable();

            //mockUserService.Setup(mock => mock.UpdateAccount(It.IsAny<AccountRequest>(), It.IsAny<Guid>()))
            //    .Verifiable();

            //mockUserService.Setup(mock => mock.GetAllAccounts())
            //    .Returns(Task.FromResult(accounts.AsEnumerable()))
            //    .Verifiable();

            //mockUserService.Setup(mock => mock.GetAccountByCriteria(It.IsAny<Expression<Func<Account, bool>>>()))
            //    .Returns(Task.FromResult(accounts.AsQueryable().Where(expression).AsEnumerable()))
            //    .Verifiable();

        }

        [Test]
        public async Task Register_ShouldReturnStatusOK()
        {
            var expectedStatusCode = 200; // Expected status code for OK

            ActionResult result = await userController.Register(It.IsAny<UserRequest>()) as ActionResult;

            var okResult = result as OkResult;

            mockUserService.Verify(mock => mock.Register(It.IsAny<UserRequest>()));
            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
        }


        [Test]
        public async Task Delete_ShouldReturnStatusOK()
        {
            var expectedStatusCode = 200;

            ActionResult result = await userController.Delete() as ActionResult;

            var okResult = result as OkResult;

            mockUserService.Verify(mock => mock.Delete(It.IsAny<int>()));
            Assert.AreEqual(okResult.StatusCode, expectedStatusCode);
        }
    }
}
