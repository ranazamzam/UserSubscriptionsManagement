﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Domain.Models;

namespace UserSubscriptionsManagement.Services.Interfaces
{
    public interface ISubscriptionService
    {
        SubscriptionData GetSubscriptionById(int id);

        List<SubscriptionData> GetAllSubscriptions();

        Guid AddSubscription(SubscriptionData user);

        bool UpdateSubscription(Guid subscriptionId);

        bool DeleteSubscription(Guid subscription);
    }
}
