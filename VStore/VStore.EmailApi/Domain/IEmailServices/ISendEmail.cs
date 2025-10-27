using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.Domain.Interfaces.IEmailServices
{
    public interface ISendEmail
    {
        Task SendWelcomeEmail(Guid userId, string email, string userName);
        Task SendEmailConfirmation(Guid id);
        Task ResetPassword(string email, string name, string code);
    }
}
