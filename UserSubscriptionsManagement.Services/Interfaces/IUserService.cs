using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Domain.Models;

namespace UserSubscriptionsManagement.Services.Interfaces
{
    public interface IUserService
    {
        User GetUserById(int id);

        List<User> GetAllUsers();

        int AddUser(User user);

        bool DeleteUser(int userId);
    }
}
