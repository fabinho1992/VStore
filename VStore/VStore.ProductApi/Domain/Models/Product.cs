namespace VStore.ProductApi.Domain.Models;

public class Product
{
    public Product()
    {
        
    }

    public Product(int id, string name, decimal price, string description, long stock, string imageUrl, int categoryId)
    {
        Id = id;
        Name = name;
        Price = price;
        Description = description;
        Stock = stock;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public long Stock { get; private set; }
    public string ImageUrl { get; private set; }
    public Category Catergory { get; set; }
    public int CategoryId { get; private set; }

    public void UpdateStock(long quantity)
    {
        Stock += quantity;
    }

    public void UpdateId(int id)
    {
        Id = id;
    }
}
