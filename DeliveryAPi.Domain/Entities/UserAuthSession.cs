namespace DeliveryAPi.Domain.Entities.Base;

public class UserAuthSession:BaseEntity
{
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}