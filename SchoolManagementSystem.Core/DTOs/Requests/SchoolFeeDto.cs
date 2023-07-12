namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class SchoolFeeDto
    {
        public int Id { get; set; }
        public string? Term { get; set; }
        public string? ClassName { get; set; }
        public string FeeName { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal TotalFees { get; set; }
        public DateTime SetDate { get; set; }
    }
}
