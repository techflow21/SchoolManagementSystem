using System;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Service.Implementation;

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscription _subscription;

        private readonly ITenantResolver _tenantService;

        public SubscriptionController(ISubscription subscription, ITenantResolver tenantService)
        {
            _subscription = subscription;

            _tenantService = tenantService;


        }

        [HttpPost("Subscription")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Subscription([FromForm]  SubscriptionRequest subscriptionPlans)
        {
            var Response = await _subscription.subscription(subscriptionPlans);

            if (Response.SubscriptionSuccess == false)
            {
                return BadRequest(Response);
            }

             return Ok(Response);
            
        }
    }
}

