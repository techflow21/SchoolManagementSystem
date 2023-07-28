using System;
using Microsoft.Extensions.Configuration;
using PayStack.Net;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class SubscriptionServices : ISubscription
    {

        private readonly IConfiguration _configuration;
        

        //ClientPayment is the class, the file name is contact, Subscription  class too

        public SubscriptionServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TransactionInitializeResponse MakePayment(DepositRequest depositRequest)
        {
            string secret = _configuration.GetSection("ApiSecret").GetSection("SecretKey").Value;
            PayStackApi payStack = new(secret);
            TransactionInitializeRequest initializeRequest = MapToTransactionInitializeRequest(depositRequest);
            var result = payStack.Transactions.Initialize(initializeRequest);
            return result;
        }

        public TransactionVerifyResponse VerifyPayment(string referenceCode)
        {
            string secret = _configuration.GetSection("ApiSecret").GetSection("SecretKey").Value;
            PayStackApi payStack = new(secret);
            TransactionVerifyResponse result = payStack.Transactions.Verify(referenceCode);
            return result;

        }

        private TransactionInitializeRequest MapToTransactionInitializeRequest(DepositRequest depositRequest)
        {
            return new TransactionInitializeRequest
            {
                
                Currency = depositRequest.Currency,
                Bearer = depositRequest.Bearer,
                AmountInKobo = depositRequest.AmountInKobo,
                Email = depositRequest.Email,
                TransactionCharge = depositRequest.TransactionCharge,
                Reference = depositRequest.Reference,
                CallbackUrl = depositRequest.CallbackUrl,
                SubAccount = depositRequest.SubAccount,
                Plan = depositRequest.Plan,
                

            };
        }
    }
}

