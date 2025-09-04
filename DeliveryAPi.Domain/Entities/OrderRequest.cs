using DeliveryAPi.Domain.Entities.Base;

namespace DeliveryAPi.Domain.Entities;

public class OrderRequest:BaseEntity
{
    public decimal Price { get; set; }
    public int CourierProfileId { get; set; }
    public CurierProfile CourierProfile { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}