using DeliveryAPI.Application.DTOs;
using DeliveryAPi.Domain.Entities;

namespace DeliveryAPI.Application.Abstractions;

public interface IJwtService
{
    public JwtTokenDto GenerateJwtToken(User user);

}