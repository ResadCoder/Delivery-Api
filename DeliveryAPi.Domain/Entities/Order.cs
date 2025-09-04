using DeliveryAPi.Domain.Entities.Base;
using DeliveryAPi.Domain.Enums;

namespace DeliveryAPi.Domain.Entities;

public class Order : BaseEntity
{
    public string? Description { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string? ImageUrl { get; set; } 
    public decimal Price { get; set; } 
    public string ReciverName { get; set; } =  null!;
    public string ReciverAddress { get; set; } =  null!;
    public string RecieverPhoneNumber { get; set; } =  null!;
    public float? Rating { get; set; } 
    
    public int? UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; } = null!;
    
    public int? CourierId { get; set; }
    public CurierProfile? CurierProfile { get; set; } 
    
    public OrderStatusEnum Status { get; set; } 
    
    public ICollection<OrderRequest> OrderRequests { get; set; } = new List<OrderRequest>();
    
}