﻿using System;
using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class SubscriptionRequest
    {
        [Required(ErrorMessage = "Subscription Plan Must be Selected")]
        public SubscriptionPlans subscription { get; set; } 

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(pattern: "/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$/", ErrorMessage = "Email format is wrong" )]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name of person performing the payment is required")]
        public string Name { get; set; }
        
    }
}

