using DeliveryAPi.Domain.Enums;

namespace DeliveryAPI.Infrastructure.Models;

public record SendOrderStatusViewModel(
    string ReceiverName,
    OrderStatusEnum Status,
    string? Description = null,
    string? CurierName = null,
    string? CurierPhoneNumber = null,
    string? CurierCarNumber = null
);