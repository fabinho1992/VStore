namespace UserApi.Domain.Events
{
    public class LoginEvent
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public DateTime Expired { get; set; }
    }
}
