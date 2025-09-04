using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.Abstractions.Repositories.Order;
using DeliveryAPI.Application.Abstractions.Repositories.Users;
using DeliveryAPI.Application.Abstractions.UnitOfWork;
using DeliveryAPI.Application.DTOs.Orders;
using DeliveryAPi.Domain.Entities;
using DeliveryAPi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class OrderService(
    IUserIdentityService userIdentity,IUserRepository userRepository
    ,IOrderRepository orderRepository,IUnitOfWork unitOfWork
    ,ICloudinaryService cloudinaryService
):IOrderService
{
    public async Task CreateOrderAsync(OrderPostDto orderPostDto, CancellationToken cancellationToken)
    {
        User? user  = await userRepository.GetByIdAsync(userIdentity.UserId, cancellationToken: cancellationToken)
            ?? throw new Exception("User not found");
        if (user.UserProfile == null)
            throw new Exception("UserProfileId not found");
        Order order = new Order
        {
            ReciverName = orderPostDto.ReceiverFullName,
            ReciverAddress = orderPostDto.ReceiverAddress,
            RecieverPhoneNumber = orderPostDto.ReceiverPhoneNumber,
            Description = orderPostDto.Description,
            Price = orderPostDto.Price,
            Status = OrderStatusEnum.Pending,
            UserProfileId = user.UserProfile.Id,
            ImageUrl = await cloudinaryService.UploadFileAsync(orderPostDto.ImageUrl,FileTypeEnum.Image,cancellationToken,"orders")
        };
        await orderRepository.CreateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken)
    {
        Order order = await orderRepository.GetByIdAsync(orderId, cancellationToken: cancellationToken)
            ?? throw new Exception("Order not found");

        if (order.Status != OrderStatusEnum.Pending)
            throw new Exception("Not pending order");
        
        orderRepository.Remove(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateOrderAsync(PutOrderDto dto, CancellationToken cancellationToken)
    {
        Order order = await orderRepository.GetByIdAsync(dto.Id, isTracking: true,cancellationToken: cancellationToken)
            ?? throw new Exception("Order not found");

        if (order.Status != OrderStatusEnum.Pending)
            throw new Exception("Not pending order");

        order.ReciverAddress = dto.ReceiverAddress;
        order.RecieverPhoneNumber = dto.ReceiverPhoneNumber;
        order.Description = dto.Description;
        order.Price = dto.Price;
        order.ReciverName = dto.ReceiverFullName;
        order.ImageUrl = dto.Photo is  not null ? await cloudinaryService.UploadFileAsync(dto.Photo,FileTypeEnum.Image,cancellationToken)
            : order.ImageUrl;
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<GetOrderItemDto>> GetAllOrdersAsync(int page, int take, CancellationToken cancellationToken)
     => await orderRepository
            .GetAll((page-1)*take, take)
            .Where(o => o.UserProfileId == userIdentity.UserProfileId)
            .Select(o => new GetOrderItemDto(
                o.Id,
                o.ReciverName,
                o.RecieverPhoneNumber,
                o.ReciverAddress,
                o.ImageUrl,
                o.Status,
                o.Price,
                o.CreatedAt
                ))
            .ToListAsync(cancellationToken: cancellationToken);
    
}