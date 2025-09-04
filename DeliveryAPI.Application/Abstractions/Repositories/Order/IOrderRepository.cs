using DeliveryAPI.Application.Abstractions.Repositories.Generic;
using  DeliveryAPi.Domain.Entities;

namespace DeliveryAPI.Application.Abstractions.Repositories.Order;

public interface IOrderRepository: IGenericRepository<DeliveryAPi.Domain.Entities.Order>
{
    
}