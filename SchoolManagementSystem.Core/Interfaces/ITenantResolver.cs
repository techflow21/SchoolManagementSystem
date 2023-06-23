using SchoolManagementSystem.Core.Contracts;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ITenantResolver
    {
        Tenant GetCurrentTenant();
    }
}
