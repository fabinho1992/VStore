using Microsoft.AspNetCore.Identity;

namespace UserApi.Domain
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public string ResetToken { get; set; } = string.Empty;
        public DateTimeOffset ResetTokenExpiration { get; set; }
    }
}
