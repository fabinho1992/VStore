using System.Text.Json;
using VStore.EmailApi.Application.Dtos;

namespace VStore.EmailApi.Application.UserHttpClient
{
    public class HttpClientUser : IHttpClientUser
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientUser> _logger;
        private readonly IConfiguration _configuration;

        public HttpClientUser(HttpClient httpClient, ILogger<HttpClientUser> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<UserResponseHttp> GetUserByIdAsync(Guid userId)
        {
            try
            {
                _logger.LogInformation($"🔍 Buscando usuário {userId} no User API");

                var url = $"{_configuration["OrderService:BaseUrl"]}/api/Auth/{userId}";
                _logger.LogInformation($"🔗 URL: {url}");

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"✅ Resposta recebida - Status: {response.StatusCode}");
                    _logger.LogInformation($"📄 Conteúdo JSON: {content}");

                    // 👇 DESSERIALIZE PARA A ESTRUTURA CORRETA 👇
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<UserResponseHttp>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (apiResponse?.Data == null)
                    {
                        _logger.LogError($"❌ Dados do usuário não encontrados na resposta");
                        return null;
                    }

                    var user = apiResponse.Data;
                    _logger.LogInformation($"👤 Usuário encontrado - Email: '{user.Email ?? "NULL"}', DisplayName: '{user.DisplayName ?? "NULL"}'");

                    return user;
                }

                _logger.LogWarning($"⚠️ Usuário {userId} não encontrado. Status: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Erro ao buscar usuário {userId}");
                return null;
            }
        }
    }
}