using VStore.OrderApi.Domain.Models.Enum;
using VStore.Shared.Contracts.Dtos;

namespace VStore.OrderApi.Apllication_Order.Dtos.Response
{
    public class OrderResponse
    {
        public int Id { get;  set; }
        public Guid CustomerId { get;  set; }
        public OrderStatus Status { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public decimal TotalAmount { get;  set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new();
    }
}
