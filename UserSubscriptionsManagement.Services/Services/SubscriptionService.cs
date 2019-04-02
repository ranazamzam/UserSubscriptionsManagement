using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Domain.Models;
using UserSubscriptionsManagement.Contracts.ServiceContracts;

namespace UserSubscriptionsManagement.Services.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public Guid AddSubscription(SubscriptionData subscriptionData)
        {
            var subscriptionEntity = subscriptionData.ToEntity();
            _unitOfWork.Repository<Subscription>().Insert(subscriptionEntity);
            _unitOfWork.Save();
            return subscriptionEntity.Id;
        }

        public bool DeleteSubscription(Guid subscriptionId)
        {
            _unitOfWork.Repository<Subscription>().Delete(subscriptionId);
            return _unitOfWork.Save() == 1 ? true : false;
        }

        public List<SubscriptionData> GetAllSubscriptions()
        {
            var subscriptions = _unitOfWork.Repository<Subscription>().GetAllNoTracking.ToList();
            return subscriptions.ToModel();
        }

        public SubscriptionData GetSubscriptionById(Guid id)
        {
            var subscription = _unitOfWork.Repository<Subscription>().GetById(id);

            if (subscription == null)
            {

            }
            
            return subscription.ToModel();
        }

        public bool UpdateSubscription(Guid subscriptionId)
        {
            var subscription = _unitOfWork.Repository<Subscription>().GetById(subscriptionId);

            if (subscription == null)
            {

            }
            
            _unitOfWork.Repository<Subscription>().Update(subscription);
            return _unitOfWork.Save() == 1 ? true : false;
        }
    }
}
