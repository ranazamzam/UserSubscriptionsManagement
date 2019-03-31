using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSubscriptionsManagement.Domain.Models
{
    public class UserSubscription
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public Guid SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
    }
}
