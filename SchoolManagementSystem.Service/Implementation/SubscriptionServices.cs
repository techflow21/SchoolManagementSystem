using System;
using Microsoft.Extensions.Configuration;
using PayStack.Net;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class SubscriptionServices : ISubscription
    {

        private readonly IConfiguration _configuration;

        private readonly IRepository<Subscription> _subscription;

        private readonly IUnitOfWork _unitOfWork;
        

        //ClientPayment is the class, the file name is contact, Subscription  class too

        public SubscriptionServices(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;

            _unitOfWork = unitOfWork;

            _subscription = unitOfWork.GetRepository<Subscription>();
        }

       

        public async Task<SubscriptionResponse> subscription(SubscriptionRequest subscriptionRequest)
        {
            

            var payment = new PaymentRequest()
            {
                CallbackUrl = "https://localhost:7067/swagger/index.html",
                Currency = "NGN",
                Reference = $"SchoolMgtApp{GenerateRandomNumber()}",
                Email = subscriptionRequest.Email,

                
            };


            switch (subscriptionRequest.subscription)
            {
                case SubscriptionPlans.Six_Month:
                    return await SixMonthPlan(payment);

                case SubscriptionPlans.One_year:
                    return await OneYearPlan(payment);

                case SubscriptionPlans.Two_Years:
                    return await TwoYearsPlan(payment);

                case SubscriptionPlans.Six_Years:
                    return await SixYearPlan(payment);

                default:
                    return new SubscriptionResponse()
                    {
                        Message = "Invalid Selection",

                        SubscriptionSuccess = false
                    }
                    ;
            }




        }

        private TransactionInitializeResponse MakePayment(PaymentRequest depositRequest)
        {
            string secret = _configuration.GetSection("ApiSecret").GetSection("SecretKey").Value ?? " ";
            PayStackApi payStack = new(secret);


            TransactionInitializeRequest initializeRequest = MapToTransactionInitializeRequest(depositRequest);


            var result = payStack.Transactions.Initialize(initializeRequest);


            return result;
        }

       

        private TransactionInitializeRequest MapToTransactionInitializeRequest(PaymentRequest depositRequest)
        {
            return new TransactionInitializeRequest
            {
                
                Currency = depositRequest.Currency,
                //Bearer = depositRequest.Bearer,
                AmountInKobo = depositRequest.AmountInKobo,
                Email = depositRequest.Email,
                //TransactionCharge = depositRequest.TransactionCharge,
                Reference = depositRequest.Reference,
                CallbackUrl = depositRequest.CallbackUrl,
                //SubAccount = depositRequest.SubAccount,
                //Plan = depositRequest.Plan,
                

            };
        }


        private async Task<SubscriptionResponse> SixMonthPlan(PaymentRequest payment)
        {
            var subscriptionResponse = new SubscriptionResponse();
            payment.AmountInKobo = SubscriptionPlans.Six_Month.GetPriceValue() * 100;

            var subscription = new Subscription()
            {
                Name = "Six Month Plan",

                Duration = DateTime.Now.ToUniversalTime().AddMonths(6) - DateTime.Now.ToUniversalTime(),

                SubscribedDate = DateTime.Now.ToUniversalTime(),

                ExpiryDate = DateTime.Now.ToUniversalTime().AddMonths(6)

            };

            var result = MakePayment(payment);

            if (result.Status)
            {
                await _subscription.AddAsync(subscription);
                var SaveChanges = await _unitOfWork.SaveChangesAsync();

                if (SaveChanges == 1)
                {

                    subscriptionResponse.Message = "Six Month Subscription was successfull";

                    subscriptionResponse.SubscriptionSuccess = true;

                    return subscriptionResponse;
                }

                
            }
            subscriptionResponse.Message = "Six Month Subscription was unsuccessfull";

            subscriptionResponse.SubscriptionSuccess = false;

            return subscriptionResponse;

        }

        private async Task<SubscriptionResponse> OneYearPlan(PaymentRequest payment)
        {
            var subscriptionResponse = new SubscriptionResponse();

            payment.AmountInKobo = SubscriptionPlans.One_year.GetPriceValue() * 100;
            var subscription = new Subscription()
            {
                Name = "One year Plan",

                Duration = DateTime.Now.ToUniversalTime().AddYears(1) - DateTime.Now.ToUniversalTime(),

                SubscribedDate = DateTime.Now.ToUniversalTime(),

                ExpiryDate = DateTime.Now.ToUniversalTime().AddYears(1)

            };

            var result = MakePayment(payment);

            if (result.Status)
            {
                await _subscription.AddAsync(subscription);
                var SaveChanges = await _unitOfWork.SaveChangesAsync();

                if (SaveChanges == 1)
                {
                    subscriptionResponse.Message = "One year Subscription was successfull";

                    subscriptionResponse.SubscriptionSuccess = true;

                    return subscriptionResponse;
                }

                

            }

            subscriptionResponse.Message = "One year Subscription was unsuccessfull";

            subscriptionResponse.SubscriptionSuccess = false;

            return subscriptionResponse;

        }

        private async Task<SubscriptionResponse> TwoYearsPlan(PaymentRequest payment)
        {
            var subscriptionResponse = new SubscriptionResponse();

            payment.AmountInKobo = SubscriptionPlans.Two_Years.GetPriceValue() * 100;

            var subscription = new Subscription()
            {
                Name = "Two years Plan",

                Duration = DateTime.Now.ToUniversalTime().AddYears(2) - DateTime.Now.ToUniversalTime(),

                SubscribedDate = DateTime.Now.ToUniversalTime(),

                ExpiryDate = DateTime.Now.ToUniversalTime().AddYears(2)

            };
            var result = MakePayment(payment);

            if (result.Status)
            {
                await _subscription.AddAsync(subscription);
                var SaveChanges = await _unitOfWork.SaveChangesAsync();

                if (SaveChanges == 1)
                {
                    subscriptionResponse.Message = "Two years Subscription was successfull";

                    subscriptionResponse.SubscriptionSuccess = true;

                    return subscriptionResponse;
                }
            }
                

            subscriptionResponse.Message = "Two years Subscription was unsuccessfull";

            subscriptionResponse.SubscriptionSuccess = false;

            return subscriptionResponse;
        }

        private async Task<SubscriptionResponse> SixYearPlan(PaymentRequest payment)
        {
            var subscriptionResponse = new SubscriptionResponse();

            payment.AmountInKobo = SubscriptionPlans.Six_Years.GetPriceValue() * 100;

            var subscription = new Subscription()
            {
                Name = "Six yearss Plan",

                Duration = DateTime.Now.ToUniversalTime().AddYears(6) - DateTime.Now.ToUniversalTime(),

                SubscribedDate = DateTime.Now.ToUniversalTime(),

                ExpiryDate = DateTime.Now.ToUniversalTime().AddYears(6)

            };

            var result = MakePayment(payment);

            if (result.Status)
            {
                await _subscription.AddAsync(subscription);
                var SaveChanges = await _unitOfWork.SaveChangesAsync();

                if (SaveChanges == 1)
                {
                    subscriptionResponse.Message = "Six years Subscription was successfull";

                    subscriptionResponse.SubscriptionSuccess = true;

                    return subscriptionResponse;
                }
            }

               

            subscriptionResponse.Message = "Six years Subscription was unsuccessfull";

            subscriptionResponse.SubscriptionSuccess = false;

            return subscriptionResponse;

        }

        private  string GenerateRandomNumber()
        {
            Random random = new Random();

            var randomNumber = random.Next(100000000, 999999999);


            return randomNumber.ToString();
        }
    }

}



//var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
//var api = new PayStackApi(testOrLiveSecret);



// Initializing a transaction
//var response = api.Transactions.Initialize("user@somewhere.net", 5000000);
//if (response.Status)
//  // use response.Data
//else
//            // show response.Message

//            // Verifying a transaction
//            var verifyResponse = api.Transactions.Verify("transaction-reference"); // auto or supplied when initializing;
//if (verifyResponse.Status)
  /* 
      You can save the details from the json object returned above so that the authorization code 
      can be used for charging subsequent transactions
      
      // var authCode = verifyResponse.Data.Authorization.AuthorizationCode
      // Save 'authCode' for future charges!
  */