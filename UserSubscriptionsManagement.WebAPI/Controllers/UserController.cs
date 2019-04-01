using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Contracts.ServiceContracts;

namespace UserSubscriptionsManagement.WebAPI.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        #region  Actions
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UserData), (int)HttpStatusCode.OK)]
        public IActionResult GetUserById(int userId)
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
                _logger.LogCritical(ex.Message);
                throw new Exception("An exception occured.");
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<UserData>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllUsers()
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
                _logger.LogCritical(ex.Message);
                throw new Exception("An exception occured.");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserData), (int)HttpStatusCode.OK)]
        public IActionResult AddUser(UserData user)
        {
            try
            {
                var newUserId = _userService.AddUser(user);

                return CreatedAtAction(nameof(GetUserById), new { userId = newUserId }, user);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception("An exception occured.");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(UserData), (int)HttpStatusCode.OK)]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                var success = _userService.DeleteUser(userId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception("An exception occured.");
            }
        }

        [HttpPut]
        [Route("{userId}/{subscriptionId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult AddSubsriptionToUser(int userId, Guid subscriptionId)
        {
            try
            {
                var success = _userService.AddSubscriptionToUser(userId, subscriptionId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception("An exception occured.");
            }
        }
        #endregion
    }
}