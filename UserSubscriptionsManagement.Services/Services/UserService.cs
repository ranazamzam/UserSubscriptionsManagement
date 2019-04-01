using System;
using System.Collections.Generic;
using System.Linq;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Domain.Models;
using UserSubscriptionsManagement.Contracts.ServiceContracts;
using UserSubscriptionsManagement.Contracts.DataContracts;

namespace UserSubscriptionsManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public int AddUser(UserData userData)
        {
            var userEntity = userData.ToEntity();
            _unitOfWork.Repository<User>().Insert(userEntity);
            _unitOfWork.Save();
            return userEntity.Id;
        }

        public bool DeleteUser(int userId)
        {
            _unitOfWork.Repository<User>().Delete(userId);
            return _unitOfWork.Save() == 1 ? true : false;
        }

        public List<UserData> GetAllUsers()
        {
            var users = _unitOfWork.Repository<User>().GetAllNoTracking.ToList();
            return users.ToModel();
        }

        public UserData GetUserById(int id)
        {
            var user = _unitOfWork.Repository<User>().GetById(id);

            if (user == null)
            {

            }

            var userData = user.ToModel();
            userData.Subscriptions = new List<SubscriptionData>();

            foreach (var subscription in user.Subscriptions)
            {
                userData.Subscriptions.Add(subscription.Subscription.ToModel());
            }

            return userData;
        }

        public bool AddSubscriptionToUser(int userId, Guid subscriptionId)
        {
            var user = _unitOfWork.Repository<User>().GetById(userId);

            if (user == null)
            {

            }

            var subscription = _unitOfWork.Repository<Subscription>().GetById(subscriptionId);

            if (subscription == null)
            {

            }

            var userSubscription = _unitOfWork.Repository<UserSubscription>()
                                              .Find(x => x.UserId == userId && x.SubscriptionId == subscriptionId);

            if (userSubscription.Any())
            {

            }

            _unitOfWork.Repository<UserSubscription>().Insert(new UserSubscription()
            {
                UserId = userId,
                SubscriptionId = subscriptionId
            });

            return _unitOfWork.Save() == 1 ? true : false;

        }
    }
}
