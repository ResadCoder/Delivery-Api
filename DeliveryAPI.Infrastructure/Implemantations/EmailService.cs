using System.Net;
using System.Net.Mail;
using DeliveryAPI.Application.Abstractions;

using Microsoft.Extensions.Configuration;
using DeliveryAPI.Infrastructure.Models;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class EmailService(IConfiguration configuration,IRazerRenderViewService renderViewService): IEmailService
{
    
    public async  Task SendConfirmationOtpAsync(string recieverEmail, SendConfirmOtpViewModel model,CancellationToken cancellationToken = default)
    {   
        string body = await renderViewService.RenderViewToStringAsync("ConfirmEmailView", model);
        await SendEmailAsync(recieverEmail, body,"Confirm your email", cancellationToken: cancellationToken);
    }

    private async Task SendEmailAsync(string reciever, string body, string subject, bool isBodyHtml = true,CancellationToken cancellationToken = default)
    {
        
        using var smtp = new SmtpClient
        {
            Host = configuration["ApplicationEmail:Host"]!,
            Port = int.Parse(configuration["ApplicationEmail:Port"]!),
            Credentials = new NetworkCredential(
                configuration["ApplicationEmail:Email"]!,
                configuration["ApplicationEmail:Password"]!
            ),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Timeout = 20000
        };
        MailAddress fromAddress =  new MailAddress(configuration["ApplicationEmail:Email"]!, "Delivery API");
        MailAddress toAddress = new MailAddress(reciever);
        MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = isBodyHtml;
        await smtp.SendMailAsync(mailMessage, cancellationToken);
    }
}