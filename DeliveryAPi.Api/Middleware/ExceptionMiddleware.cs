using System.Net;
using Microsoft.AspNetCore.Http;
using DeliveryAPI.Application.Exceptions.Base;

namespace DeliveryAPi.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (BaseException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)ex.Status;

                var res = new { Status = (int)ex.Status, Message = ex.Message };
                await context.Response.WriteAsJsonAsync(res);
            }
            catch (Exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
            
                var res = new { Status = 500, Message = "Unexpected error occurred." };
                await context.Response.WriteAsJsonAsync(res);
            }
        }
    }
}