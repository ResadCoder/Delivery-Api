namespace DeliveryAPI.Application.DTOs.Orders;

public record GetOrderRequestItemDto(
    int Id,
    decimal CurierPrice,
    string CurierPin
    );