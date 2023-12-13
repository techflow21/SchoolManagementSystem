using System;

namespace SchoolManagementSystem.Core.Enums
{
    public enum SubscriptionPlans
    {
        Six_Month ,
        One_year,
        Two_Years,
        Six_Years
    }
    public static class GetPrices
    {
        public static int GetPriceValue(this SubscriptionPlans subscription)
        {
            return subscription switch
            {
                
                SubscriptionPlans.Six_Month => 50000,
                SubscriptionPlans.One_year => 90000,
                SubscriptionPlans.Two_Years => 160000,
                SubscriptionPlans.Six_Years => 300000,
                _ => 0
            };
        }
    }
}




