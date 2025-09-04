using DeliveryAPi.Domain.Entities.Base;
using DeliveryAPi.Domain.Enums;

namespace DeliveryAPi.Domain.Entities;

public class User: BaseEntity
{
    public string Name { get; set; } =  null!;
    
    public string Surname { get; set; }=  null!;
    
    public string Email { get; set; }=  null!;
    
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }=  null!;
    public string PasswordHash { get; set; } =  null!;
    
    public UserProfile? UserProfile { get; set; } 
    
    public CurierProfile? CurierProfile { get; set; } 
    
    public ICollection<OtpCode> OtpCodes { get; set; } =  new List<OtpCode>();
    
    public ICollection<UserAuthSession> UserAuthSessions { get; set; } =  new List<UserAuthSession>();
    public UserRoleEnum  Role { get; set; }
}