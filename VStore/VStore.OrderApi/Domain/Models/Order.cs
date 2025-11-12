using VStore.OrderApi.Domain.Models.Enum;

namespace VStore.OrderApi.Domain.Models;

public class Order
{
    public Order(Guid customerId)
    {
        CustomerId = customerId;
        Status = OrderStatus.PendingPayment;
        CreatedDate = DateTime.UtcNow;
        TotalAmount = 0;
        _items = new List<OrderItem>();
    }

    public int Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public decimal TotalAmount { get; private set; }

    private List<OrderItem> _items;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public void AddItem(int productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var item = new OrderItem(productId, productName, quantity, unitPrice);
            _items.Add(item);
        }

        UpdateTotalAmount();
    }

    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            UpdateTotalAmount();
        }
    }

    public void UpdateItemQuantity(int productId, int newQuantity)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.UpdateQuantity(newQuantity);
            UpdateTotalAmount();
        }
    }

    private void UpdateTotalAmount()
    {
        TotalAmount = _items.Sum(item => item.Subtotal);
    }

    public void UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }
}
