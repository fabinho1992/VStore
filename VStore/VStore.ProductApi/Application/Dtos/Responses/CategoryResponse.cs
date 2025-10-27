using VStore.ProductApi.Domain.Models;

namespace VStore.ProductApi.Application.Dtos.Responses
{
    public class CategoryResponse
    {
        public CategoryResponse()
        {
            
        }

        public CategoryResponse(int id, string name, List<ProductResponseName> products)
        {
            Id = id;
            Name = name;
            NameProduct = products;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<ProductResponseName> NameProduct { get; private set; }
    }
}
