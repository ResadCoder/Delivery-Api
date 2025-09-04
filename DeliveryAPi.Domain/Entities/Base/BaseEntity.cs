namespace DeliveryAPi.Domain.Entities.Base;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }
    public virtual DateTime CreatedAt { get; set; }
    public virtual DateTime UpdatedAt { get; set; }
    public virtual bool IsDeleted { get; set; }
    
}