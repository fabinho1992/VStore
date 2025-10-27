
using MimeKit;
using System.Net;
using System.Net.Mail;
using UserApi.Domain.Interfaces.IEmailServices;

namespace UserApi.Infrastructure.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        public async Task SendEmailService(string subject, string toEmail, string userName, string message)
        {
            try
            {
                _logger.LogInformation($"📧 Preparando email para: {toEmail}");

                var smtpServer = _config["SmtpSettings:Server"];
                var smtpPort = int.Parse(_config["SmtpSettings:Port"]);
                var smtpUsername = _config["SmtpSettings:User"];
                var smtpPassword = _config["SmtpSettings:Pass"];

                // Crie o email com MailKit
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress("VStore", smtpUsername));
                mimeMessage.To.Add(new MailboxAddress(userName, toEmail));
                mimeMessage.Subject = subject;

                // Corpo do email em HTML
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = message;
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                // Envie com MailKit
                using var client = new MailKit.Net.Smtp.SmtpClient();

                _logger.LogInformation($"🔗 Conectando ao SMTP: {smtpServer}:{smtpPort}");

                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);

                _logger.LogInformation($"📤 Enviando email para: {toEmail}");

                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"✅ Email enviado com sucesso para: {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Erro ao enviar email para: {toEmail}");
                throw;
            }
        }
    }
}
