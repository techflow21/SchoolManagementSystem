namespace SchoolManagementSystem.Core.Entities
{
    public class SchoolFee : EntityBase
    {
        public string Class { get; set; }
        public string FeeName { get; set; }
        public decimal FeeAmount { get; set; }
    }
}
