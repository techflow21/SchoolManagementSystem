namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class ClassFeeDto
    {
        public string? Term { get; set; }
        public string? Class { get; set; }
        public string FeeName { get; set; }
        public decimal FeeAmount { get; set; }
    }
}
