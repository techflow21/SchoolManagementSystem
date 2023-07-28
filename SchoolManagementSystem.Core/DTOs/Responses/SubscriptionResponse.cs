using System;
namespace SchoolManagementSystem.Core.DTOs.Responses
{
    public class SubscriptionResponse
    {
        public bool SubscriptionSuccess { get; set; } = false;

        public bool MailSentSuccess { get; set; } = false;

        public string? Message { get; set; } = null!;
     }
}

