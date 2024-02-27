using Moq;
using ParkingSpaces.Controllers;
using ParkingSpaces.Repository.Repository_Interfaces;
using ParkingSpaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSpaces_Tests.User_Tests
{
    internal class UserControllerTests
    {
        private Mock<IUserService> mockUserService;
        private Mock<IUserRepository> mockUserRepository;

        private UserController userController;

        [SetUp]
        public void Setup()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockUserService = new Mock<IUserService>(mockUserRepository.Object);

            userController = new UserController(mockUserService.Object);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}
