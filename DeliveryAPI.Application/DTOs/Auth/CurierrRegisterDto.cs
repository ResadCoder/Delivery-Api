using DeliveryAPi.Domain.Enums;

namespace DeliveryAPI.Application.DTOs;

public record CurierrRegisterDto(string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    string Pin,
    TransportEnum TransportEnumType,
    string?TransportNumber);