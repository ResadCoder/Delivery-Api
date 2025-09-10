using DeliveryAPi.Api.Attributes;
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
    [PaginationValidate]
    public async Task<IActionResult> GetOrders(int page = 1, int take = 10, CancellationToken cancellationToken = default)
    {
        return Ok( await orderService.GetAllOrdersAsync(page, take, cancellationToken));
    }
    
    [Authorize(Roles = "Customer")] 
    [HttpDelete("{id}")]
    [ValidateId]

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

    [Authorize(Roles = "Customer")]
    [HttpGet("{id}")]
    [ValidateId]
    public async Task<IActionResult> GetOrder(int id, CancellationToken cancellationToken)
    {
        return Ok(await orderService.GetOrderAsync(id, cancellationToken));
    }
    
    [Authorize(Roles = "Curier")]
    [HttpPut("request")]
    public async Task<IActionResult> MakeOrderRequest( RequestOrderDto dto, CancellationToken cancellationToken)
    {
        await orderService.MakeOrderRequest(dto, cancellationToken);
        return NoContent();
    }
    
    
    
    [Authorize(Roles = "Curier")]
    [HttpPut("take")]
    [ValidateId("orderId")]
    public async Task<IActionResult> TakeOrder(int orderId, CancellationToken cancellationToken)
    {
        await orderService.TakeOrder(orderId, cancellationToken);
        return NoContent();
    }
    
    
}