using System.Security.Claims;
using DeliveryAPI.Application.Abstractions;
using DeliveryAPi.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class UserIdentityService: IUserIdentityService
{
    private HttpContext _httpContext;
    public UserIdentityService(IHttpContextAccessor httpContextAccessor)
    {
        if(httpContextAccessor.HttpContext == null)
            throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        _httpContext = httpContextAccessor.HttpContext;
    }
    
    public int UserId => int.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public int? CurierProfileId
    {
        get
        {
            if ( _httpContext.User.FindFirstValue("CurierProfileId") is not null)
            {
              return int.Parse(_httpContext.User.FindFirstValue("CurierProfileId")!);
            }
            return null;
        }
    }

    public int? UserProfileId
    {
        get
        {
            if ( _httpContext.User.FindFirstValue("UserProfileId") is not null)
                return int.Parse(_httpContext.User.FindFirstValue("UserProfileId")!);
            return null;
        }
    }

    public string Email => _httpContext.User.FindFirstValue(ClaimTypes.Email)!;
    
    public UserRoleEnum Role => Enum.Parse<UserRoleEnum>(_httpContext.User.FindFirstValue(ClaimTypes.Role)!);
    
    public string Name => _httpContext.User.FindFirstValue(ClaimTypes.Name)!;
    
    public string Surname => _httpContext.User.FindFirstValue(ClaimTypes.Surname)!;
    
    
}