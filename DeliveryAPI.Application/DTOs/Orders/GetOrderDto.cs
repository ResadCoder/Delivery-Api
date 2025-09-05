using DeliveryAPi.Domain.Enums;

namespace DeliveryAPI.Application.DTOs.Orders;

public record GetOrderDto(
    int Id,
    string RecieverFullName,
    string RecieverPhoneNumber,
    OrderStatusEnum Status,
    DateTime CreatedAt,
    decimal Price,
    string? Description,
    string Address,
    string ImageUrl
    );