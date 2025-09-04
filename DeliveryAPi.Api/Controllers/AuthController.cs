using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAPi.Api.Controllers;

[Route("api/auth")]
[ApiController]

public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("/user/register")]
    
    public async Task<IActionResult> RegisterUser(UserRegisterDto dto,CancellationToken cancellationToken)
    {
        await service.RegisterUserAsycn(dto, cancellationToken);
        return Created();
    }
    
    [HttpPost("/curier/register")]
    
    public async Task<IActionResult> RegisterCurier(CurierrRegisterDto dto,CancellationToken cancellationToken)
    {
        await service.RegisterCurierAsync(dto, cancellationToken);
        return Created();
    }
    
    [HttpPost("/confirm-email")]
    
    public async Task<IActionResult> ConfirmEmailWithOtp(ConfirmEmailWithOtpCodeDto dto,CancellationToken cancellationToken)
    => Ok(await service.ConfirmEmailWithOtpAsync(dto, cancellationToken));
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginDto dto, CancellationToken cancellationToken)
    => Ok(await service.LoginAsync(dto, cancellationToken));

    [HttpPost("/logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await service.LogOutAsync(cancellationToken);
        return NoContent();
    }
}