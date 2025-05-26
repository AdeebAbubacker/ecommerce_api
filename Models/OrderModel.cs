using System.Text.Json.Serialization;
public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending"; 
    public DateTime CreatedAt { get; set; }

    public UserModel User { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}