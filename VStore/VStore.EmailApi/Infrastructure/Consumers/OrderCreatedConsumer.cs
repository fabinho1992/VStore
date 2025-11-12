using MassTransit;
using VStore.EmailApi.Domain.Interfaces;
using VStore.Shared.Contracts.Events;

namespace VStore.EmailApi.Infrastructure.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent> // ← CORRIGIDO AQUI
    {
        private readonly ISendEmail _sendEmail;
        private readonly ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(ISendEmail sendEmail, ILogger<OrderCreatedConsumer> logger)
        {
            _sendEmail = sendEmail;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            try
            {
                _logger.LogInformation($"Recebido evento de pedido criado: #{context.Message.Id}");

                // Envia o email de confirmação
                await _sendEmail.OrderConfimed(context.Message);

                _logger.LogInformation($"Processamento concluído para o pedido #{context.Message.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao processar pedido #{context.Message?.Id ?? 0}");
                throw;
            }
        }
    }
}