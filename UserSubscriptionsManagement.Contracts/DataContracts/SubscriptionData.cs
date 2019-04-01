using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UserSubscriptionsManagement.Contracts.DataContracts
{
    [DataContract]
    public class SubscriptionData
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public double PriceIncVatAmount { get; set; }

        [DataMember]
        public int CallMinutes { get; set; }
    }
}
