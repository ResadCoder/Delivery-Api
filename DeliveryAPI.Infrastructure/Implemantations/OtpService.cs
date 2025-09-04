using System.Security.Cryptography;
using System.Text;
using DeliveryAPI.Application.Abstractions;

namespace DeliveryAPI.Persistence.Implementations.Services;
internal class OtpService : IOtpService
{
    public string GetOtpCode()
    {
        var bytes = new byte[5];
        RandomNumberGenerator.Fill(bytes);

        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            sb.Append(b % 10);
        }
        return sb.ToString();
    }
}