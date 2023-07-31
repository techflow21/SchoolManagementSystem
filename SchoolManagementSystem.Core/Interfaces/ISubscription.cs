using System;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ISubscription
    {
        Task<SubscriptionResponse> subscription(SubscriptionRequest subscriptionRequest);
    }
}

