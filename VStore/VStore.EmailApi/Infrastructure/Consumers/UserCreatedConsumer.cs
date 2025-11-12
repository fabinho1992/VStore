using MassTransit;
using VStore.EmailApi.Domain.Interfaces;
using VStore.Shared.Contracts.Events;

namespace VStore.EmailApi.Infrastructure.Consumers
{
    
        public class UserCreatedConsumer : IConsumeUserCreated
        {
            private readonly ISendEmail _sendEmailService;
            private readonly ILogger<UserCreatedConsumer> _logger;

            public UserCreatedConsumer(ISendEmail sendEmailService, ILogger<UserCreatedConsumer> logger)
            {
                _sendEmailService = sendEmailService;
                _logger = logger;
            }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation($"📨 Recebido evento UserCreated para: {message.Email}");

            try
            {
                // ✅ Log antes de enviar
                _logger.LogInformation($"🔄 Iniciando envio de email para: {message.Email}");

                await _sendEmailService.SendWelcomeEmail(message.UserId, message.Email, message.UserName);

                // ✅ Log de sucesso
                _logger.LogInformation($"✅ Email enviado com sucesso para: {message.Email}");
                _logger.LogInformation($"Detalhes do usuário: ID={message.UserId}, Nome={message.UserName}, Email={message.Email}");
                _logger.LogInformation($"Timestamp do evento: {context}");
            }
            catch (Exception ex)
            {
                // ✅ Log de erro detalhado
                _logger.LogError(ex, $"❌ ERRO ao enviar email para: {message.Email}");
                throw;
            }
        }
    }
   }

