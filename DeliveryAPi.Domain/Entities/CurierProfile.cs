using DeliveryAPi.Domain.Entities.Base;
using DeliveryAPi.Domain.Enums;

namespace DeliveryAPi.Domain.Entities;

public class CurierProfile : BaseEntity
{
    public string Pin { get; set; } = null!;
    public string? TransportNumber { get; set; }
    public  TransportEnum Transport { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<OrderRequest> OrderRequests { get; set; } = new List<OrderRequest>();
}