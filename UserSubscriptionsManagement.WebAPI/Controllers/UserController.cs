using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Contracts.ServiceContracts;

namespace UserSubscriptionsManagement.WebAPI.Controllers
{
    [Route("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region  Actions
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetUserById(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return BadRequest();
                }

                var user = _userService.GetUserById(userId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured.");
            }
        }

        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();

                if (users.Any())
                {
                    return Ok(users);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured.");
            }
        }

        [HttpPost]
        public IHttpActionResult AddUser(UserData user)
        {
            try
            {
                var newUserId = _userService.AddUser(user);

                return Ok( user);
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured.");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteUser(int userId)
        {
            try
            {
                var success = _userService.DeleteUser(userId);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured.");
            }
        }

        [HttpPut]
        [Route("{userId}/{subscriptionId}")]
        public IHttpActionResult AddSubsriptionToUser(int userId, Guid subscriptionId)
        {
            try
            {
                var success = _userService.AddSubscriptionToUser(userId, subscriptionId);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured.");
            }
        }
        #endregion
    }
}