using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Contracts.ServiceContracts;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Domain.Models;
using UserSubscriptionsManagement.Services.Services;
using UserSubscriptionsManagement.Services;
using System.Linq.Expressions;

namespace UserSubscriptionsManagement.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {

        private IUserService _userService;
        private List<User> _users;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<User>> _mockUserRepository;

        #region One time setup before all tests
        [OneTimeSetUp]
        public void Setup()
        {
            _users = SetUpUsers();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            AutoMapperConfiguration.Configure();
        }
        #endregion

        #region  setup before each test
        [SetUp]
        public void ReInitializeTest()
        {
            _users = SetUpUsers();
            _mockUserRepository = new Mock<IRepository<User>>();
        }
        #endregion

        #region unit tests

        [Test]
        public void GetAllUsers_ThereIsUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            _mockUserRepository.Setup(p => p.GetAllNoTracking).Returns(_users.AsQueryable());
            _mockUnitOfWork.Setup(s => s.Repository<User>()).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object);

            // Act
            var users = _userService.GetAllUsers();

            // Assert
            var usersAsEntity = users.ToEntity();
            var comparer = new PatientEntityComparer();
            CollectionAssert.AreEqual(usersAsEntity.OrderBy(product => product, comparer),
                                     _users.OrderBy(product => product, comparer), comparer);

            Assert.AreEqual(2, users.Count);

            Assert.IsInstanceOf(typeof(List<UserData>), users);
        }

        [Test]
        public void GetAllUsers_IfThereIsNoUsers_ShouldReturnEmptyList()
        {
            _users.Clear();
            _mockUserRepository.Setup(p => p.GetAllNoTracking).Returns(_users.AsQueryable());
            _mockUnitOfWork.Setup(s => s.Repository<User>()).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object);

            var users = _userService.GetAllUsers();

            Assert.AreEqual(0, users.Count);
        }

        [Test]
        public void GetUserById_UserIdExists_ShouldReturnTheRightUserItem()
        {
            List<Subscription> subscriptions = SetUpSubscriptions();
            Mock<IRepository<Subscription>> mockSubscriptionRepository = new Mock<IRepository<Subscription>>();
            mockSubscriptionRepository.Setup(p => p.GetById(It.IsAny<object[]>()))
                                             .Returns(new Func<object[], Subscription>(id => subscriptions.Find(p => p.Id == new Guid(id[0].ToString()))));

            _mockUserRepository.Setup(p => p.GetByIdInclude(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, object>>[]>()))
                                            .Returns(new Func<Expression<Func<User, bool>>, Expression<Func<User, object>>[], User>((id, argm2) => _users.AsQueryable().First(id)));
            _mockUnitOfWork.Setup(s => s.Repository<User>()).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.Setup(s => s.Repository<Subscription>()).Returns(mockSubscriptionRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object);

            var userData = _userService.GetUserById(1);

            var userFromTestList = _users.Find(a => a.Id == 1);

            Assert.AreEqual(userFromTestList.Id, userData.Id);
            Assert.AreEqual(userFromTestList.FirstName, userData.FirstName);

            Assert.IsInstanceOf(typeof(UserData), userData);

        }

        [Test]
        public void AddUser_ShouldbeSuccessful()
        {
            // Arrange
            _mockUserRepository.Setup(p => p.Insert((It.IsAny<User>())))
                               .Callback(new Action<User>(newUser =>
                               {
                                   var lastUserId = _users.Last().Id;
                                   var nextUserId = lastUserId + 1;
                                   newUser.Id = nextUserId;
                                   _users.Add(newUser);
                               }));
            _mockUnitOfWork.Setup(s => s.Repository<User>()).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object);
            var usersCountBeforeAdd = _users.Count;
            var latUserIdBeforeAdd = _users.Max(a => a.Id);

            // Act
            var newUserId = _userService.AddUser(new UserData
            {
                FirstName = "test first 3",
                LastName = "test last 3",
                Email = "e@gmail.com"
            });

            // Assert

            Assert.That(latUserIdBeforeAdd + 1, Is.EqualTo(_users.Last().Id));
            Assert.AreEqual("test first 3", _users.Find(x => x.Id == newUserId).FirstName);
            Assert.AreEqual(usersCountBeforeAdd + 1, _users.Count);
        }

        [Test]
        public void DeleteUser_ShouldbeSuccessful()
        {
            // Arrange
            _mockUserRepository.Setup(p => p.Delete(It.IsAny<object[]>()))
                               .Callback(new Action<object[]>(id =>
                               {
                                   var userToBeRemoved = _users.Find(a => a.Id == int.Parse(id[0].ToString()));

                                   if (userToBeRemoved != null)
                                       _users.Remove(userToBeRemoved);
                               }));
            _mockUnitOfWork.Setup(s => s.Repository<User>()).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object);
            var usersCountBeforeDelete = _users.Count;
            var latUserIdBeforeAdd = _users.Max(a => a.Id);

            // Act
            var success = _userService.DeleteUser(1);

            // Assert
            Assert.AreEqual(null, _users.Find(x => x.Id == 1));
            Assert.AreEqual(usersCountBeforeDelete - 1, _users.Count);
        }
        #endregion
        #region tear down after every test 

        /// <summary>
        /// Tears down each test data
        /// </summary>
        [TearDown]
        public void DisposeTest()
        {
            _userService = null;
            _mockUserRepository = null;
            _users = null;
        }
        #endregion

        #region One time tear down after all tests

        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            _users = null;
            _mockUnitOfWork = null;
        }
        #endregion

        #region Private members methods

        private IRepository<User> SetUpUserRepository()
        {
            var mockRepo = new Mock<IRepository<User>>();

            mockRepo.Setup(p => p.GetAllNoTracking).Returns(_users.AsQueryable());

            mockRepo.Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(new Func<int, User>(id => _users.Find(p => p.Id == id)));

            mockRepo.Setup(p => p.GetById(It.IsAny<int>()))
                    .Returns(new Func<int, User>(id => _users.Find(p => p.Id == id)));

            //mockRepo.Setup(p => p.GetByIdInclude(It.IsAny<int>()))
            //       .ReturnsAsync(new Func<int, User>(id => _users.Find(p => p.Id == id)));

            mockRepo.Setup(p => p.Insert((It.IsAny<User>())))
                     .Callback(new Action<User>(newUser =>
                     {
                         var lastUserId = _users.Last().Id;
                         var nextUserId = lastUserId + 1;
                         newUser.Id = nextUserId;
                         _users.Add(newUser);
                     }));

            mockRepo.Setup(p => p.Delete(It.IsAny<int>()))
                .Callback(new Action<int>(userId =>
                {
                    var userToBeRemoved = _users.Find(a => a.Id == userId);

                    if (userToBeRemoved != null)
                        _users.Remove(userToBeRemoved);
                }));

            return mockRepo.Object;
        }

        #endregion

        #region Helpers and Data Initializer
        private List<User> SetUpUsers()
        {
            return new List<User>()
            {
                new User
                {
                    Id=1,
                    FirstName = "First Name1",
                    LastName = "Last Name1",
                    Email = "email@email.com",
                    Subscriptions = new List<UserSubscription>()
                    {
                        new UserSubscription()
                        {
                            SubscriptionId = new Guid("9DCB2E8C-2869-4A43-BADD-9E50B82A20D0"),
                        },
                        new UserSubscription
                        {
                           SubscriptionId =  new Guid("98D3D585-E4E8-4E60-AE4A-C7D7F3E8DD5B")
                        }
                    }
                },
                new User
                {
                    Id=2,
                    FirstName = "First Name2",
                    LastName = "Last Name2",
                    Email = "email2@email.com"
                },
            };
        }

        private List<Subscription> SetUpSubscriptions()
        {
            return new List<Subscription>()
            {
                new Subscription
                {
                    Id = new Guid("9DCB2E8C-2869-4A43-BADD-9E50B82A20D0"),
                    Name ="Test Sub 1",
                    Price = 10,
                    PriceIncVatAmount = 20,
                    CallMinutes = 20
                },
                new Subscription
                {
                    Id = new Guid("98D3D585-E4E8-4E60-AE4A-C7D7F3E8DD5B"),
                    Name ="Test Sub 2",
                    Price = 20,
                    PriceIncVatAmount = 30,
                    CallMinutes = 30
                }
            };
        }

        public class PatientEntityComparer : IComparer, IComparer<User>
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
