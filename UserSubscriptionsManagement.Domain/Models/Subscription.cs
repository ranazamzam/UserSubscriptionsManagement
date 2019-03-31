using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSubscriptionsManagement.Domain.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public double PriceIncVatAmount { get; set; }

        public int CallMinutes { get; set; }

        public virtual ICollection<UserSubscription> Users { get; set; }
    }
}
