using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.Abstractions.Repositories.Order;
using DeliveryAPI.Application.Abstractions.Repositories.Users;
using DeliveryAPI.Application.Abstractions.UnitOfWork;
using DeliveryAPI.Application.DTOs.Orders;
using DeliveryAPI.Application.Exceptions.Common;
using DeliveryAPi.Domain.Entities;
using DeliveryAPi.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class OrderService(
    IUserIdentityService userIdentity,
    IUserRepository userRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    ICloudinaryService cloudinaryService
) : IOrderService
{
    public async Task CreateOrderAsync(OrderPostDto orderPostDto, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(userIdentity.UserId, cancellationToken: cancellationToken,
                         includes: [nameof(User.UserProfile)])
                     ?? throw new NotFoundException("User not found");
        if (user.UserProfile == null)
            throw new NotFoundException("UserProfileId not found");
        Order order = new Order
        {
            ReciverName = orderPostDto.ReceiverFullName,
            ReciverAddress = orderPostDto.ReceiverAddress,
            RecieverPhoneNumber = orderPostDto.ReceiverPhoneNumber,
            Description = orderPostDto.Description,
            Price = orderPostDto.Price,
            Status = OrderStatusEnum.Pending,
            UserProfileId = user.UserProfile.Id,
            ImageUrl = await cloudinaryService.UploadFileAsync(orderPostDto.ImageUrl, FileTypeEnum.Image,
                cancellationToken, "orders")
        };
        await orderRepository.CreateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteOrderAsync(int orderId, CancellationToken cancellationToken)
    {
        Order order = await orderRepository.GetByIdAsync(orderId, cancellationToken: cancellationToken)
                      ?? throw new NotFoundException("Order not found");

        if (order.Status != OrderStatusEnum.Pending)
            throw new NotPendingException("Not pending order");

        orderRepository.Remove(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateOrderAsync(PutOrderDto dto, CancellationToken cancellationToken)
    {
        Order order = await orderRepository.GetByIdAsync(dto.Id, isTracking: true, cancellationToken: cancellationToken)
                      ?? throw new NotFoundException("Order not found");

        if (order.Status != OrderStatusEnum.Pending)
            throw new NotPendingException("Not pending order");

        if (userIdentity.UserProfileId != order.UserProfileId)
            throw new ForbiddenException("U cannot change other person's order");

        order.ReciverAddress = dto.ReceiverAddress;
        order.RecieverPhoneNumber = dto.ReceiverPhoneNumber;
        order.Description = dto.Description;
        order.Price = dto.Price;
        order.ReciverName = dto.ReceiverFullName;
        order.ImageUrl = dto.Photo is not null
            ? await cloudinaryService.UploadFileAsync(dto.Photo, FileTypeEnum.Image, cancellationToken)
            : order.ImageUrl;

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<GetOrderItemDto>> GetAllOrdersAsync(int page, int take,
        CancellationToken cancellationToken)
        => await orderRepository
            .GetAll((page - 1) * take, take)
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

    public async Task<GetOrderDto> GetOrderAsync(int id, CancellationToken cancellationToken)
    {
        Order order = await orderRepository.GetByIdAsync(id, cancellationToken: cancellationToken)
                      ?? throw new NotFoundException("Order not found");

        if (userIdentity.UserProfileId != order.UserProfileId)
            throw new ForbiddenException("U cannot change other person's order");

        return new GetOrderDto(
            order.Id,
            order.ReciverName,
            order.RecieverPhoneNumber,
            order.Status,
            order.CreatedAt,
            order.Price,
            order.Description,
            order.ReciverAddress,
            order.ImageUrl
        );
    }
    
    public async Task MakeOrderRequest(RequestOrderDto dto, CancellationToken cancellationToken)
    {
        // if (userIdentity.CurierProfileId == null)
        // {
        //     throw new Exception("CurierProfileId not found");
        // }
        Order order = await orderRepository.GetByIdAsync(dto.Id,includes: [nameof(Order.OrderRequests)], isTracking:true, cancellationToken: cancellationToken)
            ?? throw new NotFoundException("Order not found");

        if(order.Status != OrderStatusEnum.Pending)
            throw new NotPendingException("Not pending order");
        
        if (order.OrderRequests.Any(r => r.CourierProfileId == userIdentity.CurierProfileId))
            throw new AlreadyExistException("You have already requested this order.");
        
        order.OrderRequests.Add(new OrderRequest
        {
            Price = dto.Price,
            CourierProfileId = (int)userIdentity.CurierProfileId!
        });
        
         await unitOfWork.SaveChangesAsync(cancellationToken);
    }
    

}