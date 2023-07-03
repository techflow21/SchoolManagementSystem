using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ITenantRegistry
    {
        Tenant[] GetTenants();
        User[] GetUsers();
    }
}
