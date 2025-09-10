using DeliveryAPI.Application.DTOs.Orders;

namespace DeliveryAPI.Application.Abstractions;

public interface IOrderRequestService
{
    Task<IEnumerable<GetOrderRequestItemDto>> GetOrderRequestItemAsync(int id, CancellationToken cancellationToken);
}