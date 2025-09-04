using DeliveryAPI.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace DeliveryAPI.Persistence.Implementations.Services;

public class CookieService(IHttpContextAccessor httpContext) : ICookieService
{
    private readonly HttpContext _httpContext = httpContext.HttpContext ?? throw new ArgumentNullException(nameof(httpContext));

    public void SetCookie(string key, string value, CookieOptions? options)
    {
        _httpContext.Response.Cookies.Append(key, value, options ?? new CookieOptions());
    }

    public void RemoveCookie(string key)
    {
        _httpContext.Response.Cookies.Delete(key);
    }

    public string? GetCookie(string key)
    {
        return _httpContext.Request.Cookies[key];
    }

    public T? GetCookie<T>(string key)
    {
        string? json =  _httpContext.Request.Cookies[key]
            ?? throw new KeyNotFoundException();
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }
}