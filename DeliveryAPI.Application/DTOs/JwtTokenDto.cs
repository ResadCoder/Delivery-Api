namespace DeliveryAPI.Application.DTOs;

public record JwtTokenDto(string  AccessToken, string RefreshToken, DateTime RefreshTokenExpireTime);