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
    public class SubscriptionServiceTests
    {
        private ISubscriptionService _subcriptionService;
        private List<Subscription> _subscriptions;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IRepository<Subscription>> _mockSubscriptionRepository;

        #region One time setup before all tests
        [OneTimeSetUp]
        public void Setup()
        {
            _subscriptions = SetUpSubscriptions();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            AutoMapperConfiguration.Configure();
        }
        #endregion

        #region  setup before each test
        [SetUp]
        public void ReInitializeTest()
        {
            _subscriptions = SetUpSubscriptions();
            _mockSubscriptionRepository = new Mock<IRepository<Subscription>>();
        }
        #endregion

        #region unit tests

        [Test]
        public void GetAllSubscriptions_ThereIsSubscriptions_ShouldReturnListOfSubscriptions()
        {
            // Arrange
            _mockSubscriptionRepository.Setup(p => p.GetAllNoTracking).Returns(_subscriptions.AsQueryable());
            _mockUnitOfWork.Setup(s => s.Repository<Subscription>()).Returns(_mockSubscriptionRepository.Object);
            _subcriptionService = new SubscriptionService(_mockUnitOfWork.Object);

            // Act
            var subscriptions = _subcriptionService.GetAllSubscriptions();

            // Assert
            var subscriptionAsEntity = subscriptions.ToEntity();
            var comparer = new SubscriptionComparer();
            CollectionAssert.AreEqual(subscriptionAsEntity.OrderBy(s => s, comparer),
                                     _subscriptions.OrderBy(s => s, comparer), comparer);

            Assert.AreEqual(2, subscriptions.Count);

            Assert.IsInstanceOf(typeof(List<SubscriptionData>), subscriptions);
        }

        [Test]
        public void GetAllSubscriptions_IfThereIsNoSubscriptions_ShouldReturnEmptyList()
        {
            _subscriptions.Clear();
            _mockSubscriptionRepository.Setup(p => p.GetAllNoTracking).Returns(_subscriptions.AsQueryable());
            _mockUnitOfWork.Setup(s => s.Repository<Subscription>()).Returns(_mockSubscriptionRepository.Object);
            _subcriptionService = new SubscriptionService(_mockUnitOfWork.Object);

            var subscriptions = _subcriptionService.GetAllSubscriptions();

            Assert.AreEqual(0, subscriptions.Count);
        }

        [Test]
        public void GetSubscriptionById_SubscriptionIdExists_ShouldReturnTheRightSubscriptionItem()
        {
            _mockSubscriptionRepository.Setup(p => p.GetById(It.IsAny<object[]>()))
                                       .Returns(new Func<object[], Subscription>(id => _subscriptions.Find(p => p.Id == new Guid(id[0].ToString()))));

            _mockUnitOfWork.Setup(s => s.Repository<Subscription>()).Returns(_mockSubscriptionRepository.Object);
            _subcriptionService = new SubscriptionService(_mockUnitOfWork.Object);

            var subscrtiptionData = _subcriptionService.GetSubscriptionById(new Guid("9DCB2E8C-2869-4A43-BADD-9E50B82A20D0"));

            var subscriptionFromTestList = _subscriptions.Find(a => a.Id == new Guid("9DCB2E8C-2869-4A43-BADD-9E50B82A20D0"));

            Assert.AreEqual(subscriptionFromTestList.Id, subscrtiptionData.Id);
            Assert.AreEqual(subscriptionFromTestList.Name, subscrtiptionData.Name);

            Assert.IsInstanceOf(typeof(SubscriptionData), subscrtiptionData);

        }

        [Test]
        public void AddSubscription_ShouldbeSuccessful()
        {
            // Arrange
            _mockSubscriptionRepository.Setup(p => p.Insert((It.IsAny<Subscription>())))
                                       .Callback(new Action<Subscription>(newSubcription =>
                                       {
                                           _subscriptions.Add(newSubcription);
                                       }));
            _mockUnitOfWork.Setup(s => s.Repository<Subscription>()).Returns(_mockSubscriptionRepository.Object);
            _subcriptionService = new SubscriptionService(_mockUnitOfWork.Object);
            var subscriptionsCountBeforeAdd = _subscriptions.Count;

            // Act
            var newId = _subcriptionService.AddSubscription(new SubscriptionData
            {
                Id = new Guid("E9F3F089-6D6F-4D51-B1BE-1103F37EC9BE"),
                Name = "test sub 3",
                Price = 10,
                CallMinutes = 10,
                PriceIncVatAmount = 20
            });

            // Assert
            Assert.AreEqual("test sub 3", _subscriptions.Find(x => x.Id == new Guid("E9F3F089-6D6F-4D51-B1BE-1103F37EC9BE")).Name);
            Assert.AreEqual(subscriptionsCountBeforeAdd + 1, _subscriptions.Count);
        }

        [Test]
        public void DeleteSubscription_ShouldbeSuccessful()
        {
            // Arrange
            _mockSubscriptionRepository.Setup(p => p.Delete(It.IsAny<object[]>()))
                                       .Callback(new Action<object[]>(id =>
                                       {
                                            var itemToBeRemoved = _subscriptions.Find(a => a.Id == new Guid(id[0].ToString()));

                                            if (itemToBeRemoved != null)
                                                _subscriptions.Remove(itemToBeRemoved);
                                       }));
            _mockUnitOfWork.Setup(s => s.Repository<Subscription>()).Returns(_mockSubscriptionRepository.Object);
            _subcriptionService = new SubscriptionService(_mockUnitOfWork.Object);
            var usersCountBeforeDelete = _subscriptions.Count;

            // Act
            var success = _subcriptionService.DeleteSubscription(new Guid("98D3D585-E4E8-4E60-AE4A-C7D7F3E8DD5B"));

            // Assert
            Assert.AreEqual(usersCountBeforeDelete - 1, _subscriptions.Count);
        }
        #endregion
        #region tear down after every test 

        /// <summary>
        /// Tears down each test data
        /// </summary>
        [TearDown]
        public void DisposeTest()
        {
            _subcriptionService = null;
            _mockSubscriptionRepository = null;
            _subscriptions = null;
        }
        #endregion

        #region One time tear down after all tests

        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
            _subscriptions = null;
            _mockUnitOfWork = null;
        }
        #endregion

        #region Helpers and Data Initializer
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

        public class SubscriptionComparer : IComparer, IComparer<Subscription>
        {
            public int Compare(object expected, object actual)
            {
                var lhs = expected as Subscription;
                var rhs = actual as Subscription;
                if (lhs == null || rhs == null) throw new InvalidOperationException();
                return Compare(lhs, rhs);
            }

            public int Compare(Subscription expected, Subscription actual)
            {
                if (expected.Id.CompareTo(actual.Id) == 0
                    && expected.Name.CompareTo(actual.Name) == 0
                    && expected.Price.CompareTo(actual.Price) == 0
                    && expected.PriceIncVatAmount.CompareTo(actual.PriceIncVatAmount) == 0
                    && expected.CallMinutes.CompareTo(actual.CallMinutes) == 0)
                    return 0;

                return -1;
            }
        }

        #endregion
    }
}
