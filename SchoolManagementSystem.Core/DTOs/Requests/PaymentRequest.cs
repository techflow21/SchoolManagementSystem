using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class PaymentRequest
    {
        //[Required]
        //public string CurrentUserId { get; set; }
        [Required]
        public string Reference { get; set; }


        [Required]
        public int AmountInKobo { get; set; }
        [Required]
        public string Email { get; set; }

        //public string Plan { get; set; }



        public string CallbackUrl { get; set; }


        //public string SubAccount { get; set; }
        //[Required]

        //public int TransactionCharge { get; set; }


        public string Currency { get; set; } = "NGN";

        //public string Bearer { get; set; }

    }

}

