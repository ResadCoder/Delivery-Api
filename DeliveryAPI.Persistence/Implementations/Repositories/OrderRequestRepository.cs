using DeliveryAPI.Application.Abstractions.Repositories.Order;
using DeliveryAPI.Application.Abstractions.Repositories.OrderRequest;
using DeliveryAPi.Domain.Entities;
using DeliveryAPI.Persistence.Context;

namespace DeliveryAPI.Persistence.Implementations.Repositories;

internal class OrderRequestRepository(AppDbContext context): GenericRepository<OrderRequest>(context), IOrderRequestRepository
{
    
}