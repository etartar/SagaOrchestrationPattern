using System.ComponentModel.DataAnnotations.Schema;

namespace Order.API.Models
{
    public class OrderItem
    {
        public OrderItem()
        {
        }

        public OrderItem(int orderId, int productId, decimal price, int count)
        {
            OrderId = orderId;
            ProductId = productId;
            Price = price;
            Count = count;
        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int Count { get; set; }

        public Order Order { get; set; }
    }
}
