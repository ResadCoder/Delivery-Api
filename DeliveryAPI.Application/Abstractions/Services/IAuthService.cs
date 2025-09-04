using DeliveryAPI.Application.DTOs;

namespace DeliveryAPI.Application.Abstractions;

public interface IAuthService
{
    Task RegisterUserAsycn(UserRegisterDto dto,CancellationToken cancellationToken = default);
    
    Task RegisterCurierAsync(CurierrRegisterDto dto,CancellationToken cancellationToken = default);

    Task<string> ConfirmEmailWithOtpAsync(ConfirmEmailWithOtpCodeDto dto,CancellationToken cancellationToken = default);
    
    Task LogOutAsync(CancellationToken cancellationToken = default);

    Task<string> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
}