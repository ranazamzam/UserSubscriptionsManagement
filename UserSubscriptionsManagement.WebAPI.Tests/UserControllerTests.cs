using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Contracts.ServiceContracts;
using UserSubscriptionsManagement.Domain.Models;
using UserSubscriptionsManagement.WebAPI.Controllers;

namespace UserSubscriptionsManagement.WebAPI.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _userController;
        private Mock<IUserService> _mockUserService;
        private List<UserData> _users;


        #region  setup before each test
        [SetUp]
        public void ReInitializeTest()
        {
            _users = SetUpUsers();
            _mockUserService = new Mock<IUserService>();
        }
        #endregion

        [Test]
        public void GetAllUsers_ThereIsUsers_ShouldReturnOkAndListOfUsers()
        {
            // Arrange
            _mockUserService.Setup(p => p.GetAllUsers()).Returns(_users);
            _userController = new UserController(_mockUserService.Object);

            // Act
            var response = _userController.GetAllUsers();

            // Assert
            var okResult = response as OkNegotiatedContentResult<List<UserData>>;
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(okResult.Content);

            var returnedUsers = okResult.Content as List<UserData>;
            Assert.IsNotNull(returnedUsers);
            Assert.IsInstanceOf(typeof(List<UserData>), returnedUsers);
            Assert.AreEqual(returnedUsers.Any(), true);

            Assert.AreEqual(2, returnedUsers.Count);
        }

        [Test]
        public void GetAllUsers_IfThereIsNoUsers_ShouldReturnNotFoundAndEmptyList()
        {
            // Arrange
            _users.Clear();
            _mockUserService.Setup(p => p.GetAllUsers()).Returns(_users);
            _userController = new UserController(_mockUserService.Object);

            // Act
            var response = _userController.GetAllUsers();

            var notFoundResult = response as NotFoundResult;

            // Assert
            Assert.IsNotNull(notFoundResult);
        }

        [Test]
        public void GetAllUsers_IfThrowException_ShouldReturnExceptionMessage()
        {
            // Arrange
            _mockUserService.Setup(p => p.GetAllUsers()).Throws<Exception>();
            _userController = new UserController(_mockUserService.Object);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _userController.GetAllUsers());
            Assert.That(ex.Message, Is.EqualTo("Exception of type 'System.Exception' was thrown."));
        }

        [Test]
        public void GetUserById_InvalidId_ShouldReturnBadRequest()
        {
            // Arrange
            _userController = new UserController(_mockUserService.Object);

            // Act
            var response = _userController.GetUserById(-1);

            var badRequestResult = response as BadRequestResult;

            // Assert
            Assert.IsNotNull(badRequestResult);
        }

        [Test]
        public void AddUser_ShouldReturnOk()
        {
            // Arrange
            _mockUserService.Setup(p => p.AddUser(It.IsAny<UserData>())).Callback(new Action<UserData>(newUser =>
            {
                var lastUserId = _users.Last().Id;
                var nextUserId = lastUserId + 1;
                newUser.Id = nextUserId;
                _users.Add(newUser);
            }));
            _userController = new UserController(_mockUserService.Object);
            var usersCountBeforeAdd = _users.Count;

            // Act
            var response = _userController.AddUser(new UserData
            {
                FirstName = "test first 3",
                LastName = "test last 3",
                Email = "e@gmail.com"
            });

            // Assert
            var okResult = response as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(usersCountBeforeAdd + 1, _users.Count);
        }

        #region Helpers and Data Initializer
        private List<UserData> SetUpUsers()
        {
            return new List<UserData>()
            {
                new UserData
                {
                    Id=1,
                    FirstName = "First Name1",
                    LastName = "Last Name1",
                    Email = "email@email.com",
                },
                new UserData
                {
                    Id=2,
                    FirstName = "First Name2",
                    LastName = "Last Name2",
                    Email = "email2@email.com"
                },
            };
        }

        public class UserComparer : IComparer, IComparer<User>
        {
            public int Compare(object expected, object actual)
            {
                var lhs = expected as User;
                var rhs = actual as User;
                if (lhs == null || rhs == null) throw new InvalidOperationException();
                return Compare(lhs, rhs);
            }

            public int Compare(User expected, User actual)
            {
                if (expected.Id.CompareTo(actual.Id) == 0
                    && expected.FirstName.CompareTo(actual.FirstName) == 0
                    && expected.LastName.CompareTo(actual.LastName) == 0
                    && expected.Email.CompareTo(actual.Email) == 0)
                    return 0;

                return -1;
            }
        }

        #endregion
    }
}
