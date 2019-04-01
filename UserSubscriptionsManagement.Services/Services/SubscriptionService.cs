using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Domain.Models;
using UserSubscriptionsManagement.Services.Interfaces;

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

        public Guid AddSubscription(Subscription user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteSubscription(Guid subscription)
        {
            throw new NotImplementedException();
        }

        public List<Subscription> GetAllSubscriptions()
        {
            throw new NotImplementedException();
        }

        public Subscription GetSubscriptionById(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateSubscription(Guid subscriptionId)
        {
            throw new NotImplementedException();
        }
    }
}
