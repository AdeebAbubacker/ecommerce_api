using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using System.Threading.Tasks;

[ApiController]
[Route("orderitems")]
[Authorize]
public class OrderItemsController : ControllerBase
{
    private readonly AppDbContext _db;
    public OrderItemsController(AppDbContext db) => _db = db;

    // DTOs for input binding
    public class OrderItemCreateDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderItemUpdateDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _db.OrderItems
            .Include(i => i.Product)
            .Include(i => i.Order)
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await _db.OrderItems
            .Include(i => i.Product)
            .Include(i => i.Order)
            .FirstOrDefaultAsync(i => i.Id == id);

        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderItemCreateDto dto)
    {
        // Validate foreign keys
        var orderExists = await _db.Orders.AnyAsync(o => o.Id == dto.OrderId);
        var productExists = await _db.Products.AnyAsync(p => p.Id == dto.ProductId);

        if (!orderExists || !productExists)
            return BadRequest("Invalid OrderId or ProductId.");

        var orderItem = new OrderItem
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };

        _db.OrderItems.Add(orderItem);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = orderItem.Id }, orderItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderItemUpdateDto dto)
    {
        var orderItem = await _db.OrderItems.FindAsync(id);
        if (orderItem == null) return NotFound();

        // Validate foreign keys
        var orderExists = await _db.Orders.AnyAsync(o => o.Id == dto.OrderId);
        var productExists = await _db.Products.AnyAsync(p => p.Id == dto.ProductId);

        if (!orderExists || !productExists)
            return BadRequest("Invalid OrderId or ProductId.");

        orderItem.OrderId = dto.OrderId;
        orderItem.ProductId = dto.ProductId;
        orderItem.Quantity = dto.Quantity;
        orderItem.UnitPrice = dto.UnitPrice;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var orderItem = await _db.OrderItems.FindAsync(id);
        if (orderItem == null) return NotFound();

        _db.OrderItems.Remove(orderItem);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
