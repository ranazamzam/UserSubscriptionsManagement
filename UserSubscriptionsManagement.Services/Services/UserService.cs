using System;
using System.Collections.Generic;
using System.Linq;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Domain.Models;
using UserSubscriptionsManagement.Contracts.ServiceContracts;
using UserSubscriptionsManagement.Contracts.DataContracts;
using System.ServiceModel;
using UserSubscriptionsManagement.Utility.ErrorHandler;
using UserSubscriptionsManagement.Utility;

namespace UserSubscriptionsManagement.Services.Services
{
    [GlobalErrorBehaviorAttribute(typeof(GlobalErrorHandler))]
    public class UserService : IUserService
    {
        #region private fields
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        #region Services

        /// <summary>
        /// Add user to the database
        /// </summary>
        /// <param name="userData">The user data of the user to be added</param>
        /// <returns></returns>
        public int AddUser(UserData userData)
        {
            var userEntity = userData.ToEntity();
            _unitOfWork.Repository<User>().Insert(userEntity);
            _unitOfWork.Save();
            return userEntity.Id;
        }

        /// <summary>
        /// Delete user from the database
        /// </summary>
        /// <param name="userId">The id of the user to be deleted</param>
        /// <returns></returns>
        public bool DeleteUser(int userId)
        {
            var user = _unitOfWork.Repository<User>().GetById(userId);

            if (user == null)
            {
                throw new FaultException<DataNotFoundFaultException>(new DataNotFoundFaultException()
                {
                    Result = false,
                    Message = $"A user with the following id ({userId}) does not exist"
                });
            }

            _unitOfWork.Repository<User>().Delete(userId);
            return _unitOfWork.Save() == 1 ? true : false;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public List<UserData> GetAllUsers()
        {
            var users = _unitOfWork.Repository<User>().GetAllNoTracking.ToList();
            return users.ToModel();
        }

        /// <summary>
        /// Get the used with the specified id
        /// </summary>
        /// <param name="id">The id of the user to retrieve</param>
        /// <returns></returns>
        public UserData GetUserById(int id)
        {
            var user = _unitOfWork.Repository<User>().GetByIdInclude(x => x.Id == id, y => y.Subscriptions);

            if (user == null)
            {
                throw new FaultException<DataNotFoundFaultException>(new DataNotFoundFaultException()
                {
                    Result = false,
                    Message = $"A user with the following id ({id}) was not found"
                });
            }

            var userData = user.ToModel();
            userData.Subscriptions = new List<SubscriptionData>();

            foreach (var subscription in user.Subscriptions)
            {
                var subscriptionDetails = _unitOfWork.Repository<Subscription>().GetById(subscription.SubscriptionId);
                userData.Subscriptions.Add(subscriptionDetails.ToModel());
            }

            userData.TotalPriceIncVatAmount = userData.Subscriptions.Sum(item => item.PriceIncVatAmount);
            userData.TotalCallMinutes = userData.Subscriptions.Sum(item => item.CallMinutes);
            return userData;
        }

        /// <summary>
        /// Add a specific subscription to a specific user
        /// </summary>
        /// <param name="userId">The id of the user to be add the subscription to</param>
        /// <param name="subscriptionId">The id of the subscription to be added</param>
        /// <returns></returns>
        public bool AddSubscriptionToUser(int userId, Guid subscriptionId)
        {
            var user = _unitOfWork.Repository<User>().GetById(userId);

            if (user == null)
            {
                throw new FaultException<DataNotFoundFaultException>(new DataNotFoundFaultException()
                {
                    Result = false,
                    Message = $"A user with the following id ({userId}) was not found"
                });
            }

            var subscription = _unitOfWork.Repository<Subscription>().GetById(subscriptionId);

            if (subscription == null)
            {
                throw new FaultException<DataNotFoundFaultException>(new DataNotFoundFaultException()
                {
                    Result = false,
                    Message = $"A subscription with the following id ({subscriptionId}) was not found"
                });
            }

            var userSubscription = _unitOfWork.Repository<UserSubscription>()
                                              .Find(x => x.UserId == userId && x.SubscriptionId == subscriptionId);

            if (userSubscription.Any())
            {
                throw new FaultException<BusinessRuleFaultException>(new BusinessRuleFaultException()
                {
                    Result = false,
                    Message = $"The current user ({userId}) is already subscribed to the current subscription {subscriptionId}"
                });
            }

            _unitOfWork.Repository<UserSubscription>().Insert(new UserSubscription()
            {
                UserId = userId,
                SubscriptionId = subscriptionId
            });

            return _unitOfWork.Save() == 1 ? true : false;

        }

        #endregion
    }
}
