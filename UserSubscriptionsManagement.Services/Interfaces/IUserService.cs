﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Domain.Models;

namespace UserSubscriptionsManagement.Services.Interfaces
{
    public interface IUserService
    {
        UserData GetUserById(int id);

        List<UserData> GetAllUsers();

        int AddUser(UserData user);

        bool DeleteUser(int userId);

        bool AddSubscriptionToUser(int userId, Guid subscriptionId);
    }
}
