using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSubscriptionsManagement.Services.Interfaces
{
    public interface IUserSubscriptionService
    {
        bool AddSubscriptionToUser(int userId, int subscriptionId);
    }
}
