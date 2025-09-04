using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.DTOs;
using DeliveryAPi.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace DeliveryAPI.Persistence.Implementations.Services;

internal class JwtService(IConfiguration configuration): IJwtService
{
    public JwtTokenDto GenerateJwtToken(User user)
    {
        (string accessToken, DateTime valid) = GenerateAccessToken(user);
        string refreshToken = Guid.NewGuid().ToString();
        DateTime now = valid.AddMinutes(15);
        
        return new JwtTokenDto(accessToken, refreshToken, now);
    }

    private (string AccessToken, DateTime valiDateTime) GenerateAccessToken(User user)
    {
        ICollection<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.Surname , user.Surname),
            new Claim(ClaimTypes.Role , user.Role.ToString()),
            new Claim("UserProfileId", user.UserProfile?.Id.ToString()),
            new Claim("CurierProfileId", user.CurierProfile?.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        DateTime expiration = DateTime.UtcNow.AddMinutes(30);
        
        JwtSecurityToken jwtToken = new JwtSecurityToken(
            claims: claims,
            expires: expiration,
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            signingCredentials: credentials
            
        );
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string accessToken = handler.WriteToken(jwtToken);
        return (accessToken, expiration);
    }
}




