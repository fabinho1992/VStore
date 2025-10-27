using VStore.OrderApi.Apllication_Order.Dtos;

namespace VStore.OrderApi.Apllication_Order.ProductHttpClient
{
    public interface IHttpGetProducts
    {
        Task<List<ProductConsumerDto>> ProductsForOrder(List<int> productIds);  
    }
}
