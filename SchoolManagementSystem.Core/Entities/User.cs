using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Core.Entities;

public class User : IdentityUser
{
    public string Name { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string TenantId { get; set; } = null!;
}
