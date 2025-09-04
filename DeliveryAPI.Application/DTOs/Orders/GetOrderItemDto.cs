using DeliveryAPi.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Application.DTOs.Orders;

public record GetOrderItemDto(
    int Id,
    string RecieverFullName,
    string RecieverPhoneNumber,
    string RecieverAddress,
    string? ImageUrl,
    OrderStatusEnum Status,
    decimal Price,
    DateTime CreatedAt
    
    );