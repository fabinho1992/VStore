
using VStore.Shared.Contracts.Dtos;
using VStore.Shared.Contracts.Enuns;

namespace VStore.Shared.Contracts.Events
{
    public class OrderCreatedEvent
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
    }
}
