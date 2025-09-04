namespace DeliveryAPI.Application.Abstractions;

public interface IHashService
{
    string Hash(string input);
    public bool VerifyHash(string input, string hash);
}