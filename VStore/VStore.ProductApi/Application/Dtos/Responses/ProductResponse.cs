namespace VStore.ProductApi.Application.Dtos.Responses
{
    public class ProductResponse
    {

        public int Id { get;  set; }
        public string Name { get;  set; }
        public decimal Price { get;  set; }
        public string Description { get;  set; }
        public long Stock { get;  set; }
        public string CategoryName { get;  set; }
    }
}
