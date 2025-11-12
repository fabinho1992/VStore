namespace VStore.OrderApi.Apllication_Order.Dtos.Inputs
{
    public class OrderInput
    {
        public Guid CustomerId { get; set; }
        public List<OrderItemInput> Items { get; set; } = new();

    }
}
