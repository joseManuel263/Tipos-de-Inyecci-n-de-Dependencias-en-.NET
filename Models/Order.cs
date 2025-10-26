namespace TiposDeInyecciónDeDependencias.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}