using System.ComponentModel.DataAnnotations;

namespace VStore.OrderApi.Domain.Models
{
    public class OrderItem
    {
        
        public int Id { get; private set; }
        public int OrderId { get; set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Subtotal { get; private set; }
        public virtual Order Order { get; set; }

        internal OrderItem(int productId, string productName, int quantity, decimal unitPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            CalculateSubtotal();
        }

        private void CalculateSubtotal()
        {
            Subtotal = Quantity * UnitPrice;
        }

        internal void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            Quantity = newQuantity;
            CalculateSubtotal();
        }

        // Método para atualizar informações do produto (se necessário)
        internal void UpdateProductInfo(string productName, decimal unitPrice)
        {
            ProductName = productName;
            UnitPrice = unitPrice;
            CalculateSubtotal();
        }
    }
}
