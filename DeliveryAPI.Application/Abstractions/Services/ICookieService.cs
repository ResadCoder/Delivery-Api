using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Application.Abstractions;

public interface ICookieService
{
    void SetCookie(string key, string value, CookieOptions? options);
    void RemoveCookie(string key);
    string? GetCookie(string key);
    
    T? GetCookie<T>(string key);
}