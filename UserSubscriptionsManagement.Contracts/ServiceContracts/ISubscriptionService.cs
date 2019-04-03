using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Contracts.DataContracts;

namespace UserSubscriptionsManagement.Contracts.ServiceContracts
{
    [ServiceContract]
    public interface ISubscriptionService
    {
        [OperationContract]
        [FaultContract(typeof(DataNotFoundFaultException))]
        SubscriptionData GetSubscriptionById(Guid id);

        [OperationContract]
        List<SubscriptionData> GetAllSubscriptions();

        [OperationContract]
        Guid AddSubscription(SubscriptionData user);

        [OperationContract]
        [FaultContract(typeof(DataNotFoundFaultException))]
        bool UpdateSubscription(Guid id, SubscriptionData subscriptionData);

        [OperationContract]
        [FaultContract(typeof(DataNotFoundFaultException))]
        bool DeleteSubscription(Guid id);
    }
}
