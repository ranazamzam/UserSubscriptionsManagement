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
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger _logger;

        public SubscriptionController(ISubscriptionService subscriptionService, ILogger<SubscriptionController> logger)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
        }

        #region  Actions
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SubscriptionData), (int)HttpStatusCode.OK)]
        public IActionResult GetSubscriptionById(Guid id)
        {
            try
            {
                var subscription = _subscriptionService.GetSubscriptionById(id);

                return Ok(subscription);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception("An exception occured.");
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<SubscriptionData>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllSubscriptions()
        {
            try
            {
                var subscriptions = _subscriptionService.GetAllSubscriptions();

                if (subscriptions.Any())
                {
                    return Ok(subscriptions);
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
        public IActionResult AddSubscription(SubscriptionData subscription)
        {
            try
            {
                var newId = _subscriptionService.AddSubscription(subscription);

                return CreatedAtAction(nameof(GetSubscriptionById), new { id = newId }, subscription);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new Exception("An exception occured.");
            }
        }


        [HttpPut]
        [Route("{id}")]
        public IActionResult UpdateSubscription(Guid id)
        {
            try
            {
                var success = _subscriptionService.UpdateSubscription(id);

                return NoContent();
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
        public IActionResult DeleteSubscription(Guid id)
        {
            try
            {
                var success = _subscriptionService.DeleteSubscription(id);

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