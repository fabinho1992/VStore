
using System.Text;
using System.Text.Json;
using VStore.OrderApi.Apllication_Order.Dtos;

namespace VStore.OrderApi.Apllication_Order.ProductHttpClient
{
    public class HttpGetProducts : IHttpGetProducts
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public HttpGetProducts(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<List<ProductConsumerDto>> ProductsForOrder(List<int> ids)
        {
            // 1. Converte a lista de IDs em uma string separada por vírgulas
            var idsParam = string.Join(",", ids);

            // 2. Monta a URL com os IDs como parâmetro de query
            var url = $"{_configuration["ProductService:BaseUrl"]}/api/Products/products-ids?ids={idsParam}";

            // 3. Faz a chamada HTTP
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // 4. Desserializa a resposta
            var content = await response.Content.ReadAsStringAsync();

            // Log para debug
            Console.WriteLine($"JSON Recebido: {content}");

            // Desserializa para o wrapper ApiResponse
            var apiResponse = JsonSerializer.Deserialize<ResultViewModel<List<ProductConsumerDto>>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // Retorna a lista de produtos da propriedade Data, ou lista vazia se for null
            return apiResponse?.Data ?? new List<ProductConsumerDto>();
        }
        //public async Task<List<ProductConsumerDto>> ProductsForOrder(List<int> ids)
        //{
        //    // 1. Converte a lista de IDs em uma string separada por vírgulas
        //    var idsParam = string.Join(",", ids);
        //    // Exemplo: Se productIds = [1, 2, 3] → idsParam = "1,2,3"

        //    // 2. Monta a URL com os IDs como parâmetro de query
        //    var url = $"{_configuration["ProductService:BaseUrl"]}/api/Products/products-ids?ids={idsParam}";
        //    // Exemplo: "https://produto-service/api/products/batch?ids=1,2,3"

        //    // 3. Faz a chamada HTTP
        //    var response = await _httpClient.GetAsync(url);
        //    response.EnsureSuccessStatusCode();

        //    // 4. Desserializa a resposta
        //    var content = await response.Content.ReadAsStringAsync();
        //    var jsonResponse = await response.Content.ReadAsStringAsync();

        //    // Log para debug - REMOVA DEPOIS DE VER O FORMATO
        //    Console.WriteLine($"JSON Recebido: {jsonResponse}");
        //    var products = JsonSerializer.Deserialize<List<ProductConsumerDto>>(
        //        content,
        //        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        //    );

        //    return products;

        //}
    }
}
