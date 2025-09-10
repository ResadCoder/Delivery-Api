using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.Abstractions.Repositories.OrderRequest;
using DeliveryAPI.Application.Abstractions.Repositories.Users;
using DeliveryAPI.Application.DTOs.Orders;
using DeliveryAPI.Application.Exceptions.Common;
using DeliveryAPi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class OrderRequestService(IOrderRequestRepository orderRequest,IUserIdentityService userIdentity): IOrderRequestService
{
    public async Task<IEnumerable<GetOrderRequestItemDto>> GetOrderRequestItemAsync(int id, CancellationToken cancellationToken)
    {
        return await orderRequest.GetAny(
                or => or.OrderId == id && or.Order.UserProfileId == userIdentity.UserProfileId,
                isTracking: false,
                orderBy:or=> or.Price
                )
            .Select(or => new GetOrderRequestItemDto(
                or.Id,
                or.Price,
                or.CourierProfile.Pin
            ))
            .ToListAsync(cancellationToken: cancellationToken); 
    }
}
