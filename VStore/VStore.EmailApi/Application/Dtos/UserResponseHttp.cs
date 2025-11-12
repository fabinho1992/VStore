namespace VStore.EmailApi.Application.Dtos
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class UserResponseHttp
    {
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
    }
}
