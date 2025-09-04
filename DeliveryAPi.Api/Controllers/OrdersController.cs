using DeliveryAPI.Application.Abstractions;
using DeliveryAPI.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAPi.Api.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController(IOrderService orderService) : ControllerBase
{ 
    
    [Authorize(Roles = "Customer")] 
    [HttpPost]
    public async Task<IActionResult> Create([FromForm]OrderPostDto orderPostDto, CancellationToken cancellationToken)
    {
        await orderService.CreateOrderAsync(orderPostDto, cancellationToken);
        return StatusCode(201);
    }

    [Authorize(Roles = "Customer")] 
    [HttpGet]
    public async Task<IActionResult> GetOrders(int page = 1, int take = 10, CancellationToken cancellationToken = default)
    {
        return Ok( await orderService.GetAllOrdersAsync(page, take, cancellationToken));
    }
    
    [Authorize(Roles = "Customer")] 
    [HttpDelete("{id}")]

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await orderService.DeleteOrderAsync(id, cancellationToken);
        return NoContent();
    }
    
    [Authorize(Roles = "Customer")] 
    [HttpPut]

    public async Task<IActionResult> UpdateOrder(PutOrderDto dto, CancellationToken cancellationToken)
    {
        await orderService.UpdateOrderAsync(dto, cancellationToken);
        return NoContent();
    }
}