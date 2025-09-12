namespace VStore.ProductApi.Domain.Models
{
    public class Category
    {
        public Category()
        {
            
        }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<Product>? Products { get; private set; }
    }
}
