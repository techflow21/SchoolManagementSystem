namespace SchoolManagementSystem.Core.Entities
{
    public class Subscription : EntityBase
    {
        public string Name { get; set; }
        public int? Duration { get; set; } = 0;// In months
        public DateTime SubscribedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
