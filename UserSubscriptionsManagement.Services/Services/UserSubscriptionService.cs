using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Services.Interfaces;

namespace UserSubscriptionsManagement.Services.Services
{
    public class UserSubscriptionService : IUserSubscriptionService
    {

        private readonly IUnitOfWork _unitOfWork;

        public UserSubscriptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public bool AddSubscriptionToUser(int userId, int subscriptionId)
        {
            throw new NotImplementedException();
        }
    }
}
