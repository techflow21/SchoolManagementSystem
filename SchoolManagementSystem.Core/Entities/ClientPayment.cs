namespace SchoolManagementSystem.Core.Entities
{
    public class ClientPayment : EntityBase
    {
        public string SchoolName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Subscription Subscription { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime Date { get; set; }
    }
}
