using DeliveryAPi.Domain.Enums;

namespace DeliveryAPi.Domain.Entities.Base;

public class OtpCode: BaseEntity
{
    public string Code { get; set; }=  null!;
    public DateTime ExpirationDate { get; set; }
    public OtpCodeTypeEnum OtpCodeType { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}