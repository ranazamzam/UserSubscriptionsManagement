using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Domain.Models;

namespace UserSubscriptionsManagement.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Subscription GetSubscriptionById(int id);

        List<Subscription> GetAllSubscriptions();

        Guid AddSubscription(Subscription user);

        bool UpdateSubscription(Guid subscriptionId);

        bool DeleteSubscription(Guid subscription);
    }
}
