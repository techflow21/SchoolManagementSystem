using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Infrastructure.Configurations;

namespace SchoolManagementSystem.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly ITenantRegistry _tenantRegistry;

    public AuthenticationController(ITenantRegistry tenantRegistry, ILoggerManager logger)
    {
        _tenantRegistry = tenantRegistry;
        _logger = logger;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginRequest)
    {
        if (loginRequest is null)
            return BadRequest();

        var user = _tenantRegistry.GetUsers()
            .FirstOrDefault(e => e.Name == loginRequest.UserName && e.Secret == loginRequest.Password);

        if (user is null)
            return Unauthorized("Invalid user");
        _logger.LogWarn("Unauthorized user try to login");

        if (_tenantRegistry.GetTenants().FirstOrDefault(e => e.Name == user.TenantId) is not { } tenant)
            return Unauthorized("Invalid tenant");

        var tokenString = JwtHelper.GenerateToken(tenant);
        _logger.LogInfo("Tenant logged in successful");

        return Ok(new { Token = tokenString });
    }
}
