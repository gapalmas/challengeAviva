using Req = App.Core.Dto.Request;
using App.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderManager manager) : ControllerBase
    {
        private readonly IOrderManager _manager = manager;

        [HttpPost] public async Task<IActionResult> Create(Req.OrderRequestDto dto) => Ok(await _manager.CreateOrderAsync(dto));
        [HttpGet] public async Task<IActionResult> Get() => Ok(await _manager.GetAllAsync());
        [HttpGet("{id}")] public async Task<IActionResult> Get(Guid id) => Ok(await _manager.GetByIdAsync(id));
        [HttpPost("{id}/cancel")] public async Task<IActionResult> Cancel(Guid id) { await _manager.CancelAsync(id); return NoContent(); }
        [HttpPost("{id}/pay")] public async Task<IActionResult> Pay(Guid id) { await _manager.PayAsync(id); return NoContent(); }
    }
}
