namespace DeliveryAPI.Infrastructure.Models;

public record SendConfirmOtpViewModel(string otpCode,DateTime expirationDate);