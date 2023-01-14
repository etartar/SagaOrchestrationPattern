namespace Order.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        public OrderStatus Status { get; set; }
        public string FailMessage { get; set; }
        public Address Address { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public static Order Create(string buyerId)
        {
            return new Order
            {
                BuyerId = buyerId,
                Status = OrderStatus.Suspend,
                CreatedDate = DateTime.Now,
                FailMessage = string.Empty
            };
        }

        public Order AddAddress(string line, string province, string district)
        {
            Address = new Address(line, province, district);
            return this;
        }

        public void AddOrderItem(int productId, decimal price, int count)
        {
            Items.Add(new OrderItem(Id, productId, price, count));
        }
    }
}
