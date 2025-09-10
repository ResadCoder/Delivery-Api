using DeliveryAPI.Infrastructure.Models;

namespace DeliveryAPI.Application.Abstractions;

public interface IEmailService
{
  Task SendConfirmationOtpAsync(string recieverEmail,SendConfirmOtpViewModel model,CancellationToken cancellationToken = default);
  
  Task SendOrderStatusAsync(string recieverEmail,SendOrderStatusViewModel model,CancellationToken cancellationToken = default);
}