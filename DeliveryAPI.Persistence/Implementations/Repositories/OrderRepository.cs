using DeliveryAPI.Application.Abstractions.Repositories.Order;
using DeliveryAPi.Domain.Entities;
using DeliveryAPI.Persistence.Context;

namespace DeliveryAPI.Persistence.Implementations.Repositories;

internal class OrderRepository(AppDbContext context) : GenericRepository<Order>(context), IOrderRepository
{
    
}