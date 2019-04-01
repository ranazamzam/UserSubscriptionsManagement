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
    public interface IUserService
    {
        [OperationContract]
        UserData GetUserById(int id);

        [OperationContract]
        List<UserData> GetAllUsers();

        [OperationContract]
        int AddUser(UserData user);

        [OperationContract]
        bool DeleteUser(int userId);

        [OperationContract]
        bool AddSubscriptionToUser(int userId, Guid subscriptionId);
    }
}
