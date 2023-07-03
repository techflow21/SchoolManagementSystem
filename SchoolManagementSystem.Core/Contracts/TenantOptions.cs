using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Contracts
{
    public class TenantOptions
    {
        public string? DefaultConnection { get; set; }
        public Tenant[] Tenants { get; set; } = Array.Empty<Tenant>();
        public User[] Users { get; set; } = Array.Empty<User>();
    }
}
