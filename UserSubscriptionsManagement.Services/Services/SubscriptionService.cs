using System;
using System.Collections.Generic;
using System.Linq;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Domain.Models;
using UserSubscriptionsManagement.Contracts.ServiceContracts;
using System.ServiceModel;
using UserSubscriptionsManagement.Utility;
using UserSubscriptionsManagement.Utility.ErrorHandler;

namespace UserSubscriptionsManagement.Services.Services
{
    [GlobalErrorBehaviorAttribute(typeof(GlobalErrorHandler))]
    public class SubscriptionService : ISubscriptionService
    {
        #region private fields
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region constructor
        public SubscriptionService(IUnitOfWork unitOfWork)
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
        /// Add subscription to the database
        /// </summary>
        /// <param name="userData">The subscription data of the subscription to be added</param>
        /// <returns></returns>
        public Guid AddSubscription(SubscriptionData subscriptionData)
        {
            var subscriptionEntity = subscriptionData.ToEntity();
            _unitOfWork.Repository<Subscription>().Insert(subscriptionEntity);
            _unitOfWork.Save();
            return subscriptionEntity.Id;
        }

        /// <summary>
        /// Delete subscription from the database
        /// </summary>
        /// <param name="userId">The id of the subscription to be deleted</param>
        /// <returns></returns>
        public bool DeleteSubscription(Guid id)
        {
            var subscription = _unitOfWork.Repository<Subscription>().GetById(id);

            if (subscription == null)
            {
                throw new FaultException<DataNotFoundFaultException>(new DataNotFoundFaultException()
                {
                    Result = false,
                    Message = $"A subscription with the following id ({id}) was not found"
                });
            }

            _unitOfWork.Repository<Subscription>().Delete(id);
            return _unitOfWork.Save() == 1 ? true : false;
        }

        /// <summary>
        /// Get all subscriptions
        /// </summary>
        /// <returns></returns>
        public List<SubscriptionData> GetAllSubscriptions()
        {
            var subscriptions = _unitOfWork.Repository<Subscription>().GetAllNoTracking.ToList();
            return subscriptions.ToModel();
        }

        /// <summary>
        /// Get the Subscription with the specified id
        /// </summary>
        /// <param name="id">The id of the subscription to retrieve</param>
        /// <returns></returns>
        public SubscriptionData GetSubscriptionById(Guid id)
        {
            var subscription = _unitOfWork.Repository<Subscription>().GetById(id);

            if (subscription == null)
            {
                throw new FaultException<DataNotFoundFaultException>(new DataNotFoundFaultException()
                {
                    Result = false,
                    Message = $"A subscription with the following id ({id}) was not found"
                });
            }
            
            return subscription.ToModel();
        }

        /// <summary>
        /// Update an exisiting subscription using its id
        /// </summary>
        /// <param name="subscriptionId">the id of the subscription to updated</param>
        /// <param name="subscriptionData">the new data of the subscription to updated</param>
        /// <returns></returns>
        public bool UpdateSubscription(Guid id, SubscriptionData subscriptionData)
        {
            var subscription = _unitOfWork.Repository<Subscription>().GetById(id);

            if (subscription == null)
            {
                throw new FaultException<DataNotFoundFaultException>(new DataNotFoundFaultException()
                {
                    Result = false,
                    Message = $"A subscription with the following id ({id}) was not found"
                });
            }

            subscriptionData.ToEntity(subscription);
            subscription.Id = id;

            _unitOfWork.Repository<Subscription>().Update(subscription);
            return _unitOfWork.Save() == 1 ? true : false;
        }
        #endregion
    }
}
