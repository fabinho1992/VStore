namespace VStore.ProductApi.Application.Dtos.Responses
{
    public class ProductResponse
    {
        public ProductResponse()
        {
            
        }

        public ProductResponse(int id, string name, decimal price, string description, long stock, string categoryName)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            Stock = stock;
            CategoryName = categoryName;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public long Stock { get; private set; }
        public string CategoryName { get; private set; }
    }
}
