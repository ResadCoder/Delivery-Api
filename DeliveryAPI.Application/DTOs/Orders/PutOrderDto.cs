using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Application.DTOs.Orders;

public record PutOrderDto(
    int Id,
    string ReceiverFullName,
    string ReceiverPhoneNumber,
    string ReceiverAddress,
    IFormFile? Photo,
    string Description,
    decimal Price
    );