using DeliveryAPi.Api.Attributes;
using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAPi.Api.Controllers
{
    [Route("api/orderr-equests")]
    [ApiController]
    
    public class OrderRequestsController(IOrderRequestService orderRequestService) : ControllerBase
    {
        [HttpGet("orderrequests/get/{orderId}")] 
        [Authorize(Roles = "Customer")]
        [ValidateId("orderId")]
        public async Task<IActionResult> GetOrderRequests(int orderId, CancellationToken cancellationToken)
        { 
            return Ok(await orderRequestService.GetOrderRequestItemAsync(orderId, cancellationToken));
        }
    }
}