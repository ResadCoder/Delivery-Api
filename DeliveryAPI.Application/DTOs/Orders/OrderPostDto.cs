using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Application.DTOs.Orders;

public record OrderPostDto(
    string ReceiverFullName,
    string ReceiverEmail,
    string ReceiverPhoneNumber,
    string ReceiverAddress,
    IFormFile? ImageUrl,
    string Description,
    decimal Price
    );