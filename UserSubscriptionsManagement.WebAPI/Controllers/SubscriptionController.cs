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
    public class SubscriptionController : ApiController
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        #region  Actions
        [System.Web.Http.HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetSubscriptionById(Guid id)
        {
            try
            {
                var subscription = _subscriptionService.GetSubscriptionById(id);

                return Ok(subscription);
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured.");
            }
        }

        [HttpGet]
        public IHttpActionResult GetAllSubscriptions()
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
                throw new Exception("An exception occured.");
            }
        }

        [HttpPost]
        public IHttpActionResult AddSubscription(SubscriptionData subscription)
        {
            try
            {
                var newId = _subscriptionService.AddSubscription(subscription);

                return Ok(subscription);
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured.");
            }
        }


        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateSubscription(Guid id)
        {
            try
            {
                var success = _subscriptionService.UpdateSubscription(id);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occured.");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSubscription(Guid id)
        {
            try
            {
                var success = _subscriptionService.DeleteSubscription(id);

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