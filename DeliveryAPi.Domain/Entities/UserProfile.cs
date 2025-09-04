using DeliveryAPi.Domain.Entities.Base;

namespace DeliveryAPi.Domain.Entities;

public class UserProfile : BaseEntity
{
    public string? Address { get; set; }
    public int  UserId { get; set; }
    public User? User { get; set; } =  null!;
}