using DeliveryAPi.Domain.Enums;

namespace DeliveryAPI.Application.Abstractions;

public interface IUserIdentityService
{
    int UserId { get; }
    
    int? CurierProfileId { get; }
    
    int? UserProfileId { get; }
    string Email { get; }
    UserRoleEnum Role { get; }
    string Name { get; }
    string Surname { get; }
}