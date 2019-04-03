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
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetSubscriptionById(Guid id)
        {
            var subscription = _subscriptionService.GetSubscriptionById(id);

            return Ok(subscription);
        }

        [HttpGet]
        public IHttpActionResult GetAllSubscriptions()
        {
            var subscriptions = _subscriptionService.GetAllSubscriptions();

            if (subscriptions.Any())
            {
                return Ok(subscriptions);
            }

            return NotFound();
        }

        [HttpPost]
        public IHttpActionResult AddSubscription(SubscriptionData subscription)
        {
            var newId = _subscriptionService.AddSubscription(subscription);

            return Ok(subscription);
        }


        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateSubscription(Guid id, SubscriptionData subscriptionData)
        {
            var success = _subscriptionService.UpdateSubscription(id, subscriptionData);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSubscription(Guid id)
        {
            var success = _subscriptionService.DeleteSubscription(id);

            return Ok();
        }
        #endregion
    }
}