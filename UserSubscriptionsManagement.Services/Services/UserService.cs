using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Domain.Interfaces;
using UserSubscriptionsManagement.Domain.Models;
using UserSubscriptionsManagement.Services.Interfaces;

namespace UserSubscriptionsManagement.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
        
        public int AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            var patients = _unitOfWork.Repository<User>().GetAllNoTracking.ToList();
        }

        public User GetUserById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
