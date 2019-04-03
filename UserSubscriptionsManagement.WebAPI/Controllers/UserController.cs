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
    [RoutePrefix("api/users")]
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
        public IHttpActionResult GetUserById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var user = _userService.GetUserById(id);

            return Ok(user);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();

            if (users.Any())
            {
                return Ok(users);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddUser(UserData user)
        {
            var newUserId = _userService.AddUser(user);

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            var success = _userService.DeleteUser(id);

            return Ok();
        }

        [HttpPut]
        [Route("{userId}/{subscriptionId}")]
        public IHttpActionResult AddSubsriptionToUser(int userId, Guid subscriptionId)
        {
            var success = _userService.AddSubscriptionToUser(userId, subscriptionId);

            return Ok();
        }
        #endregion
    }
}