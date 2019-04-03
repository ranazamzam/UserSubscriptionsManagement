using System.Collections.Generic;
using System.Runtime.Serialization;

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
        public double TotalPriceIncVatAmount { get; set; }

        [DataMember]
        public double TotalCallMinutes { get; set; }
    }
}
