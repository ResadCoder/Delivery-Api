using DeliveryAPI.Application.DTOs.Orders;
using DeliveryAPi.Domain.Entities;

namespace DeliveryAPI.Application.Abstractions;

public interface IOrderService
{
    Task CreateOrderAsync(OrderPostDto orderPostDto, CancellationToken cancellationToken);
    
    Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken);
    
    Task UpdateOrderAsync(PutOrderDto dto, CancellationToken cancellationToken);
    
    Task<IEnumerable<GetOrderItemDto>> GetAllOrdersAsync(int page,int take,CancellationToken cancellationToken);
    Task<GetOrderDto> GetOrderAsync(int id, CancellationToken cancellationToken);
    
    Task MakeOrderRequest(RequestOrderDto dto,CancellationToken cancellationToken);
    Task TakeOrder(int orderId, CancellationToken cancellationToken);
    
    Task CancelOrderByCurier(int orderId, CancellationToken cancellationToken);
    
    Task SelectOrderFromOrderRequest(int orderId, int orderRequestId, CancellationToken cancellationToken);
    
}