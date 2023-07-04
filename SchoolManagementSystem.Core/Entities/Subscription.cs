namespace SchoolManagementSystem.Core.Entities
{
    public class Subscription : EntityBase
    {
        public string Name { get; set; }
        public int? Duration { get; set; } // In months
        public DateTime SubscribedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
