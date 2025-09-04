namespace DeliveryAPI.Application.DTOs;

public record UserRegisterDto(string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    string Address
    );
