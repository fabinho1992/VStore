using Microsoft.AspNetCore.Identity;
using UserApi.Domain;
using UserApi.Domain.Interfaces.IEmailServices;

namespace UserApi.Infrastructure.EmailService
{
    public class SendEmail : ISendEmail
    {
        
        private readonly IEmailService _emailService;

        public SendEmail(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendWelcomeEmail(Guid userId, string email, string userName)
        {
            var subject = "Bem-vindo à VStore!";
            var message = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background: #ff6b35; color: white; padding: 20px; text-align: center;'>
                <h1>🛍️ VStore</h1>
            </div>
            <div style='padding: 20px; background: #f9f9f9;'>
                <h2>Olá, {userName}!</h2>
                <p>Bem-vindo(a) à VStore! Sua conta foi criada com sucesso.</p>
                <p>Estamos felizes em tê-lo(a) conosco. Comece a explorar nossos produtos:</p>
                <div style='text-align: center; margin: 20px 0;'>
                    <a href='https://vstore.com' 
                       style='background: #ff6b35; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>
                        Visitar Loja
                    </a>
                </div>
                <p>Atenciosamente,<br/><strong>Equipe VStore</strong></p>
            </div>
        </div>";

            await _emailService.SendEmailService(
                subject: subject,
                toEmail: email,
                userName: userName,
                message: message
            );
        }

        public Task ResetPassword(string email, string name, string code)
        {
            throw new NotImplementedException();
        }

        public async Task SendEmailConfirmation(Guid id)
        {
            
                throw new ArgumentException("Usuário não encontrado");

             
        }
    }
}
