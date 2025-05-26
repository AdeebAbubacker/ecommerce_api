// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using MyApi.Data;

// [ApiController]
// [Route("orders")]
// [Authorize]
// public class OrdersController : ControllerBase
// {
//     private readonly AppDbContext _db;
//     public OrdersController(AppDbContext db) => _db = db;

//     [HttpGet]
//     public async Task<IActionResult> GetAll()
//     {
//         var orders = await _db.Orders
//             .Include(o => o.OrderItems)
//             .ThenInclude(oi => oi.Product)
//             .ToListAsync();
//         return Ok(orders);
//     }

//     [HttpGet("{id}")]
//     public async Task<IActionResult> Get(int id)
//     {
//         var order = await _db.Orders
//             .Include(o => o.OrderItems)
//             .ThenInclude(oi => oi.Product)
//             .FirstOrDefaultAsync(o => o.Id == id);
//         return order == null ? NotFound() : Ok(order);
//     }

//     [HttpPost]
//     public async Task<IActionResult> Create(Order order)
//     {
//         order.CreatedAt = DateTime.UtcNow;
//         _db.Orders.Add(order);
//         await _db.SaveChangesAsync();
//         return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
//     }

//     [HttpPut("{id}")]
//     public async Task<IActionResult> Update(int id, Order updated)
//     {
//         var order = await _db.Orders.FindAsync(id);
//         if (order == null) return NotFound();

//         order.Status = updated.Status;
//         order.TotalPrice = updated.TotalPrice;

//         await _db.SaveChangesAsync();
//         return NoContent();
//     }

//     [HttpDelete("{id}")]
//     public async Task<IActionResult> Delete(int id)
//     {
//         var order = await _db.Orders.FindAsync(id);
//         if (order == null) return NotFound();

//         _db.Orders.Remove(order);
//         await _db.SaveChangesAsync();
//         return NoContent();
//     }
// }
