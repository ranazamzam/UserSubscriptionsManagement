using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserSubscriptionsManagement.Contracts.DataContracts
{
    [DataContract]
    public class UserData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public ICollection<SubscriptionData> Subscriptions { get; set; }

        [DataMember]
        public double TotalPriceIncVatAmount => Subscriptions.Sum(subscription => subscription.PriceIncVatAmount);

        [DataMember]
        public double TotalCallMinutes => Subscriptions.Sum(subscription => subscription.CallMinutes);
    }
}
