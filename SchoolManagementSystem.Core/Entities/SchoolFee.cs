namespace SchoolManagementSystem.Core.Entities
{
    public class SchoolFee : EntityBase
    {
        public Class Class { get; set; }
        public string FeeName { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal TotalFees { get; set; }
    }
}
