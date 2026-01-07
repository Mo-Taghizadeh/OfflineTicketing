using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using OfflineTicketing.Application.Abstractions;
using OfflineTicketing.Domain.Enums;

namespace OfflineTicketing.Infrastructure.Security;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http) => _http = http;

    public Guid? UserId
    {
        get
        {
            var id = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? _http.HttpContext?.User?.FindFirstValue("sub");
            return Guid.TryParse(id, out var g) ? g : null;
        }
    }

    public string? Email => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public UserRole? Role
    {
        get
        {
            var r = _http.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
            return Enum.TryParse<UserRole>(r, out var role) ? role : null;
        }
    }
}
