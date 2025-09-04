using DeliveryAPI.Application.Abstractions;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class HashService: IHashService
{
    public string Hash(string input)
    {
        return BCrypt.Net.BCrypt.HashPassword(input);
    }

    public bool VerifyHash(string input, string hash)
    {
        if(string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty", nameof(input));
        if(string.IsNullOrEmpty(hash))
            throw new ArgumentException("Hash cannot be null or empty", nameof(hash));
        return BCrypt.Net.BCrypt.Verify(input, hash);
    }
}